var app = require('../app');
var http = require('http');

var server = http.createServer(app);

/**
 * Socket.io
 */
var socketApi = require('../socketApi');
var io = socketApi.io;
io.attach(server);

module.exports = server;
