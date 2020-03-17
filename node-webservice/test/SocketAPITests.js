var chai = require('chai');
var expect = chai.expect;

var io = require('socket.io-client');

const PORT = require('./TestsMain').TEST_PORT;

const initSocket = () => {
    return new Promise((resolve, reject) => {
      // create socket for communication
      const socket = io('http://localhost:' + PORT, {
        "reconnection delay": 0,
        "reopen delay": 0,
        "force new connection": true
      });
  
      // define event handler for sucessfull connection
      socket.on("connect", () => {
        resolve(socket);
      });
    });
  };
  
  const destroySocket = socket => {
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


describe("Socket.io API", () => {
/*
    var socketClient;
    beforeEach(done => {
        // use a new socket client for each test
        socketClient = io('http://localhost:' + PORT);

        socketClient.on('connect', data => {
            done();
        });
    });*/

    it ("receives the beep-boop test message", async () => {
        const socketClient = await initSocket();
        const sendMessage = new Promise((resolve, reject) => {
            socketClient.emit("beep", resolve);
            setTimeout(() => {
                reject(new Error("Failed to get reponse, connection timed out..."));
            }, 2000);
        });
        const response = await sendMessage;
        expect(response.message).to.equal("boop");

        await destroySocket(socketClient);
        /*
        socketClient.emit("beep", response => {
            expect(response.message).to.equal("boop");
        });*/
        //expect(socketClient).to.not.be.null;
    });
/*
    it ("responds to newgame with a game ID", done => {
        socketClient.emit('newgame', response => {
            expect(response).to.not.haveOwnProperty("error");
            expect(response).to.haveOwnProperty('gameID');
            expect(response.gameID).to.be.a('string');
            expect(response.gameID.length).to.equal(5);
            done();
        });
    });
    */
});
