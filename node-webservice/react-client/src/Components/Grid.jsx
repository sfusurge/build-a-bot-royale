import React, { Component } from 'react';
import GridCell from "./GridCell";

class Grid extends Component {

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
            onClicked={() => this.props.onCellClick(data.x, data.y)}
        />
    }

    render() {
        const partsArray = this.partsArrayTo2DArray(this.props.parts);
        const gridStyleBuild = {
            height: "60vmin",
            width: "60vmin",
            margin: "2vmin auto"

        }
        return (
            <div className="Grid-Container" style = {gridStyleBuild}>
                { partsArray.map(this.renderRow.bind(this)) }
            </div>
        );
    }
}

export default Grid;
