import React, { Component } from 'react';
import "../CSS/ResultsPage.css"


class ResultsPage extends Component {
    
    render(){
        return (
            <div style={{color:"#282c34"}}>
                <section className="results-table">
                    <div className="results-table-outer">
                        <div className="results-table-inner">
                            <div className="results-cell" style={{border: "8px solid rgb(207, 14, 14)", backgroundColor: "rgb(241, 65, 65)"}}>
                                {this.renderDamage()}
                            </div>
                        </div>
                        <div className="results-table-inner">
                            <div className="results-cell" style={{border: "8px solid rgb(252, 186, 3)", backgroundColor: "rgb(253, 198, 48)"}}>
                            {this.renderPlace()}
                            </div>
                        </div>
                        <div className="results-table-inner">
                            <div className="results-cell" style={{border: "8px solid rgb(86, 140, 219)", backgroundColor: "rgb(103, 154, 230)"}}>
                                {this.renderKills()}
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        )
    }

    renderDamage(){
        var results = this.props.results;

        var playerIndex = -1;
        for(var a = 0; a < results["topDamage"].length; a++){
            if(results["topDamage"][a]["name"] === this.props.name){
                playerIndex = a;
                break;
            }
        }
        //Display the top 3 results (if there are enough people in the game), as well as the user's results.
        return(
        <div className="results-data-table">
            <h2> DAMAGE </h2>
            {results["topDamage"].length >= 1 && <p className="results-data">1st. {results["topDamage"][0]["name"]} : {results["topDamage"][0]["damage"].toFixed(1)} </p>}
            {results["topDamage"].length >= 2 && <p className="results-data">2nd. {results["topDamage"][1]["name"]} : {results["topDamage"][1]["damage"].toFixed(1)} </p>}
            {results["topDamage"].length >= 3 && <p className="results-data">3rd. {results["topDamage"][2]["name"]} : {results["topDamage"][2]["damage"].toFixed(1)} </p>}
            <h6>--------------------------</h6>
            {playerIndex !== -1 && <p className="results-data"  style={{"font-weight" : "bold"}}>{(a+1).toString() + this.getOrdinal(a+1)}. {results["topDamage"][a]["name"]} : {results["topDamage"][a]["damage"].toFixed(1)} </p>}
        </div>
        );
    }

    renderPlace(){
        var results = this.props.results;

        var playerIndex = -1;
        for(var a = 0; a < results["topPlacements"].length; a++){
            if(results["topPlacements"][a]["name"] === this.props.name){
                playerIndex = a;
                break;
            }
        }
        //Display the top 3 results (if there are enough people in the game), as well as the user's results.
        return(
        <div className="results-data-table">
            <h2> PLACE </h2>
            {results["topPlacements"].length >= 1 && <p className="results-data">1st. {results["topPlacements"][0]["name"]}</p>}
            {results["topPlacements"].length >= 2 && <p className="results-data">2nd. {results["topPlacements"][1]["name"]}</p>}
            {results["topPlacements"].length >= 3 && <p className="results-data">3rd. {results["topPlacements"][2]["name"]}</p>}
            <h6>--------------------------</h6>
            {playerIndex !== -1 && <p className="results-data" style={{"font-weight" : "bold"}}>{(a+1).toString() + this.getOrdinal(a+1)}. {results["topPlacements"][a]["name"]} </p>}
        </div>
        );
    }

    renderKills(){
        var results = this.props.results;

        var playerIndex = -1;
        for(var a = 0; a < results["topKills"].length; a++){
            if(results["topKills"][a]["name"] === this.props.name){
                playerIndex = a;
                break;
            }
        }
        //Display the top 3 results (if there are enough people in the game), as well as the user's results.
        return(
        <div className="results-data-table">
            <h2> KILLS </h2>
            {results["topKills"].length >= 1 && <p className="results-data">1st. {results["topKills"][0]["name"]} : {results["topKills"][0]["kills"]}</p>}
            {results["topKills"].length >= 2 && <p className="results-data">2nd. {results["topKills"][1]["name"]} : {results["topKills"][1]["kills"]}</p>}
            {results["topKills"].length >= 3 && <p className="results-data">3rd. {results["topKills"][2]["name"]} : {results["topKills"][2]["kills"]}</p>}
            <h6>--------------------------</h6>
            {playerIndex !== -1 && <p className="results-data" style={{"font-weight" : "bold"}}>{(a+1).toString() + this.getOrdinal(a+1)}. {results["topKills"][a]["name"]}  : {results["topKills"][a]["kills"]}</p>}
        </div>
        );
    }

    getOrdinal(num){
        if(num === 11 || num === 12 || num === 13){
            return "th";
        }else{
            if(num%10 === 1){
                return "st";
            }else if(num%10 === 2){
                return "nd";
            }else if(num%10 === 3){
                return "rd";
            }else{
                return "th";
            }
        }
    }
}
export default ResultsPage;