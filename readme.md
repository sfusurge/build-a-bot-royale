# Build-A-Bot Royale

![Publish Unity game to Itch.io 🎮](https://github.com/sfusurge/build-a-bot-royale/workflows/Publish%20Unity%20game%20to%20Itch.io%20%F0%9F%8E%AE/badge.svg)

Game about building robots and battling them. 
Players build their robot on their device, then everyone watches the battle on a larger screen.

## Overview
The project consists of three parts:
* Node.js + express web service: handles connection between clients and websocket connection
* React client: The website that players use to build their robot
* Unity game: Where the robot battle happen.

## Dev setup
Each of the three parts of the project require their own development setup. There are readme.md files in three top level folders with more instructions on how to get set up.

### Node.js setup
* Clone the repo
* Download Node.js
* Navigate to /node-webservice
* Run `npm install` to install dependencies
* Run `npm run start` to start the webservice (alternatively, run `npm run dev` to start the webservice and auto-restart when code changes)
* The Node webservice is now being served from `localhost:9000`, open this url in your browser

### React setup
* Clone the repo
* Download Node.js
* Navigate to /node-webservice/react-client
* Run `npm install` to install dependencies
* Run `npm run start`
* The react app is now being served from `localhost:3000`, open this url in your browser.
* For the React app to access the Node.js API, you will need to have it running at the same time.

For doing web dev work, it's best to have both the Node.js and React services running, you'll need to run two terminals for this.

### Unity setup
* Clone the repo
* Download Unity Hub
* Download Unity 2019
* Open the folder /BotRoyaleUnity in Unity
* Open the scene /Assets/Scenes/BattleScene.unity
* Press play in the editor
