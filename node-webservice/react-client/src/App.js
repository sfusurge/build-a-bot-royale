import React, { Component } from 'react';
import background from './background.jpg';
import TextInput from './Components/TextInput'
import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link,
  useParams
} from "react-router-dom";
import socketIO from './API/socketHandler.js';
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
      connectedToAPI: false,
      currentGameID: null
    };
  }

  callAPI() {
      fetch("/api/test")
          .then(res => res.text())
          .then(res => this.setState({ apiResponse: res }));
  }

  componentDidMount() {
    setTimeout(() => {
      this.callAPI();
    }, 2000);

    socketIO.on('connect', () => {
      this.setState({ connectedToAPI: true });
    });
  }

  onGameIDSubmit(gameID) {
    socket.emit('joingame', gameID, (returnedID) => {
      this.setState({currentGameID: returnedID});
    });
  }

  render() {
    if (this.state.connectedToAPI) {
      if (this.state.currentGameID == null) {
        return (
          <Router>
            <div className="App">
              <header className="App-header">
                <img src='./logo.svg' style={{margin:'0.5vmin 3vmin', width:'calc(30px + 5vmin)'}}/>
                <h1 style={{textAlign:'center'}}>Build-a-Bot Royale</h1>
                <img src='./logo.svg' style={{margin:'0.5vmin 3vmin', width:'calc(30px + 5vmin)'}}/>
              </header>
              <div className="App-body" style={backgroundStyle}>
                <Switch>
                  <Route path="/" exact>
                    <TextInput onSubmit={this.onGameIDSubmit.bind(this)}/>
                  </Route>
                  <Route path="/game">
                    <p>Play game here!</p>
                  </Route>
                  <Route>
                    <p>404! No route matched!</p>
                  </Route>
                </Switch>
              </div>
            </div>
          </Router>
        );
      }else {
        return (
          <h1>Playing the game with id {this.state.currentGameID}!</h1>
        );
      }
    }else {
      return (
        <h1>Connecting...</h1>
      )
    }
  }
}

export default App;
