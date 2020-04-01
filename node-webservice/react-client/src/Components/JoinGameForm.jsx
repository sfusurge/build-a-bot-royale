import React, { Component } from 'react';
import TextInput from './TextInput';

class JoinGameForm extends Component {
  OnGameIDSubmit(formInfo) {
    // use React router history to change the url to the game page, making React router render a different page
    this.props.history.push('/game/' + formInfo.gameID + "/" + formInfo.username);
  }

  render() {
    return (
      <div className="join-game-form" style={{margin: '0 auto'}}>
        <TextInput onSubmit={ this.OnGameIDSubmit.bind(this) }/>
      </div>
    );
  }
}

export default JoinGameForm;
