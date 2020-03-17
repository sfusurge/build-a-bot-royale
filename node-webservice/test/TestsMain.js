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
    return new Promise((resolve, reject) => {
        // check if socket connected
        if (socket.connected) {
            // disconnect socket
            socket.disconnect();
            resolve(true);
        } else {
            // not connected
            resolve(false);
        }
    });
  };

module.exports = { TEST_PORT, CreateSocketClient, DestroySocketClient }
