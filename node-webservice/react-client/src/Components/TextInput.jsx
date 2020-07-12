import React, { Component } from 'react';

const inputStyle = {
    width: 'calc((60px + 14vmin))',
    padding:'5px', 
    fontSize: '1.2em', 
    textAlign:'center',
    margin:'1vh',
}

class TextInput extends Component {
    constructor(props) {
        super(props);
        this.state = { nickname: "", gameID: "" }

        this.handleSubmit = this.handleSubmit.bind(this);
        this.isInputValid = this.isInputValid.bind(this);
    }

    componentDidMount() {
        // auto-fill the gameID if specified in url query params
        const params = new URLSearchParams(window.location.search);
        const paramGameID = params.get("gameID")
        if (paramGameID) {
            this.setState({ gameID: paramGameID });
        }
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
                <div className="field-wrapper">
                    <label htmlFor="nickname-field">Nickname</label>
                    <input
                        type="text"
                        id="nickname-field"
                        style={ inputStyle }
                        value={ this.state.nickname }
                        className="Text-input"
                        autoComplete="off"
                        onChange={ e => this.setState({ nickname: e.target.value })}
                    /> 
                </div>
                <div className="field-wrapper">                
                    <label htmlFor="gameid-field">Game Code</label>                           
                    <input
                        type="text"
                        id="gameid-field"
                        style={ inputStyle }
                        value={ this.state.gameID }
                        className="Text-input"
                        autoComplete="off"
                        onChange={ e => this.setState({ gameID: e.target.value.toUpperCase() })}
                    />
                </div>
                <input type="submit" className="submit-button" style={{margin:'2vh 0 0 0'}} value='Enter' disabled={ !this.isInputValid() }/>
            </form>
         );
    }
}
 
export default TextInput;
