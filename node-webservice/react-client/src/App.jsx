import React, { Component } from 'react';
import ConnectionMessage from './Components/ConnectionMessage';

import HomePage from './Components/HomePage';
import GameplayPage from './Components/GameplayPage';
import ErrorPage from './Components/ErrorPage';
import AboutPage from './Components/AboutPage';
import HostGamePage from './Components/HostGamePage';
import DebugHostPage from './Components/DebugHostPage';

import background from './background.jpg';
import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";
import { socket } from './API/socketHandler.js';

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
          <Route path="/" exact component={ HomePage } />
          <Route path="/game/:gameid/:username" exact component={ GameplayPage } />
          <Route path="/about" exact component={ AboutPage } />
          <Route path="/host" exact component={ HostGamePage } />
          <Route path="/debughost" exact component={ DebugHostPage } />
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
