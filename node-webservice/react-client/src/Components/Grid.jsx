import React, { Component } from 'react';
import GridCell from "./GridCell";
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

    style(type1) {
        return { 
            backgroundImage: `url(${type1})`,
            backgroundSize: (this.props.gameplayPhase === "build") ? 'calc(14vmin - 2px) calc(14vmin - 2px)' : 'calc(12vmin - 2px) calc(12vmin - 2px)',
            height: (this.props.gameplayPhase === "build") ? "14vmin" : "12vmin",
            width: (this.props.gameplayPhase === "build") ? "14vmin" : "12vmin",
            border: "1px solid black",
            outline:"none",
        };
    }

    render() {
        const partsArray = this.partsArrayTo2DArray(this.props.parts);
        const gridStyleBuild = {
            height: (this.props.gameplayPhase === "build") ? "70vmin" : "60vmin",
            width: (this.props.gameplayPhase === "build") ? "70vmin" : "60vmin",
            margin: "2vmin"
        }

        return (   
                //  <div className="Grid-Container" style = {gridStyle}> </div>
                <div className="Grid-Container" style = {gridStyleBuild}> 
                    { partsArray.map(this.renderRow.bind(this)) }
                </div>

        );
    }
}

export default Grid;
