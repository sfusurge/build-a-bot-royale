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

        // emit joined game message
        socket.to(currentGame).emit(
            "playerjoined",
            {
                players: GameUtils.NumberOfClientsInGame(currentGame) - 1  // -1 because one client is the Unity app, which is not a player
            }
        );
    }

    // when user creates a new game, generate a game id and join that game
    socket.on('newgame', function(onJoinGame) {
        // generate game id. Regenerate the id if another game with that ID is already happening
        var gameID = null;
        while (gameID === null || GameUtils.IsAnotherClientInGame(gameID)) {
            gameID = GameID.GenerateGameID(5);
        }

        // change this clients username to show that its a host
        socket.username = "game_host_" + gameID;

        // join the generated game id
        joinGame(gameID);
        if (onJoinGame != null) {
            onJoinGame(gameID);
        }
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

            // join the game and acknowledge to client
            joinGame(gameID);
            ack(null); // ack with null to say no error            
        } catch (error) {
            ack(error.message); // was not able to join game, so ack with the error message
        }
    });

    // forward game messages to all clients in the current game
    socket.on('game-message', function (messageData, ack) {
        try {
            if (typeof messageData !== 'object')
            {
                throw "messagedata is of type " + typeof messageData + " but has to be an object";
            }

            io.to(currentGame).emit('game-message', messageData);
            if (ack != null) {
                ack();
            }
        } catch (e) {
            console.log("Error sending message: " + e);
            ack(e);
        }
    });

    // when user disconnects
    socket.on('disconnect', function () {
        currentConnections -= 1;
       console.log("user disconnected | " + currentConnections);
    });
});

module.exports = socketApi;
