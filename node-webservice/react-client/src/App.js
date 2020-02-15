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

const backgroundStyle = {
  backgroundImage:`url(${background})`,
  flex: 1,
  resizeMode: 'cover', 
  backgroundPosition:"center",
}

class App extends Component {
  constructor(props) {
    super(props);
    this.state = { apiResponse: "waiting for api..." };
  }

  callAPI() {
      fetch("/api/test")
          .then(res => res.text())
          .then(res => this.setState({ apiResponse: res }));
  }

  componentWillMount() {
    setTimeout(() => {
      this.callAPI();
    }, 2000);
  }

  render() {
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
                <TextInput/>
                <p>API status: { this.state.apiResponse }</p>
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
  }
}

export default App;
