class MockSocket {
    constructor() {
        this.clientEvents = {};

        this.on = this.on.bind(this);
    }

    // Noraml Socket client API
    on(eventName, func) {
        if (!this.clientEvents[eventName]) {
            this.clientEvents[eventName] = [];
        }
        this.clientEvents[eventName].push(func);
    }

    emit(eventName, eventData) {
    }

    // Extra methods for tests to call
    serverEmit(eventName, eventData) {
        if (this.clientEvents[eventName]) {
            this.clientEvents[eventName].forEach(eventFunction => {
                eventFunction(eventData);
            });
        }
    }
}

export default MockSocket;
