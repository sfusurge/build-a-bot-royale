import React, { Component } from 'react';
import RobotJSONObjectForm from './RobotJSONObjectForm';
import RobotBuildingWidget from './RobotBuildingWidget';
import ErrorPage from './ErrorPage';
import socket from '../API/socketHandler';

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
    // get the game id from the url params
    const gameID = this.props.match.params.gameid;

    // join the game by sending the 'joingame' message to the socket API
    socket.emit('joingame', gameID, (err) => {
      if (err) {
        this.setState({ error: err});
      } else {
        this.setState({ joinedGameID: gameID });
      }
    });
  }

  renderGameplayUI() {
    // show different gameplay ui based on the gameplay phase
    if (this.state.gameplayPhase === 'buildrobot') {
      return <label><RobotJSONObjectForm/><RobotBuildingWidget/></label>;
      //return <RobotBuildingWidget/>; //eventually remove RobotJSONObjectForm altogether
    }
    if (this.state.gameplayPhase === 'controlrobot') {
      return <h1>TODO: put robot controls here</h1>;
    }
    return <h1 className="error-message">Invalid gameplay phase: {this.state.gameplayPhase}</h1>
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
        { this.renderGameplayUI() }
      </div>
    );
  }
}

export default GameplayPage;
