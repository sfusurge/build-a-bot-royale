var chai = require('chai');
var expect = chai.expect;

var io = require('socket.io-client');

var server = require('../bin/testServer.js');
const TEST_PORT = 9090;

// start and stop the server between each test
beforeEach(done => {
    server.listen(TEST_PORT, () => {
        done();
    });
})

afterEach(done => {
    server.close(() => {
        done();
    });
});

// provide functions to test files for socket connections
const CreateSocketClient = () => {
    return new Promise((resolve, reject) => {
        // create socket connection
        const socket = io('http://localhost:' + TEST_PORT, {
            "reconnection delay": 0,
            "reopen delay": 0,
            "force new connection": true
        });

        // wait for socket to connect
        socket.on("connect", () => {
            resolve(socket);
        });
    });
};
  
const DestroySocketClient = socket => {
    ValidateSocket(socket);
    return new Promise((resolve, reject) => {
        if (socket.connected) {
            socket.disconnect();
            resolve(true);
        } else {
            resolve(false);
        }
    });
};

const SendSocketMessage = (socket, messageName, messageData) => {
    ValidateSocket(socket);
    return new Promise(resolve => {
        if (messageData) {
            socket.emit(messageName, messageData, resolve);
        } else {
            socket.emit(messageName, resolve);
        }
    });
};

const ValidateSocket = (socket) => {
    if (typeof socket !== 'object' || !socket.emit) {
        throw new Error("Invalid socket passed in");
    }
}

module.exports = { TEST_PORT, CreateSocketClient, DestroySocketClient, SendSocketMessage }
