var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("updateGameState", () => {
    var createdGameID;
    var gameHostClient;
    beforeEach(async () => {
        // create a new game for each joingame test
        gameHostClient = await CreateSocketClient();
        const newGameResponse = await SendSocketMessage(gameHostClient, 'newgame');
        createdGameID = newGameResponse.gameID;
    });

    afterEach(async () => {
        await DestroySocketClient(gameHostClient);
    });

    it ("host receives gameStateChanged message when it changes the game state", done => {
        gameHostClient.on("gameStateChanged", messageData => {
            if (messageData.gameState === "testingState") { // this check if to filter out the 'initial' state
                done();
            }
        });

        (async () => {
            await SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "testingState" });
        })();
    });

    it ("player receives gameStateChanged message when host changes the game state", done => {
        (async () => {
            playerClient = await CreateSocketClient();
            await SendSocketMessage(playerClient, "joingame", { gameID: createdGameID, username: "testing-user" });

            playerClient.on("gameStateChanged", async messageData => {
                expect(messageData).to.have.property("gameState");
                expect(messageData.gameState).to.equal("cool-state");

                await DestroySocketClient(playerClient);
                done();
            });

            await SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "cool-state" });
        })();
    });

    it ("100+ connected players receive gameStateChanged message", done => {
        (async () => {
            const numberOfPlayersToConnect = 101;
            var numberOfStateChangeMessagesReceived = 0;

            const playerSockets = [];
            for (var playerIndex = 0; playerIndex < numberOfPlayersToConnect; playerIndex++) {
                playerClient = await CreateSocketClient();
                playerSockets.push(playerClient);
                await SendSocketMessage(playerClient, "joingame", { gameID: createdGameID, username: "user_" + playerIndex });
    
                playerClient.on("gameStateChanged", async messageData => {
                    expect(messageData.gameState).to.equal("next-state");
                    
                    numberOfStateChangeMessagesReceived += 1;
                    if (numberOfStateChangeMessagesReceived >= numberOfPlayersToConnect) {
                        playerSockets.forEach(async socket => await DestroySocketClient(socket));
                        done();
                    }
                });
            }

            await SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "next-state" });
        })();
    });
});
