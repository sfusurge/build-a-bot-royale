import openSocket from 'socket.io-client';

const socketURL = (() => {
    // use Node environment to pick which server to connect to
    let serverEnvironment = process.env.NODE_ENV;

    // override server choice if uri param server is specificed
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.has("server")) {
        serverEnvironment = urlParams.get("server");
    }

    switch (serverEnvironment) {
        case 'production':
            return 'https://build-a-bot-royale.herokuapp.com/';
        case 'test':
            return 'http://localhost:9009';
        default:
            return 'http://localhost:9000';
    }
})();

const socket = openSocket(socketURL);

export default socket;
