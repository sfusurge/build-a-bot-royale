import React, { Component } from 'react';
import GridCell from "./GridCell";
import "../App.css"
class Grid extends Component {
     currentType = "block";
    partsArrayTo2DArray(partsData) {
        // build empty 5x5 array
        var partsArray = [];
        for (var rowIndex = 0; rowIndex < 5; rowIndex++) {
            var row = [];
            for (var colIndex = 0; colIndex < 5; colIndex++) {
                row.push({ x: colIndex, y: rowIndex });
            }
            partsArray.push(row);
        }

        // put defined parts into the array
        partsData.forEach(element => {
            partsArray[element.y][element.x] = element;
        });

        return partsArray;
    }
    
    

    renderRow(data, index) {
        return <div key={index} style={{ display: "flex", flexDirection: "row" }}>
            { data.map(this.renderCell.bind(this)) }
        </div>
    }

    renderCell(data, index) {
        return <GridCell 
            key={ index }
            partType={ data.type }
            partDirection = {data.direction}
            partHealth = {data.health}
            onClicked={() => this.props.onCellClick(data.x, data.y, this.currentType)}
            gameplayPhase = {this.props.gameplayPhase}
        />
    }

    render() {
        const partsArray = this.partsArrayTo2DArray(this.props.parts);
        var properClass;
        if(this.props.gameplayPhase === "build"){
            properClass = "grid-style-build";
        }else{
            properClass = "grid-style-battle";
        }

        return (  
            <div className = {properClass} style = {{margin:"2vmin"}}> 
                { partsArray.map(this.renderRow.bind(this)) }
            </div>

        );
    }
}

export default Grid;
