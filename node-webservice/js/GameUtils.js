var GameUtils = (socketIO) => ({
    TestGameIds: ["TEST0", "TEST1", "12345"],
    CanPlayerJoinGame: function(gameID) {
        // always allow players to join the test games
        if (this.TestGameIds.includes(gameID)) {
            return true;
        }

        // only allow player to join the game if at least one other client is already there
        return this.IsAnotherClientInGame(gameID);
    },
    IsAnotherClientInGame: function(gameID) {
        return this.NumberOfClientsInGame(gameID) > 0;
    },
    NumberOfClientsInGame: function(gameID) {
        var room = socketIO.sockets.adapter.rooms[gameID];
        if (!room) {
            return 0;
        }
        return room.length;
    }
});

module.exports = GameUtils;
