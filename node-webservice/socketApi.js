var socket_io = require('socket.io');
var io = socket_io();
var socketApi = {};
var GameID = require('./js/GameID.js');

socketApi.io = io;

var currentConnections = 0;
io.on('connection', function(socket){
    currentConnections += 1;
    console.log("user connected | " + currentConnections);

    var currentGame = "no-game";
    function joinGame(gameID) {
        // leave any room already joined
        socket.leaveAll();
    
        // join the specified room
        socket.join(gameID);
        currentGame = gameID;
    }

    socket.on('test-connection', function() {
        socket.emit("socket.io api available", getTime());
    });

    // when user creates a new game, generate a game id and join that game
    socket.on('newgame', function(onJoinGame) {
        var gameID = GameID.GenerateGameID(5);
        joinGame(gameID);
        if (onJoinGame != null) {
            onJoinGame(gameID);
        }
    });

    // when user joins a game, specifying a game ID
    socket.on('joingame', function (gameID, onJoinGame) {
        joinGame(gameID);
        onJoinGame(gameID);
    });

    // forward game messages to all clients in the current game
    socket.on('game-message', function (messageType, messageData, onBroadcastSent) {
        io.to(currentGame).emit('game-message', messageType, messageData);
        if (onBroadcastSent != null) {
            onBroadcastSent();
        }
    });

    // when user disconnects
    socket.on('disconnect', function () {
        currentConnections -= 1;
       console.log("user disconnected | " + currentConnections);
    });
});

module.exports = socketApi;
