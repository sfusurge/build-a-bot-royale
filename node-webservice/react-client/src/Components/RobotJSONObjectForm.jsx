import React, { Component } from 'react';
import socket from '../API/socketHandler';

class RobotJSONObjectForm extends Component {
  constructor(props) {
    super(props);

    this.state = {robotString: JSON.stringify([
      {
          "type": "block",
          "x": 2,
          "y": 4,
          "direction": "north"
      },
      {
          "type": "spike",
          "x": 2,
          "y": 3,
          "direction": "north"
      },
      {
          "type": "center",
          "x": 3,
          "y": 3,
          "direction": "north"
      }
    ])};

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    this.setState({robotString: event.target.value});
  }

  handleSubmit(event) {
    event.preventDefault();

    try {
      const robotObject = JSON.parse(this.state.robotString);

      socket.emit(
        'game-message',
        { action: "submitrobot", parts: robotObject },
        response => {
          if (response.error) {
            alert("Error processing robot data: " + response.error);
          }
        }
      );
      alert("Sent robot data");
    } catch(e) {
      alert("Error sending robot data: " + e);
    }
  }

  render() {
    return (
      <form onSubmit={this.handleSubmit}>
        <label>
          Robot parts as JSON array:
          <textarea value={this.state.robotString} onChange={this.handleChange} />
        </label>
        <input type="submit" value="Submit" />
      </form>
    );
  }
}

export default RobotJSONObjectForm;
