var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("game-message", () => {
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

    it ("host receives game-message when player sends one", done => {
        gameHostClient.on("game-message", messageData => {
            expect(messageData.action).to.equal("special-testing-action");
            expect(messageData.username).to.equal("thisUser");
            expect(messageData.otherProperty).to.equal("heyhey123");
            done();
        });

        (async () => {
            const playerClient = await CreateSocketClient();
            await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" });
            await SendSocketMessage(playerClient, 'game-message', { action: "special-testing-action", otherProperty: "heyhey123" });
            await DestroySocketClient(playerClient);
        })();
    });

    it ("player receives game-message when host sends one", done => {
        (async () => {
            const playerClient = await CreateSocketClient();
            await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" });

            playerClient.on("game-message", async messageData => {
                expect(messageData.action).to.equal("hi there");

                expect(messageData.username).to.contain(createdGameID);
                expect(messageData.username).to.contain("game_host");

                expect(messageData.extraProperty).to.equal("hello world");
                await DestroySocketClient(playerClient);
                done();
            });

            await SendSocketMessage(gameHostClient, "game-message", { action: "hi there", extraProperty: "hello world" });
        })();
    });

    it ("100+ players receive game-message when host sends one", done => {
        (async () => {
            const playerSockets = [];
            const playersToConnect = 101;
            var gameMessagesReceived = 0;
            for (var playerIndex = 0; playerIndex < playersToConnect; playerIndex += 1) {
                const playerClient = await CreateSocketClient();
                playerSockets.push(playerClient);
                await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "gnereatedUser" + playerIndex });

                playerClient.on("game-message", async messageData => {
                    expect(messageData.action).to.equal("greetings_everyone");
                    expect(messageData.extraData).to.equal("heya");
                    expect(messageData.username).to.contain("host");

                    gameMessagesReceived += 1;
                    if (gameMessagesReceived >= playersToConnect) {
                        playerSockets.forEach(async socket => await DestroySocketClient(socket));
                        done();
                    }
                });
            }

            await SendSocketMessage(gameHostClient, "game-message", { action: "greetings_everyone", extraData: "heya" });
        })();
    });
});
