import React, { Component } from 'react';

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
    try {
      var robotObject = JSON.parse(this.state.robotString);
      this.props.onSubmit(robotObject);
    } catch {
      alert("Invalid json");
    }
    event.preventDefault();
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
