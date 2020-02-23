import React, { Component } from 'react';
import TextInput from './TextInput';

class JoinGameForm extends Component {
  OnGameIDSubmit(gameID) {
    // use React router history to change the url to the game page, making React router render a different page
    this.props.history.push('/game/' + gameID)
  }

  render() {
    return (
      <div className="join-game-form">
        <TextInput onSubmit={ this.OnGameIDSubmit.bind(this) }/>
      </div>
    );
  }
}

export default JoinGameForm;
