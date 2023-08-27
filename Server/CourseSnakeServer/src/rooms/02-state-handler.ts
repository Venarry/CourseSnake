import { Room, Client, updateLobby } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class MyVector3 extends Schema
{
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;

    constructor (x: number, y: number, z: number)
    {
        super();

        this.x = x;
        this.y = y;
        this.z = z;
    }

    SetValues(newValues: MyVector3)
    {
        this.x = newValues.x;
        this.y = newValues.y;
        this.z = newValues.z;
    }
}

export class Player extends Schema {
    @type(MyVector3)
    Position = new MyVector3(0, 0, 0);

    @type(MyVector3)
    Rotation = new MyVector3(0, 0, 0);

    @type(MyVector3)
    Direction = new MyVector3(0, 0, 0);

    @type("boolean")
    BoostState = false;

    @type("number")
    Score = 0;

    SetPosition(position: MyVector3)
    {
        this.Position.SetValues(position);
    }

    SetRotation(rotation: MyVector3)
    {
        this.Rotation.SetValues(rotation);
    }

    SetDirection(position: MyVector3)
    {
        this.Direction.SetValues(position);
    }

    SetBoostState(state: boolean)
    {
        this.BoostState = state;
    }

    SetScore(score: number)
    {
        this.Score = score;
    }

    constructor (position: MyVector3)
    {
        super();

        this.Position.SetValues(position);
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    CreatePlayer(sessionId: string, position: MyVector3) {
        this.players.set(sessionId, new Player(position));
    }

    RemovePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    SetPlayerPosition(sessionId: string, position: MyVector3) 
    {
        this.players.get(sessionId).SetPosition(position);
    }

    SetPlayerRotation(sessionId: string, rotation: MyVector3) 
    {
        this.players.get(sessionId).SetRotation(rotation);
    }

    SetPlayerDirection(sessionId: string, direction: MyVector3) 
    {
        this.players.get(sessionId).SetDirection(direction);
    }

    SetBoostState(sessionId: string, state: boolean)
    {
        this.players.get(sessionId).SetBoostState(state);
    }

    SetPlayerScore(sessionId: string, score: number) 
    {
        this.players.get(sessionId).SetScore(score);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());
        this.setMetadata(options).then(() => updateLobby(this));

        this.onMessage("PlayerSpawned", (client, position) => 
        {
            this.state.CreatePlayer(client.sessionId, position);
        });

        this.onMessage("Position", (client, position) => 
        {
            this.state.SetPlayerPosition(client.sessionId, position);
        });

        this.onMessage("Rotation", (client, rotation) => 
        {
            this.state.SetPlayerRotation(client.sessionId, rotation);
        });

        this.onMessage("Direction", (client, direction) => 
        {
            this.state.SetPlayerDirection(client.sessionId, direction);
        });

        this.onMessage("Score", (client, score) => 
        {
            this.state.SetPlayerScore(client.sessionId, score);
        });

        this.onMessage("BoostState", (client, state) => 
        {
            this.state.SetBoostState(client.sessionId, state);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) 
    {
        //this.state.CreatePlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.RemovePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
