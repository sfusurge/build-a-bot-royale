var chai = require('chai');
var expect = chai.expect;

var io = require('socket.io-client');

const PORT = require('./TestsMain').TEST_PORT;

describe("Socket.io API", () => {

    var socketClient;
    beforeEach(done => {
        // use a new socket client for each test
        socketClient = io('http://localhost:' + PORT);

        socketClient.on('connect', data => {
            done();
        });
    });

    it ("receives the beep-boop test message", done => {
        socketClient.emit("beep", response => {
            expect(response.message).to.equal("boop");
            done();
        });
        expect(socketClient).to.not.be.null;
    });

    it ("responds to newgame with a game ID", done => {
        socketClient.emit('newgame', response => {
            expect(response).to.not.haveOwnProperty("error");
            expect(response).to.haveOwnProperty('gameID');
            expect(response.gameID).to.be.a('string');
            expect(response.gameID.length).to.equal(5);
            done();
        });
    });
});
