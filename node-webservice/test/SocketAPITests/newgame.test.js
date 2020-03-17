var chai = require('chai');
var expect = chai.expect;

const TestUtils = require('../testUtils');
const CreateSocketClient = TestUtils.CreateSocketClient;
const DestroySocketClient = TestUtils.DestroySocketClient;

describe("newgame", () => {
    it ("responds with a game ID", async () => {
        const socketClient = await CreateSocketClient();

        const newGameResponse = await new Promise(resolve => {
            socketClient.emit('newgame', resolve);
        });

        expect(newGameResponse).to.not.have.property("error");
        expect(newGameResponse).to.have.property('gameID');
        expect(newGameResponse.gameID).to.be.a('string');
        expect(newGameResponse.gameID.length).to.equal(5);

        await DestroySocketClient(socketClient);
    });

    it ("responds with different game IDs when called twice", async () => {
        const socketClient = await CreateSocketClient();

        const newGameResponse1 = await new Promise(resolve => {
            socketClient.emit('newgame', resolve);
        });

        const newGameResponse2 = await new Promise(resolve => {
            socketClient.emit('newgame', resolve);
        });

        expect(newGameResponse1.gameID).to.not.equal(newGameResponse2.gameID);

        await DestroySocketClient(socketClient);
    });
});
