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

    it ("host receives playerConnect when player connects", async function() {
        const playerClient = await CreateSocketClient();

        await Promise.all([
            new Promise(resolve => {
                gameHostClient.on('playerConnect', messageData => {
                    expect(messageData).to.have.property("username");
                    expect(messageData.username).to.equal("thisUser");
                    resolve();
                });
            }),
            await SendSocketMessage(playerClient, 'joingame', { gameID: createdGameID, username: "thisUser" })
        ]);

        await DestroySocketClient(playerClient);
    });

    it ("host receives playerConnect when >100 players connect", async function() {
        const numberOfPlayersToConnect = 101;
        
        const players = [];
        for (let playerIndex = 0; playerIndex < numberOfPlayersToConnect; playerIndex += 1) {
            players.push(await CreateSocketClient());
        }

        await Promise.all([
            new Promise(resolve => {
                let playerConnectMessagesSeen = 0;
                gameHostClient.on('playerConnect', messageData => {
                    playerConnectMessagesSeen += 1;
                    if (playerConnectMessagesSeen >= numberOfPlayersToConnect) {
                        resolve();
                    }
                });
            }),
            ...players.map((player, playerIndex) =>
                SendSocketMessage(player,'joingame', { gameID: createdGameID, username: "generated_user" + playerIndex })
            )
        ]);

        players.forEach(async player => await DestroySocketClient(player));
    });
});
