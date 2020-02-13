var socket_io = require('socket.io');
var io = socket_io();
var socketApi = {};

socketApi.io = io;

io.on('connection', function(socket){
    console.log("user connected");
    var currentRoom = "no-room";

    socket.on('message', function(msg){
        console.log("message");
      io.to(currentRoom).emit('message', msg);
    });

    socket.on('joingame', function (gameID) {
        socket.leaveAll();
        socket.join(gameID);
        currentRoom = gameID;
        socket.emit("connected-to-gameid", gameID);
    });
});

module.exports = socketApi;