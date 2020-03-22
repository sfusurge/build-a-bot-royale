var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("playerDisconnect", () => {
    var createdGameID;
    var gameHostClient;
    beforeEach(async () => {
        gameHostClient = await CreateSocketClient();
        const newGameResponse = await SendSocketMessage(gameHostClient, 'newgame');
        createdGameID = newGameResponse.gameID;
    });

    afterEach(async () => {
        await DestroySocketClient(gameHostClient);
    });

    it ("host receives playerDisconnect when player disconnects", async function() {
        const playerClient = await CreateSocketClient();
        await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" });

        await Promise.all([
            new Promise(resolve => {
                gameHostClient.on('playerConnect', messageData => {
                    expect(messageData).to.have.property("username");
                    expect(messageData.username).to.equal("thisUser");
                    resolve();
                });
            }),
            DestroySocketClient(playerClient)
        ]);
    });
});
