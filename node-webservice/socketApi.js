var socket_io = require('socket.io');
var io = socket_io();
var socketApi = {};

socketApi.io = io;

io.on('connection', function(socket){
    console.log("user connected");
    var currentRoom = "no-room";

    // when user sends a message
    socket.on('message', function(messageContent){
        // forward the sent message to everyone in the current room
        io.to(currentRoom).emit('message', messageContent);
    });

    // when user joins a game, specifying a game ID
    socket.on('joingame', function (gameID) {
        // leave any room already joined
        socket.leaveAll();

        // join the specified room
        socket.join(gameID);
        currentRoom = gameID;

        // send message back to client saying they have joined this room
        socket.emit("connected-to-gameid", gameID);
    });

    // when user disconnects
    socket.on('disconnect', function () {
       console.log("user disconnected");
    });
});

module.exports = socketApi;