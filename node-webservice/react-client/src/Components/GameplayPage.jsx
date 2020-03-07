import React, { Component } from 'react';
import RobotJSONObjectForm from './RobotJSONObjectForm';
import ErrorPage from './ErrorPage';
import socket from '../API/socketHandler';
import Grid from './Grid';

class GameplayPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      joinedGameID: null,
      gameplayPhase: "buildrobot",
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
    socket.emit('joingame', socketMessageData, (err) => {
      if (err) {
        this.setState({ error: err });
      } else {
        this.setState({ joinedGameID: gameID, username: username });
      }
    });
  }

  renderGameplayUI() {
    // show different gameplay ui based on the gameplay phase
    if (this.state.gameplayPhase === 'buildrobot') {
      return (
        <div className='gameplay-page'>
          <h3>Playing game {this.props.match.params.gameid}</h3>
          <Grid onCellClick={this.handleCellClicked} parts={this.state.parts}></Grid>
        </div>
      );
      //return <RobotJSONObjectForm />;
    }
    if (this.state.gameplayPhase === 'controlrobot') {
      return (
        <div className='gameplay-page'>
          <h3>Playing game {this.props.match.params.gameid}</h3>
          <Grid onCellClick={this.handleCellClicked} parts={this.state.parts}></Grid>
        </div>
      );
    }
    return <h1 className="error-message">Invalid gameplay phase: {this.state.gameplayPhase}</h1>
  }

  handleCellClicked(x, y) {
    //makes sure cell has valid coordinates
    if (x < 0 || x > 4 || y < 0 || y > 4) {
      throw new Error("Invalid x or y");
    }

    if (this.state.gameplayPhase === "buildrobot") {
      // Checks to see if there is a part in the clicked location. If there is,
      // rotate the part and set 'partHere' to true.
      var partHere = false;
      var copy = [...this.state.parts];
      copy.forEach(element => {
        if (element.x === x && element.y === y) {
          element.direction = this.rotate(element.direction, x, y);
          partHere = true;
        }
      })
      this.setState({ parts: copy })


      if (!partHere) {
        //Code to place selected part from the toolbar
        var newPart = {
          "type": "block",
          "x": x,
          "y": y,
          "direction": "north",
          "health": 1.0
        }
        copy.push(newPart);
        this.setState({ parts: copy});
      }
    }


  }
  PartExistsIn(arr, target) {
    var found = 0;
    arr.forEach(element => {
      if (element[0] === target[0] && element[1] === target[1]) {
        found = 1;
      }
    })
    return found;
  }


  rotate(current, x, y) {
    const order = ["north", "west", "south", "east"]
    var partLocations = [];
    this.state.parts.forEach(element => {
      partLocations.push([element.x, element.y])
    })
    var intial = order.indexOf(current);
    if (intial === -1) {
      throw new Error("invalid direction");
    }
    for (var a = intial + 1; a < intial + 4; a++) {
      switch (order[a%4]) {
        case "north":
          if (this.PartExistsIn(partLocations,[x,y-1]) === 1) {
            return "north";
          }
          break;
        case "west":
          if (this.PartExistsIn(partLocations,[x+1,y]) === 1) {
            return "west";
          }
          break;
        case "south":
          if (this.PartExistsIn(partLocations,[x,y+1]) === 1) {
            return "south";
          }
          break;
        case "east":
          if (this.PartExistsIn(partLocations,[x-1,y]) === 1) {
            return "east";
          }
          break;
        default:
          throw new Error("invalid direction");
      }
    }
    return current;
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
}

export default GameplayPage;
