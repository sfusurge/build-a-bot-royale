import React, { Component } from 'react';
import socket from '../API/socketHandler';

class RobotBuildingWidget extends Component {
  constructor(props) {
    super(props);

    this.state = {robotString: JSON.stringify([
      {
          "type": "block",
          "x": 12,
          "y": 14,
          "direction": "north"
      },
      {
          "type": "spike",
          "x": 12,
          "y": 13,
          "direction": "north"
      },
      {
          "type": "center",
          "x": 13,
          "y": 13,
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
        (err) => {
          if (err) {
            alert("Error processing robot data: " + err);
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
          <br /> {/* replace this with css padding */}
          Build your robot:
          <br /> {/* replace this with css padding */}
          <textarea value={this.state.robotString} onChange={this.handleChange} />
        </label>
        <br /> {/* replace this with css padding */}
        <br /> {/* replace this with css padding */}
        <input type="submit" value="Submit" />
      </form>
    );
  }
}

export default RobotBuildingWidget;
