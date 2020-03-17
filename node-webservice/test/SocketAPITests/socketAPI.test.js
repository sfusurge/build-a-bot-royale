var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;

describe("Socket.io API", () => {
    it ("receives the beep-boop test message", async () => {
        const socketClient = await CreateSocketClient();

        const boopResponse = await new Promise(resolve => {
            socketClient.emit("beep", resolve);
        });
        expect(boopResponse.message).to.equal("boop");

        await DestroySocketClient(socketClient);
    });
});
