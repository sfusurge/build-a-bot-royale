import React, { Component } from 'react';
import { Link } from "react-router-dom";

class ErrorPage extends Component {
  render() {
    return (
        <div>
          <h2 className='error-message'>That's an error. No page for <code>{this.props.location.pathname}</code></h2>
          <Link to="/">Back to home</Link>
        </div>
    );
  }
}

export default ErrorPage;
