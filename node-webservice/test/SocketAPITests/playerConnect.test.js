var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("playerConnect", () => {
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

    it ("host receives playerConnect when player connects", done => {
        gameHostClient.on('playerConnect', messageData => {
            expect(messageData).to.have.property("username");
            expect(messageData.username).to.equal("thisUser");
            done();
        });

        (async () => {
            const playerClient = await CreateSocketClient();
            await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" });
            await DestroySocketClient(playerClient);
        })();
    });

    it ("host receives playerConnect when >100 players connect", done => {
        var numberOfPlayersToConnect = 101;

        var playerConnectMessagesSeen = 0;
        gameHostClient.on('playerConnect', messageData => {
            playerConnectMessagesSeen += 1;
            if (playerConnectMessagesSeen >= numberOfPlayersToConnect) {
                done();
            }
        });

        (async () => {
            const playerSockets = [];
            for (var playerIndex = 0; playerIndex < numberOfPlayersToConnect; playerIndex += 1) {
                const playerClient = await CreateSocketClient();
                playerSockets.push(playerClient);
                await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "generated_user" + playerIndex });
            }

            playerSockets.forEach(async socket => await DestroySocketClient(socket));
        })();
    });
});
