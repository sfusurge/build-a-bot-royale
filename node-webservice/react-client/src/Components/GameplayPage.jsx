import React, { Component } from 'react';
import ErrorPage from './ErrorPage';
import TestGamePage from './TestGamePage';
import { socket } from '../API/socketHandler';
import Grid from './Grid';

import TypeToolbar from './TypeToolbar';
import BehaviourBar from './BehaviourBar'
import "../App.css"



class GameplayPage extends Component {

  constructor(props) {

    super(props);
    this.state = {
      currentType: "block",
      joinedGameID: null,
      gameplayPhase: "not-set",
      behaviour: "not-set",
      parts: [
        {
          "type": "block",
          "x": 2,
          "y": 3,
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
          "type": "spike",
          "x": 2,
          "y": 4,
          "direction": "north",
          "health": 1.0
        },
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
          "x": 4,
          "y": 2,
          "direction": "east",
          "health": 1.0
        },
        {
          "type": "spike",
          "x": 2,
          "y": 0,
          "direction": "south",
          "health": 1.0
        },
        {
          "type": "block",
          "x": 3,
          "y": 2,
          "direction": "north",
          "health": 1.0
        }
      ]
    }

    this.renderGameplayUI = this.renderGameplayUI.bind(this);
    this.handleCellClicked = this.handleCellClicked.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.changeBehaviour = this.changeBehaviour.bind(this);
  }

  componentDidMount() {
    // get the game id and username from the url params
    const gameID = this.props.match.params.gameid;
    const username = this.props.match.params.username;

    const socketMessageData = {
      gameID: gameID,
      username: username
    }

    // join the game by sending the 'joingame' message to the socket API
    socket.emit('joingame', socketMessageData, response => {
      if (response.error) {
        this.setState({ error: response.error });
      } else {
        this.setState({
          joinedGameID: gameID,
          username: username,
          gameplayPhase: response.gameState
        });
      }
    });

    // update the component's state when the game changes states
    socket.on("gameStateChanged", messageData => {
      const newState = messageData.gameState;
      this.setState({ gameplayPhase: newState });
    });

    socket.on("game-message", messageData => {
      if (messageData.action === "currentParts" && messageData.name === this.state.username) {
        this.setState({ parts: messageData.parts });
      }
    })
  }

  renderGameplayUI() {
    // show a page where the gameplayPhase can be changed when joining a test game
    if (this.state.gameplayPhase === "test-game") {
      return (
        <TestGamePage
          gameID={this.state.joinedGameID}
          gameStates={["lobby", "build", "battle", "results"]}
          onChangeStateClicked={gameState => this.setState({ gameplayPhase: gameState })}
        />
      );
    }

    // show different gameplay ui based on the gameplay phase
    if (this.state.gameplayPhase === "initial" || this.state.gameplayPhase === "titleScreen" || this.state.gameplayPhase === "lobby") {
      return (
        <div className='gameplay-page'>
          <h1>Welcome {this.state.username}</h1>
          <h2>You are connected to game <code>{this.state.joinedGameID}</code></h2>
          <h3>Waiting for game to start...</h3>
          <h4>Game state is currently: {this.state.gameplayPhase}</h4>
        </div>
      );
    }
    if (this.state.gameplayPhase === 'build') {
      return (
        <div className='gameplay-page'>
          <h3>Playing game {this.props.match.params.gameid}</h3>
          <div className="build_surrounding_square">
            <Grid parts={this.state.parts} onCellClick={this.handleCellClicked} gameplayPhase={this.state.gameplayPhase}></Grid>
            <TypeToolbar onChangeType={(newType) => this.setState({ currentType: newType })}></TypeToolbar>
          </div>
          <button onClick={this.handleSubmit}> Submit </button>

        </div>
      );
      //return <RobotJSONObjectForm />;
    }
    if (this.state.gameplayPhase === 'battle') {

      return (
        <div className='surrounding-square'>
          <Grid onCellClick={() => { }} parts={this.state.parts} gameplayPhase={this.state.gameplayPhase}></Grid>
          <BehaviourBar clicked={this.changeBehaviour} />
          {this.renderBehaviourText()}
        </div>
      );
    }
    return <ErrorPage>No page defined for game state: {this.state.gameplayPhase}</ErrorPage>
  }

  handleSubmit(event) {
    event.preventDefault();

    try {
      socket.emit(
        'game-message',
        { action: "submitrobot", parts: this.state.parts, username: this.state.username },
        response => {
          if (response.error) {
            alert("Error processing robot data: " + response.error);
          }
        }
      );
      //alert("Sent robot data");
      this.setState({ gameplayPhase: "battle" })
    } catch (e) {
      alert("Error sending robot data: " + e);
    }
  }

  changeBehaviour(behaviour) {
    const behaviourMessage = {
      action: "changeBehaviour",
      behaviour: behaviour.toLowerCase()
    }
    this.setState({ behaviour: behaviour });
    socket.emit(
      'game-message', behaviourMessage, response => {
        if (response.error) {
          alert("Error changing robot behaviour: " + response.error);
        }
      });
  }

  handleCellClicked(x, y) {
    //makes sure cell has valid coordinates
    if (x < 0 || x > 4 || y < 0 || y > 4) {
      throw new Error("Invalid x or y");
    }

    if (this.state.gameplayPhase === "build") {
      var partHere = false;
      var copy = [...this.state.parts];
      // Checks to see if there is a part in the clicked location. If there is,
      // rotate the part and set 'partHere' to true.
      copy.forEach((element, i) => {
        if (element.x === x && element.y === y) {
          if (this.state.currentType === "empty" && !(x === 2 && y === 2)) {
            // After deleting the element in the current position, need to delete all parts
            // that are now invalid. Delete invalid modifies copy. 
            copy.splice(i, 1);
            this.deleteInvalid(copy);
          }
          else {
            element.direction = this.closestValidDirection(copy, x, y, element.direction, false);
          }
          partHere = true;
        }
      })
      this.setState({ parts: copy })

      // If the cell is empty and a part is selected
      if (!partHere && this.state.currentType !== "empty") {
        
        // 'allValidPartLocations' is a 5x5 bool grid, where an entry is true if it is a valid place for a part. 
        // (next to block or center)
        var allValidPartLocations = [];
        for (var a = 0; a < 5; a++) {
          allValidPartLocations.push([false, false, false, false, false]);
        }
        
        // Fills allValidPartLocations to match state.parts 
        this.buildAttached(allValidPartLocations, this.state.parts)

        // If the clicked cell is a valid location, then put a new part there. 
        if (allValidPartLocations[x][y]) {
          var newPart = {
            "type": this.state.currentType,
            "x": x,
            "y": y,
            "direction": "north",
            "health": 1.0
          }
          copy.push(newPart);

          // Sets the direction to a valid direction.
          newPart.direction = this.closestValidDirection(copy, x, y, newPart.direction, true);
          this.setState({ parts: copy });
        }
      }
    }
  }

  deleteInvalid(partsCopy) {
    //partsCopy is a copy of this.state.parts with one part deleted (during building gamephase)

    // 'allValidPartLocations' is a 5x5 bool grid, where an entry is true if it is a valid place for a part. 
    // (next to block or center)
    var allValidPartLocations = [];
    var a;
    for (a = 0; a < 5; a++) {
      allValidPartLocations.push([false, false, false, false, false]);
    }

    this.buildAttached(allValidPartLocations, partsCopy);

    // If there is a part at an invalid location, remove it.
    var i = 0;
    while (i < partsCopy.length) {
      if (allValidPartLocations[partsCopy[i].x][partsCopy[i].y] === false) {
        partsCopy.splice(i, 1);
      } else {
        i++;
      }
    }

    // Makes sure that all remaining spikes and shields still have a valid direction. If not, rotate the block.
    // It is guarunteed to have at least one valid direction because all parts not next to a block were already deleted.
    partsCopy.forEach(element => {
      if (element.type === "spike" || element.type === "shield") {
        element.direction = this.closestValidDirection(partsCopy, element.x, element.y, element.direction, true);
      }
    })

  }

  // returns the current direction if it is valid, and if not, returns the next valid direction.
  closestValidDirection(partsArr, x, y, direction, includeStarting) {
    // 'blocks': a 5x5 bool array where entry is true if there is a block or center in that location.
    var blocks = []
    for(var a = 0; a < 5; a++) {
      blocks.push([false, false, false, false, false]);
    }
    partsArr.forEach(element => {
      if (element.type === "block" || element.type === "center") {
        blocks[element.x][element.y] = true;
      }
    })

    const order = ["north", "west", "south", "east"]
    var intial = order.indexOf(direction);
    if (includeStarting === false) {
      intial += 1;
    }

    // for every direction, starting with the current direction, return that direction if it is valid.
    // (if there is a block in the corresponding location.)
    for (a = intial; a < intial + 4; a++) {
      switch (order[a % 4]) {
        case "north":
          if (y > 0 && blocks[x][y - 1]) {
            return "north";
          }
          break
        case "east":
          if (x > 0 && blocks[x - 1][y]) {
            return "east";
          }
          break
        case "west":
          if (x < 4 && blocks[x + 1][y]) {
            return "west";
          }
          break
        case "south":
          if (y < 4 && blocks[x][y + 1]) {
            return "south";
          }
          break
        default:
          throw new Error("invalid direction");
      }
    }
  }

  buildAttached(allValidPartLocations, partsArr) {
    // 'currentparts' is an 5x5 string grid, where each entry is the type of the part in that position.
    // If there is no part in that position, then the entry is "none".
    var currentParts = [];
    for (var a = 0; a < 5; a++) {
      currentParts.push(["none", "none", "none", "none", "none"]);
    }

    partsArr.forEach(element => {
      currentParts[element.x][element.y] = element.type;
    })

    this.recursiveAttached(2, 2, allValidPartLocations, currentParts);
  }

  recursiveAttached(x, y, allValidParts, currentParts) {
    //checks to make sure x and y are valid.
    if (x >= 0 && x < 5 && y >= 0 && y < 5) {
      //if position hasn't already been marked as valid, then mark it as valid.
      if (allValidParts[x][y] === false) {
        allValidParts[x][y] = true;
        //if there is a block or center in that position, then recursively call the method for the 4 neighbouring cells
        // in the grid.
        if (currentParts[x][y] === "block" || currentParts[x][y] === "center") {
          this.recursiveAttached(x - 1, y, allValidParts, currentParts);
          this.recursiveAttached(x + 1, y, allValidParts, currentParts);
          this.recursiveAttached(x, y - 1, allValidParts, currentParts);
          this.recursiveAttached(x, y + 1, allValidParts, currentParts);
        }
      }
    }
  }

  render() {
    if (this.state.error) {
      return <ErrorPage>{this.state.error}</ErrorPage>
    }
    if (this.state.joinedGameID === null) {
      return <h1>Joining game {this.props.match.params.gameid}...</h1>;
    }
    return this.renderGameplayUI();
  }

  renderBehaviourText() {
    if (this.state.behaviour === "not-set") {
      return <h3>Choose A Behaviour...</h3>;
    } else {
      return <h3>Behaviour: {this.state.behaviour}</h3>;
    }
  }
}

export default GameplayPage;
