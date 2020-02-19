import React, { Component } from 'react';
import ConnectionMessage from './Components/ConnectionMessage';
import JoinGameForm from './Components/JoinGameForm';
import GameplayPage from './Components/GameplayPage';
import ErrorPage from './Components/ErrorPage';

import background from './background.jpg';
import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";
import socket from './API/socketHandler.js';

const backgroundStyle = {
  backgroundImage:`url(${background})`,
  flex: 1,
  resizeMode: 'cover', 
  backgroundPosition:"center",
}

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      connectedToAPI: false
    };

    this.renderAppBodyContent = this.renderAppBodyContent.bind(this);
  }

  componentDidMount() {
    socket.on('connect', () => {
      this.setState({ connectedToAPI: true });
    });

    socket.on('disconnect', () => {
      this.setState({ connectedToAPI: false });
    });
  }

  renderAppBodyContent() {
    // show connecting message before the socket api responds
    if (this.state.connectedToAPI !== true) {
      return <ConnectionMessage/>;
    }

    // show a different component based on the URL path
    return (
      <Router>
        <Switch>
          <Route path="/" exact component={ JoinGameForm } />
          <Route path="/game/:gameid" exact component={ GameplayPage } />
          <Route component={ () => <ErrorPage>Page not found</ErrorPage> } />
        </Switch>
      </Router>
    );
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src='/logo.svg' alt="app logo" style={{margin:'0.5vmin 3vmin', width:'calc(30px + 5vmin)'}}/>
          <h1 style={{textAlign:'center'}}>Build-a-Bot Royale</h1>
          <img src='/logo.svg' alt="app logo" style={{margin:'0.5vmin 3vmin', width:'calc(30px + 5vmin)'}}/>
        </header>
        <div className="App-body" style={backgroundStyle}>
          { this.renderAppBodyContent() }
        </div>
      </div>
    );
  }
}

export default App;