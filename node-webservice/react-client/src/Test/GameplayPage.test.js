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

// Build phase tests.
it ("Delete invalid doesn't delete any when there is only the center block",() => {
  var gameplayPage = new GameplayPage();
  var parts = [
    {
      "type": "center",
      "x": 2,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
  ]
  gameplayPage.deleteInvalid(parts);
  expect(parts.length).toEqual(1);
});

it ("deleteinvalid doesn't delete any when all blocks are attached",() => {
  var gameplayPage = new GameplayPage();
  var parts = [
    {
      "type": "center",
      "x": 2,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "block",
      "x": 3,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
  ]
  gameplayPage.deleteInvalid(parts);
  expect(parts.length).toEqual(2);
});

it ("deleteInvalid deletes parts that are not attached.",() => {
  var gameplayPage = new GameplayPage();
  var parts = [
    {
      "type": "center",
      "x": 2,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "block",
      "x": 4,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
  ]
  gameplayPage.deleteInvalid(parts);
  expect(parts.length).toEqual(1);
});

it ("deleteInvalid changes all parts to have valid directions",() => {
  var gameplayPage = new GameplayPage();
  var parts = [
    {
      "type": "center",
      "x": 2,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "block",
      "x": 2,
      "y": 1,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "spike",
      "x": 1,
      "y": 1,
      "direction": "north",
      "health": 1.0
    },
  ]
  gameplayPage.deleteInvalid(parts);
  var spikeDirection;
  parts.forEach(element =>{
    if(element.type === "spike"){
      spikeDirection = element.direction;
    }
  })
  expect(parts.length).toEqual(3);
  expect(spikeDirection).toEqual("west");
});

it("buildAttached properly identifies possible valid part locations", () => {
  var gameplayPage = new GameplayPage();
  var parts = [
    {
      "type": "center",
      "x": 2,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "block",
      "x": 2,
      "y": 1,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "spike",
      "x": 1,
      "y": 2,
      "direction": "west",
      "health": 1.0
    },
    {
      "type": "shield",
      "x": 3,
      "y": 2,
      "direction": "east",
      "health": 1.0
    },
  ]

  var allValidPartLocations = [];
  for(var a = 0; a < 5; a++){
    allValidPartLocations.push([false,false,false,false,false]);
  }
  gameplayPage.buildAttached(allValidPartLocations,parts);
  var countTrue = 0;
  for(a = 0; a < 5; a++){
    for(var b = 0; b < 5; b++){
      if(allValidPartLocations[a][b] === true){
        countTrue++;
      }
    }
  }
  expect(countTrue).toEqual(8);
  expect(allValidPartLocations[2][2]).toEqual(true);
  expect(allValidPartLocations[1][2]).toEqual(true);
  expect(allValidPartLocations[3][2]).toEqual(true);
  expect(allValidPartLocations[1][1]).toEqual(true);
  expect(allValidPartLocations[2][1]).toEqual(true);
  expect(allValidPartLocations[3][1]).toEqual(true);
  expect(allValidPartLocations[2][0]).toEqual(true);
  expect(allValidPartLocations[2][3]).toEqual(true);
});

it("closestValidDirection works properly",() =>{
  var gameplayPage = new GameplayPage();
  var parts = [
    {
      "type": "center",
      "x": 2,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "block",
      "x": 2,
      "y": 1,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "block",
      "x": 1,
      "y": 2,
      "direction": "north",
      "health": 1.0
    },
    {
      "type": "spike",
      "x": 1,
      "y": 1,
      "direction": "west",
      "health": 1.0
    }
  ]
  expect(gameplayPage.closestValidDirection(parts,1,1,"west",true)).toEqual("west");
  expect(gameplayPage.closestValidDirection(parts,1,1,"west",false)).toEqual("south");
})
