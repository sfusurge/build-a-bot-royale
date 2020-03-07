var socket_io = require('socket.io');
var io = socket_io();
var socketApi = {};
var GameID = require('./js/GameID.js');
var GameUtils = require('./js/GameUtils')(io);

socketApi.io = io;

var currentConnections = 0;
var totalConnections = 0;
io.on('connection', function(socket){
    currentConnections += 1;
    totalConnections += 1;
    console.log("user connected | " + currentConnections);

    // set default username
    socket.username = "user" + totalConnections;

    // test messages to check that socket.io service is working
    var numberOfBeeps = 0;
    socket.on('beep', function() {
        console.log("beeped: " + numberOfBeeps++);
        socket.emit("boop", {});
    });

    var currentGame = "no-game";
    function joinGame(gameID) {
        // leave any room already joined
        socket.leaveAll();
    
        // join the specified room
        socket.join(gameID);
        currentGame = gameID;
    }

    function changeGameState(newGameState) {
        var room = io.sockets.adapter.rooms[currentGame];
        room.gameState = newGameState;

        io.to(currentGame).emit('gameStateChanged', { gameState: room.gameState });   
    }

    // when user creates a new game, generate a game id and join that game
    socket.on('newgame', function(ack) {
        // generate game id. Regenerate the id if another game with that ID is already happening
        var gameID = null;
        while (gameID === null || GameUtils.IsAnotherClientInGame(gameID)) {
            gameID = GameID.GenerateGameID(5);
        }

        // change this clients username to show that its a host
        socket.username = "game_host_" + gameID;
        socket.isHost = true;

        // join the generated game id
        joinGame(gameID);
        if (ack != null) {
            ack({ gameID: gameID });
        }

        // set the initial game state
        changeGameState("initial");            
    });

    // when user joins a game, specifying a game ID and username
    socket.on('joingame', function (data, ack) {
        var gameID = data.gameID;
        var username = data.username;

        try {
            // check that game id and username are valid
            GameUtils.ValidatePlayerCanJoinGame(gameID, username);

            // set the username for the socket connection
            socket.username = username;
            socket.isHost = false;            

            // join the game and acknowledge to client
            joinGame(gameID);
            var room = io.sockets.adapter.rooms[currentGame];
            ack({ gameState: room.gameState }); // game successfully joined, ack with the current state of the game
            
            // send message to game clients saying this user connected
            io.to(currentGame).emit('playerConnect', { username: socket.username });
        } catch (error) {
            ack({ error: error.message }); // was not able to join game, so ack with the error message
        }
    });

    socket.on('updateGameState', function(data, ack) {
        if (!socket.isHost) {         
            if (ack) {
                ack({ error: "Not allowed to change the game state" });
            }
            return;
        }      
        var newState = data.gameState;
        changeGameState(newState);
        if (ack) {
            ack({ message: "ok" });   
        } 
    });

    // forward game messages to all clients in the current game
    socket.on('game-message', function (messageData, ack) {
        try {
            if (typeof messageData !== 'object')
            {
                throw "messagedata is of type " + typeof messageData + " but has to be an object";
            }

            // add username to message
            messageData.username = socket.username;

            io.to(currentGame).emit('game-message', messageData);
            if (ack) {
                ack({ message: "ok" });
            }
        } catch (e) {
            console.log("Error sending message: " + e);
            if (ack) {
                ack({ error: e });
            }
        }
    });

    // when user disconnects
    socket.on('disconnect', function () {
        currentConnections -= 1;
        console.log("user disconnected | " + currentConnections);

        // send message to game clients saying this user disconnected
        if (currentGame && currentGame !== "no-game") {
            io.to(currentGame).emit('playerDisconnect', { username: socket.username });
        }
    });
});

module.exports = socketApi;
