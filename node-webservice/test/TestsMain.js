var chai = require('chai');
var expect = chai.expect;

var server = require('../bin/testServer.js');
const TEST_PORT = 9090;

// start and stop the server between each test
beforeEach(done => {
    server.listen(TEST_PORT, () => {
        done();
    });
})

afterEach(done => {
    server.close(() => {
        done();
    });
});

module.exports = { TEST_PORT }
