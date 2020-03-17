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

    it ("host can change the game state", async () => {
        const messageData = {
            gameState: "best-state"
        };
        const updateStateResponse = await SendSocketMessage(gameHostClient, 'updateGameState', messageData);
        expect(updateStateResponse).to.not.have.property("error");
        expect(updateStateResponse).to.have.property("message");
        expect(updateStateResponse.message).to.be.a("string");
    });

    it ("player cannot change the game state", async () => {
        playerClient = await CreateSocketClient();
        const joinResponse = await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "best-user" });
        expect(joinResponse).to.not.have.property("error");
        
        const messageData = {
            gameState: "best-state"
        };
        const updateStateResponse = await SendSocketMessage(playerClient, 'updateGameState', messageData);
        expect(updateStateResponse).to.have.property("error");

        await DestroySocketClient(playerClient);
    });

    it ("changing the state makes any player joining see that state first", async () => {
        await SendSocketMessage(gameHostClient, 'updateGameState', { gameState: "best-state" });

        playerClient = await CreateSocketClient();

        const joinResponse = await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "best-user" });
        expect(joinResponse.gameState).to.equal("best-state");

        await DestroySocketClient(playerClient);
    });
});
