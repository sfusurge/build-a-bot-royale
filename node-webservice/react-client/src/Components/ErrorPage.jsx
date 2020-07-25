import React from 'react';
import { Link } from "react-router-dom";

const ErrorPage = props => (
    <div className = 'gameplay-page'>
      <div className = 'state-message'>
        <h4>That's an error:</h4>
        <h2>{ props.children }</h2>
        <Link to="/" style={{color:"red"}}>Back to home</Link>
      </div>
    </div>
);

export default ErrorPage;
