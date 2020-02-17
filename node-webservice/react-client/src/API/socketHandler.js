import openSocket from 'socket.io-client';
const socket = openSocket('http://localhost:9000');

export default socket;
