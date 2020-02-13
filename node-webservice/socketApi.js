var socket_io = require('socket.io');
var io = socket_io();
var socketApi = {};

socketApi.io = io;

var games = {};

io.on('connection', function(socket){
    console.log('A user connected: ' + socket.author);
});

io.on('message', function(data) {
    const { chatRoomName, author, message } = data;
    console.log("message!");
    if (message == "newgame") {
        console.log("new game!!!!");
    }
});

socketApi.sendNotification = function() {
    io.sockets.emit('hello', {msg: 'Hello World!'});
}

module.exports = socketApi;