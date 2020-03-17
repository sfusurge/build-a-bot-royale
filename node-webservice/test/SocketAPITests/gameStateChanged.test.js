var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("gameStateChanged", () => {
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

    it ("host receives gameStateChanged message when it changes the game state", async function() {
        await Promise.all([
            new Promise(resolve => {
                gameHostClient.on("gameStateChanged", messageData => {
                    if (messageData.gameState === "testingState") { // this check if to filter out the 'initial' state
                        resolve();
                    }
                });
            }),
            SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "testingState" })
        ]);
    });

    it ("player receives gameStateChanged message when host changes the game state", async function() {
        playerClient = await CreateSocketClient();
        await SendSocketMessage(playerClient, "joingame", { gameID: createdGameID, username: "testing-user" });

        await Promise.all([
            new Promise(resolve => {
                playerClient.on("gameStateChanged", async messageData => {
                    expect(messageData).to.have.property("gameState");
                    expect(messageData.gameState).to.equal("cool-state");
                    resolve();
                });
            }),
            SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "cool-state" })
        ]);

        await DestroySocketClient(playerClient);
    });

    it ("100+ connected players receive gameStateChanged message", async function() {
        const playerSockets = [];

        const numberOfPlayersToConnect = 101;
        for (var playerIndex = 0; playerIndex < numberOfPlayersToConnect; playerIndex++) {
            const playerClient = await CreateSocketClient();
            await SendSocketMessage(playerClient, "joingame", { gameID: createdGameID, username: "user_" + playerIndex });
            playerSockets.push(playerClient);
        }

        const responses = await Promise.all([
            ...playerSockets.map(socket => {
                return new Promise(resolve => {
                    socket.on("gameStateChanged", resolve);
                });
            }),
            SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "testingState" })
        ]);
        responses.forEach((response, responseIndex) => {
            if (responseIndex != responses.length - 1) {
                expect(response.gameState).to.equal("testingState");
            }
        });

        playerSockets.forEach(async socket => await DestroySocketClient(socket));
    });
});
