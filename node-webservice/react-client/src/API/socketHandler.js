import openSocket from 'socket.io-client';

var socketURL = process.env.NODE_ENV === 'production' ? 'https://build-a-bot-royale.herokuapp.com/' : 'http://localhost:9000';
const socket = openSocket(socketURL);

export default socket;
