import React, { Component } from 'react';

const inputStyle = {
    width: 'calc((60px + 14vmin))',
    padding:'0.5vw', 
    fontSize: 'calc(10px + 2vmin)', 
    textAlign:'center',
    margin:'1vh'
}

const submitStyle = {
    width:'calc((10px + 2vmin)*5)',
    fontSize: 'calc(10px + 2vmin)',
    textAlign:'center',
}



class TextInput extends Component {
    constructor(props) {
        super(props);
        this.state = { nickname: "", gameID: "" }

        this.handleSubmit = this.handleSubmit.bind(this);
        this.isInputValid = this.isInputValid.bind(this);
    }

    handleSubmit(event){
        this.props.onSubmit( {
            gameID: this.state.gameID,
            username: this.state.nickname
        });
        
        event.preventDefault();
    }

    isInputValid() {
        return this.state.gameID.length === 5 && this.state.nickname.length > 0;
    }

    render() { 
        return ( 
            <form onSubmit={this.handleSubmit} style={{display:'flex', flexDirection:'column', alignItems:'center'}}>
                <input
                    type="text"
                    placeholder="Nickname"
                    style={ inputStyle }
                    value={ this.state.nickname }
                    onChange={ e => this.setState({ nickname: e.target.value })}
                />            
                <input
                    type="text"
                    placeholder="Game Code"
                    style={ inputStyle }
                    value={ this.state.gameID }
                    onChange={ e => this.setState({ gameID: e.target.value })}
                />
                <input type="submit" value='Enter' style={submitStyle} disabled={ !this.isInputValid() }/>
            </form>
         );
    }
}
 
export default TextInput;
