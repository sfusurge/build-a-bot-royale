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

    it ("host receives game-message when player sends one", async function() {
        const playerClient = await CreateSocketClient();
        await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" });

        const [receivedGameMessage, sentMessageResponse] = await Promise.all([
            new Promise(resolve => gameHostClient.on("game-message", resolve)),
            SendSocketMessage(playerClient, 'game-message', { action: "special-testing-action", otherProperty: "heyhey123" })
        ]);

        expect(receivedGameMessage.action).to.equal("special-testing-action");
        expect(receivedGameMessage.username).to.equal("thisUser");
        expect(receivedGameMessage.otherProperty).to.equal("heyhey123");

        await DestroySocketClient(playerClient);
    });

    it ("player receives game-message when host sends one", async function() {
        const playerClient = await CreateSocketClient();
        await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" });

        const [receivedGameMessage, sentMessageResponse] = await Promise.all([
            new Promise(resolve => playerClient.on("game-message", resolve)),
            SendSocketMessage(gameHostClient, "game-message", { action: "hi there", extraProperty: "hello world" })
        ]);

        expect(receivedGameMessage.action).to.equal("hi there");

        expect(receivedGameMessage.username).to.contain(createdGameID);
        expect(receivedGameMessage.username).to.contain("game_host");

        await DestroySocketClient(playerClient);
    });

    it ("100+ players receive game-message when host sends one", async function() {
        const playerSockets = [];

        const playersToConnect = 101;
        for (var playerIndex = 0; playerIndex < playersToConnect; playerIndex += 1) {
            const playerSocket = await CreateSocketClient();
            await SendSocketMessage(playerSocket, 'joingame', { gameID: createdGameID, username: "gnereatedUser" + playerIndex });
            playerSockets.push(playerSocket);
        }

        const responses = await Promise.all([
            ...playerSockets.map(socket => {
                return new Promise(resolve => {
                    socket.on('game-message', resolve);
                });
            }),
            SendSocketMessage(gameHostClient, "game-message", { action: "greetings_everyone", extraData: "heya" })
        ]);

        responses.forEach((response, responseIndex) => {
            if (responseIndex !== responses.length - 1) { // the last response is for the socket message to send the game-message
                expect(response.action).to.equal("greetings_everyone");
                expect(response.extraData).to.equal("heya");
                expect(response.username).to.contain("host");
            }
        });

        playerSockets.forEach(async socket => DestroySocketClient(socket));
    });
});
