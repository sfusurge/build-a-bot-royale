import React, { Component } from 'react';

class GridCell extends Component {
    colorForType(typeForPart) {
        if (typeForPart === "A") {
            return "red";
        }
        if (typeForPart === "B") {
            return "yellow";
        }
        return "grey";
    }

    style() {
        return {
            backgroundColor: this.colorForType(this.props.partType),
            height: "40px",
            width: "40px"
        };
    }

    render() {
        return (
            <button onClick={this.props.onClicked} style={ this.style() }>
                { this.props.partType }
            </button>
        );
    }
}

export default GridCell;
