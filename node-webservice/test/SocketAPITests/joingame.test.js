var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;
const SendSocketMessage = TestUtils.SendSocketMessage;

describe("joingame", () => {
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

    it ("can join a game that has been created", async () => {
        playerClient = await CreateSocketClient();

        const messageData = {
            gameID: createdGameID,
            username: "Tester1"
        }
        const joinResponse = await SendSocketMessage(playerClient, 'joingame', messageData);

        expect(joinResponse).to.not.have.property("error");
        expect(joinResponse).to.have.property("gameState");
        expect(joinResponse.gameState).to.be.a("string");

        await DestroySocketClient(playerClient);
    });

    it ("responds with an error when missing gameID from message data", async () => {
        playerClient = await CreateSocketClient();

        const messageData = {
            username: "Tester1"
        }
        const joinResponse = await SendSocketMessage(playerClient, 'joingame', messageData);

        expect(joinResponse).to.have.property("error");

        await DestroySocketClient(playerClient);
    });

    it ("responds with an error when missing username from message data", async () => {
        playerClient = await CreateSocketClient();

        const messageData = {
            gameID: createdGameID
        }
        const joinResponse = await SendSocketMessage(playerClient, 'joingame', messageData);

        expect(joinResponse).to.have.property("error");

        await DestroySocketClient(playerClient);
    });

    it ("responds with an error when joining a game that hasn't been created", async () => {
        playerClient = await CreateSocketClient();

        const messageData = {
            gameID: "BadGm",
            username: "Tester1"
        }
        const joinResponse = await SendSocketMessage(playerClient, 'joingame', messageData);

        expect(joinResponse).to.have.property("error");

        await DestroySocketClient(playerClient);
    });

    it ("allow two users with different usernames to join", async () => {
        playerClient1 = await CreateSocketClient();
        playerClient2 = await CreateSocketClient();

        const messageData1 = {
            gameID: createdGameID,
            username: "BestUsername"
        }
        const joinResponse1 = await SendSocketMessage(playerClient1, 'joingame', messageData1);
        expect(joinResponse1).to.not.have.property("error");

        const messageData2 = {
            gameID: createdGameID,
            username: "OtherUsername"
        }
        const joinResponse2 = await SendSocketMessage(playerClient2, 'joingame', messageData2);
        expect(joinResponse2).to.not.have.property("error");

        await DestroySocketClient(playerClient1);
        await DestroySocketClient(playerClient2);
    });

    it ("doesn't allow two users to join with the same username", async () => {
        playerClient1 = await CreateSocketClient();
        playerClient2 = await CreateSocketClient();

        const messageData1 = {
            gameID: createdGameID,
            username: "BestUsername"
        }
        const joinResponse1 = await SendSocketMessage(playerClient1, 'joingame', messageData1);
        expect(joinResponse1).to.not.have.property("error");

        const messageData2 = {
            gameID: createdGameID,
            username: "BestUsername"
        }
        const joinResponse2 = await SendSocketMessage(playerClient2, 'joingame', messageData2);
        expect(joinResponse2).to.have.property("error");

        await DestroySocketClient(playerClient1);
        await DestroySocketClient(playerClient2);
    });

    it ("allows over 100 players to join", async () => {
        const playerSockets = [];

        for (var playerIndex = 0; playerIndex < 101; playerIndex += 1) {
            const playerSocket = await CreateSocketClient();
            playerSockets.push(playerSocket);

            const messageData = {
                gameID: createdGameID,
                username: "generated_user" + playerIndex
            }

            const joinResponse = await SendSocketMessage(playerSocket, 'joingame', messageData);

            expect(joinResponse).to.not.have.property("error");
        }

        playerSockets.forEach(async socket => await DestroySocketClient(socket));
    });
});
