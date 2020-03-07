var GameUtils = (socketIO) => ({
    TestGameIds: ["TEST0", "TEST1", "12345"],
    ValidateUsername: function(username, gameID) {
        if (!username) {
            throw new Error("Invalid username data");
        }

        // disallow username to be used if another socket connection in the same room is using it
        var clientsInRoom = socketIO.sockets.adapter.rooms[gameID].sockets;
        for (var clientId in clientsInRoom) {
            var clientSocket = socketIO.sockets.connected[clientId];
        
            if (clientSocket.username === username) { // throw if a socket connection has the requested username
                throw new Error("Username \"" + username + "\" has already been taken");
            }
        }
    },
    ValidateGameID: function(gameID) {
        if (!gameID) {
            throw new Error("Invalid game id data");
        }

        // dont allow a game to be joined if no other clients are there, meaning there is no host
        if (this.IsAnotherClientInGame(gameID) === false) {
            throw new Error("Game " + gameID + " hasn't been created");
        }
    },
    ValidatePlayerCanJoinGame: function(gameID, username) {
        // test games are always joinable
        if (this.TestGameIds.includes(gameID)) {
            return;
        }

        this.ValidateGameID(gameID);
        this.ValidateUsername(username, gameID);

        // GameID and username are valid for joining a game. Exit this function without throwing
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
