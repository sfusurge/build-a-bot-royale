import React from 'react';
import { render } from '@testing-library/react';
import GameplayPage from '../Components/GameplayPage';
import { BrowserRouter as Router } from 'react-router-dom';
import MockSocket from './MockSocketHandler';
import { overrideSocket } from '../API/socketHandler.js';

const useMockSocket = function() {
  const mockSocket = new MockSocket();
  overrideSocket(mockSocket);
  return mockSocket;
}

it ("shows joining game message before connecting to game", () => {
  const mockSocket = useMockSocket();

  const { queryByText } = render(
    <Router>
      <GameplayPage
        // the match and params properties are meant to be like how react-router provides params from the url
        match={
          {params:{
            username: 'user',
            gameID: "GAMID" 
          }}
        }
      />
    </Router>
  );

  expect(queryByText(/joining game/i)).toBeInTheDocument();
});

it ("emits joingame message on mount", () => {
  const mockSocket = useMockSocket();

  expect(mockSocket.EmitLog.length).toEqual(0);

  render(
    <Router>
      <GameplayPage
        match={
          {params:{
            username: 'testing_user',
            gameid: "TSTID" 
          }}
        }
      />
    </Router>
  );

  expect(mockSocket.EmitLog.length).toEqual(1);
  expect(mockSocket.EmitLog[0].messageName).toEqual('joingame');
});

it ("joingame message contains username and game id", () => {
  const mockSocket = useMockSocket();

  render(
    <Router>
      <GameplayPage
        match={
          {params:{
            username: 'testing_user',
            gameid: "TSTID" 
          }}
        }
      />
    </Router>
  );

  expect(mockSocket.EmitLog[0].messageData).toHaveProperty('username');
  expect(mockSocket.EmitLog[0].messageData).toHaveProperty('gameID');
  expect(mockSocket.EmitLog[0].messageData.gameID).toEqual('TSTID');
  expect(mockSocket.EmitLog[0].messageData.username).toEqual('testing_user');
});

it ("joingame message has ack function", () => {
  const mockSocket = useMockSocket();

  render(
    <Router>
      <GameplayPage
        match={
          {params:{
            username: 'testing_user',
            gameid: "TSTID" 
          }}
        }
      />
    </Router>
  );

  expect(mockSocket.EmitLog[0]).toHaveProperty('ack');
  expect(typeof mockSocket.EmitLog[0].ack).toEqual('function');
});

it ("doesnt show joining game message after joingame message is acked", () => {
  const mockSocket = useMockSocket();

  const { queryByText } = render(
    <Router>
      <GameplayPage
        match={
          {params:{
            username: 'testing_user',
            gameid: "TSTID" 
          }}
        }
      />
    </Router>
  );

  expect(queryByText(/joining game/i)).toBeInTheDocument();
  mockSocket.EmitLog[0].ack({ gameState: "lobby" });
  expect(queryByText(/joining game/i)).not.toBeInTheDocument();
});

it ("shows the error message if joingame message is acked with an error message", () => {
  const mockSocket = useMockSocket();

  const { queryByText } = render(
    <Router>
      <GameplayPage
        match={
          {params:{
            username: 'testing_user',
            gameid: "TSTID" 
          }}
        }
      />
    </Router>
  );

  mockSocket.EmitLog[0].ack({ error: "special error from test" });
  expect(queryByText(/joining game/i)).not.toBeInTheDocument();
  expect(queryByText(/special error from test/i)).toBeInTheDocument();
});
