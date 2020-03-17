var chai = require('chai');
var expect = chai.expect;

var GameID = require('../js/GameID.js');

describe("GameID.js", () => {
    describe("GenerateGameID()", () => {
        it ("should generate strings", () => {
            var result = GameID.GenerateGameID(5);
            expect(result).to.be.a('string');
        })
        it ("should generate a string of size 5", () => {
            var result = GameID.GenerateGameID(5);
            expect(result).to.have.length(5);
        });

        it ("should generate a string of size 0", () => {
            var result = GameID.GenerateGameID(0);
            expect(result).to.have.length(0);
        });

        it ("should generate a string of size 100", () => {
            var result = GameID.GenerateGameID(100);
            expect(result).to.have.length(100);
        });

        it ("should generate a string witout lowercase letters", () => {
            var gameID = GameID.GenerateGameID(1000);
            expect(gameID).to.equal(gameID.toUpperCase());
        });

        it ("should generate a different string when called twice", () => {
            var gameID1 = GameID.GenerateGameID(5);
            var gameID2 = GameID.GenerateGameID(5);
            expect(gameID1).not.to.equal(gameID2);
        });
    });
});
