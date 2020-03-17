var chai = require('chai');
var expect = chai.expect;

var io = require('socket.io-client');

const TestUtils = require('./TestsMain');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;

describe("Socket.io API", () => {
    it ("receives the beep-boop test message", async () => {
        const socketClient = await CreateSocketClient();
        const sendMessage = new Promise((resolve, reject) => {
            socketClient.emit("beep", resolve);
            setTimeout(() => {
                reject(new Error("Failed to get reponse, connection timed out..."));
            }, 2000);
        });
        const response = await sendMessage;
        expect(response.message).to.equal("boop...?");

        await DestroySocketClient(socketClient);
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
