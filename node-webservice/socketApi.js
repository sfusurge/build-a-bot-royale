var socket_io = require('socket.io');
var io = socket_io();
var socketApi = {};
var GameID = require('./js/GameID.js');
var GameUtils = require('./js/GameUtils')(io);

socketApi.io = io;

var currentConnections = 0;
io.on('connection', function(socket){
    currentConnections += 1;
    console.log("user connected | " + currentConnections);

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

        // join the generated game id
        joinGame(gameID);
        if (onJoinGame != null) {
            onJoinGame(gameID);
        }
    });

    // when user joins a game, specifying a game ID
    socket.on('joingame', function (gameID, ack) {
        if (GameUtils.CanPlayerJoinGame(gameID)) {
            joinGame(gameID);
            ack(null); // ack with null to say no error
        } else {
            ack("Game " + gameID + " hasn't been created");
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
