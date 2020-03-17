var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("Socket.io API", () => {
    it ("receives the beep-boop test message", async () => {
        const socketClient = await CreateSocketClient();

        const boopResponse = await SendSocketMessage(socketClient, "beep");
        expect(boopResponse.message).to.equal("boop");

        await DestroySocketClient(socketClient);
    });
});
