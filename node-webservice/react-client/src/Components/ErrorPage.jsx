import React, { Component } from 'react';
import { Link } from "react-router-dom";

class ErrorPage extends Component {
  render() {
    return (
        <div>
          <h4>That's an error:</h4>
          <h2>{ this.props.children }</h2>
          <Link to="/">Back to home</Link>
        </div>
    );
  }
}

export default ErrorPage;
