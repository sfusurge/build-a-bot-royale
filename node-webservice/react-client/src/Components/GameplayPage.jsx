import React, { Component } from 'react';
import RobotJSONObjectForm from './RobotJSONObjectForm';
import ErrorPage from './ErrorPage';
import TestGamePage from './TestGamePage';
import socket from '../API/socketHandler';
import Grid from './Grid';
class GameplayPage extends Component {
  
  constructor(props) {
   
    super(props);
    this.state = {
      currentType : "spike",
      joinedGameID: null,
      gameplayPhase: "not-set",
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
  }

  renderGameplayUI() {
    // show a page where the gameplayPhase can be changed when joining a test game
    if (this.state.gameplayPhase === "test-game") {
      return (
        <TestGamePage
          gameID={ this.state.joinedGameID }
          gameStates={ ["lobby", "build", "battle", "results"] }
          onChangeStateClicked={ gameState => this.setState({ gameplayPhase: gameState }) }
        />
      );
    }

    // show different gameplay ui based on the gameplay phase
    if (this.state.gameplayPhase === "initial" || this.state.gameplayPhase === "titleScreen" ||this.state.gameplayPhase === "lobby") {
      return (
        <div className='gameplay-page'>  
          <h1>Welcome { this.state.username }</h1>
          <h2>You are connected to game <code>{ this.state.joinedGameID }</code></h2>      
          <h3>Waiting for game to start...</h3>
          <h4>Game state is currently: { this.state.gameplayPhase }</h4>
        </div>
      );
    }
    if (this.state.gameplayPhase === 'build') {
      return (
        <div className='gameplay-page'>
          <h3>Playing game {this.props.match.params.gameid}</h3>
          <Grid onCellClick={this.handleCellClicked} parts={this.state.parts} 
          onChangeType={ (newType) => this.setState({ currentType: newType}) }></Grid>
        </div>
      );
      //return <RobotJSONObjectForm />;
    }
    if (this.state.gameplayPhase === 'battle') {
      return (
        <div className='gameplay-page'>
          <h3>Battle!</h3>
          <Grid onCellClick={ () => {} } parts={this.state.parts}></Grid>          
        </div>
      );
    }
    return <ErrorPage>No page defined for game state: {this.state.gameplayPhase}</ErrorPage> 
  }

  handleCellClicked(x, y) {
    
    //makes sure cell has valid coordinates
    if (x < 0 || x > 4 || y < 0 || y > 4) {
      throw new Error("Invalid x or y");
    }

    if (this.state.gameplayPhase === "build") {
      // Checks to see if there is a part in the clicked location. If there is,
      // rotate the part and set 'partHere' to true.
      var partHere = false;
      var copy = [...this.state.parts];
      copy.forEach((element,i) => {
      
        if (element.x === x && element.y === y) {
          if(this.state.currentType == "empty" && !(x==2 && y==2)){
            copy.splice(i, 1); 
          }
          else{
          element.direction = this.rotate(element.direction, x, y);
          
        }
        partHere = true;
        }
      })
      this.setState({ parts: copy })


      if (!partHere) {
          var newPart = {
            "type": this.state.currentType,
            "x": x,
            "y": y,
            "direction": "north",
            "health": 1.0
          }
          copy.push(newPart);
          newPart.direction = this.rotate(newPart.direction, x, y);
          this.setState({ parts: copy});

        //}
        
      }

      
    }


  }
  PartExistsIn(arr, target) {
    var found = 0;
    arr.forEach(element => {
      if (element[0] === target[0] && element[1] === target[1] ) {
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
