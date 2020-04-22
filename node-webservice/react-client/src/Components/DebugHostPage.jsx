import React, { Component } from 'react';
import { Link } from "react-router-dom";
import {socket } from '../API/socketHandler';

// these should match the names of the gamestates used in the Unity game
const gameStates = [
  "titleScreen",
  "lobby",
  "build",
  "battle",
  "results"
];

const panelStyle = {
  backgroundColor: "white",
  border: "2px solid black",
  borderRadius: "5px",
  margin: "10px",
  padding: "10px"
}

const initialState = {
  gameID: "no-game",
  players: [],
  robots: {},
  gameState: "none",
  gameMessages: [],
  messageFormContent: JSON.stringify({ action: "customGameMessage", extraInfo: "hello world", bonusInfo: "heyayaya" }, null, 2)
};

class DebugHostPage extends Component {
  constructor(props) {
    super(props);
    this.state = initialState;
    this.numberOfWindowsOpened = 0; // this doesn't need to be in state because it doesn't affect rendering
  }

  componentDidMount() {
    this.startNewGame();

    socket.on("playerConnect", data => {
      this.setState({ players: [ ...this.state.players, data.username ]})
    });

    socket.on("playerDisconnect", data => {
      this.setState({ players: this.state.players.filter(name => name !== data.username) });
    });

    socket.on("gameStateChanged", data => {
      this.setState({ gameState: data.gameState });
    });

    socket.on("game-message", messageData => {
      const action = messageData.action;

      // add this game-message to the big list of game messages received
      this.setState({ gameMessages: [ ...this.state.gameMessages, messageData ]});

      // if the game-message is for submitting a robot, also add it to the robots list
      if (action === "submitrobot") {
        this.setState({
          robots: {...this.state.robots, ...({ [messageData.username]: messageData.parts }) }
        });
      }
    });
  }

  startNewGame() {
    this.setState(initialState);
    socket.emit("newgame", response => {
      var gameID = response.gameID;
      this.setState({ gameID: gameID, gameState: gameStates[0] });
    });
  }

  renderGameIDPanel() {
    const newPlayerURL = "http://" + window.location.host + "/game/" + this.state.gameID + "/user_name" + this.numberOfWindowsOpened;
    return (
      <div style={ panelStyle }>
        <p className="game-id-label">Game id: <b>{ this.state.gameID }</b></p>
        <button onClick={ this.startNewGame.bind(this) }>New game</button><br/>
        <a
          href={ newPlayerURL }
          onClick={ () => { this.numberOfWindowsOpened += 1 } }          
          target="_blank"
          rel="noopener noreferrer"
        >Open new player tab</a>
      </div>
    );
  }

  renderGameStatePanel() {
    return (
      <div style={ panelStyle }>
        <p>Current game state: <b>{ this.state.gameState }</b></p>
        <p>Change state to:</p>
        {
          gameStates.map(gameState =>
            <button
              key={ gameState }
              onClick={
                () => {
                  socket.emit("updateGameState", { gameState: gameState }, response => {
                    if (response.error) {
                      alert("Error updating game state: " + response.error);
                    }
                  });
                }
              }
            >{ gameState }</button>
          )
        }
      </div>
    );
  }

  renderPlayersPanel() {
    return (
      <div style={ panelStyle }>
        <p>Connected players</p>
        <ul>
          { this.state.players.map((username, index) => <li key={ index }>{ username }</li>) }
        </ul>
      </div>
    );
  }

  renderRobotsPanel() {
    return (
      <div style={ panelStyle }>
        <p>Received robots:</p>
        <ul>
          {
            Object.keys(this.state.robots).map((username, index) => {
              const robotData = this.state.robots[username];
              return <li key={ index }><b>{ username }:</b> <code>{ JSON.stringify(robotData) }</code></li>
            })
          }
        </ul>
      </div>
    );
  }

  renderGameMessagesPanel() {
    return (
      <div style={ panelStyle }>
        <p>All game-messages received:</p>
        <ul>
          { this.state.gameMessages.map((gameMessageData, index) =>
            <li key={ index }>
              <code>{ JSON.stringify(gameMessageData) }</code>
            </li>
          )}
        </ul>
      </div>
    );
  }

  renderSendGameMessagePanel() {
    const handleSubmit= (e) => {
      e.preventDefault();
      try {
        socket.emit("game-message", JSON.parse(this.state.messageFormContent), response => {
          if (response.error) {
            alert("Server returned error: " + response.error);
          }
        });
      } catch (e) {
        alert("Error sending message: " + e.message);
      }
    }

    return (
      <div style={ panelStyle }>
        <p>Send a game-message</p>
        <form onSubmit={ handleSubmit }>
          <textarea
            style={{ width: "350px", height: "100px" }}
            name="message-content"
            type="text"
            value={ this.state.messageFormContent }
            onChange={(e) => {
              this.setState({ messageFormContent: e.target.value });
            }}
          />
          <br/>
          <input type="submit"/>
        </form>
      </div>
    );
  }

  render() { 
    return ( 
      <div className="debug-host-page">
        <h1>Host a game, debug menu</h1>
        <h4>This page pretends to be the Unity client and can be used for testing</h4>
        <Link to="/">home</Link>
        { this.renderGameIDPanel() }
        { this.renderGameStatePanel() }
        { this.renderPlayersPanel() }
        { this.renderRobotsPanel() }
        { this.renderGameMessagesPanel() }
        { this.renderSendGameMessagePanel() }
      </div>
    );
  }
}
 
export default DebugHostPage;
