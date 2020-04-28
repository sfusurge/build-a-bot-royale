import React, { Component } from 'react';
import "../CSS/ResultsPage.css"


class ResultsPage extends Component {
    
    render(){
        return (
            <div style={{color:"#282c34"}}>
                <h1>RESULTS</h1>
                <section className="results-table">
                    <div className="results-table-outer">
                        <div className="results-table-inner">
                            <div className="results-cell" style={{border: "8px solid rgb(207, 14, 14)", backgroundColor: "rgb(241, 65, 65)"}}>
                                <h2> DAMAGE </h2>
                                {this.renderDamage()}
                            </div>
                        </div>
                        <div className="results-table-inner">
                            <div className="results-cell" style={{border: "8px solid rgb(252, 186, 3)", backgroundColor: "rgb(253, 198, 48)"}}>
                                <h2> PLACE </h2>
                            </div>
                        </div>
                        <div className="results-table-inner">
                            <div className="results-cell" style={{border: "8px solid rgb(86, 140, 219)", backgroundColor: "rgb(103, 154, 230)"}}>
                                <h2> KILLS </h2>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        )
    }

    renderDamage(){
        var results = this.props.results;
        return(
        <div>
            <p>{JSON.stringify(results)}</p>
            <p>{this.props.name}</p>
        </div>
        );
    }
}
export default ResultsPage;