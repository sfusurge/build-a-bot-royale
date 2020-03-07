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
      gameplayPhase: "buildrobot"
    }

    this.renderGameplayUI = this.renderGameplayUI.bind(this);
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
      console.log(err);
      if (err) {
        this.setState({ error: err});
      } else {
        this.setState({ joinedGameID: gameID, username: username });
      }
    });
  }

  renderGameplayUI() {
    // show different gameplay ui based on the gameplay phase
    if (this.state.gameplayPhase === 'buildrobot') {
      return <RobotJSONObjectForm/>;
    }
    if (this.state.gameplayPhase === 'controlrobot') {
      return <h1>TODO: put robot controls here</h1>;
    }
    return <h1 className="error-message">Invalid gameplay phase: {this.state.gameplayPhase}</h1>
  }

  handleCellClicked(x, y) {
    /*
    if (x < 0 || x > 4) {
      //error
    }
    if (this.state.selectedToolbarItem === 1) {

    }*/
    console.log("here: " + x + ", " + y);
  }

  render() {
    if (this.state.error) {
      return <ErrorPage>{this.state.error}</ErrorPage>
    }
    if (this.state.joinedGameID === null) {
      return <h1>Joining game { this.props.match.params.gameid }...</h1>;
    }

    return (
      <div className='gameplay-page'>
        <h3>Playing game { this.props.match.params.gameid }</h3>
        <Grid onCellClick={this.handleCellClicked} parts={ [ ] }></Grid>
        { this.renderGameplayUI() }
      </div>
    );
  }
}

export default GameplayPage;
