function GenerateGameID(length) {
    var result           = '';
    var characters       = 'ABCDEFGHIJKLMNPQRSTUVWXYZ123456789'; // all letters and number, but with 0 and O removed
    var charactersLength = characters.length;
    for ( var i = 0; i < length; i++ ) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}

module.exports = { GenerateGameID }; 
