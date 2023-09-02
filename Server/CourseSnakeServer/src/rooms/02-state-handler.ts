import { Room, Client, updateLobby } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

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

export class ServerApple extends Schema
{
    @type("int32")
    Id = 0;

    @type(MyVector3)
    Position = new MyVector3(0, 0, 0);

    @type("number")
    Reward = 0;

    constructor (id: number, position: MyVector3, reward: number)
    {
        super();

        this.Position.SetValues(position);
        this.Reward = reward;
    }
}

export class Player extends Schema {
    @type("string")
    Name = "";

    @type(MyVector3)
    Color = new MyVector3(0, 0, 0);

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

    constructor (data: any)
    {
        super();

        this.Position.SetValues(data.Position);
        this.Name = data.Name;
        this.Color.SetValues(data.Color);
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    @type({ map: ServerApple })
    Apples = new MapSchema<ServerApple>();

    AppleId = 0;

    CreatePlayer(sessionId: string, data: any) 
    {
        this.players.set(sessionId, new Player(data));
    }

    RemovePlayer(sessionId: string) 
    {
        if(this.players.has(sessionId) == false)
        {
            console.log("player already delete");
            return;
        }

        this.players.delete(sessionId);
    }

    CreateApple(position: MyVector3, reward: number)
    {
        let id = this.AppleId++;
        let apple = new ServerApple(id, position, reward);
        this.Apples.set(id.toString(), apple);

        if(this.AppleId >= 20000000)
        {
            this.AppleId = 0;
        }
    }

    RemoveApple(key: number)
    {
        if(this.Apples.has(key.toString()))
        {
            this.Apples.delete(key.toString());
        }
    }

    SetPlayerPosition(sessionId: string, position: MyVector3) 
    {
        if(this.players.has(sessionId) == false)
        {
            console.log("player have not");
            return;
        }

        this.players.get(sessionId).SetPosition(position);
    }

    SetPlayerRotation(sessionId: string, rotation: MyVector3) 
    {
        if(this.players.has(sessionId) == false)
        {
            console.log("player have not");
            return;
        }

        this.players.get(sessionId).SetRotation(rotation);
    }

    SetPlayerDirection(sessionId: string, direction: MyVector3) 
    {
        if(this.players.has(sessionId) == false)
        {
            console.log("player have not");
            return;
        }

        this.players.get(sessionId).SetDirection(direction);
    }

    SetBoostState(sessionId: string, state: boolean)
    {
        if(this.players.has(sessionId) == false)
        {
            console.log("player have not");
            return;
        }

        this.players.get(sessionId).SetBoostState(state);
    }

    SetPlayerScore(sessionId: string, score: number) 
    {
        if(this.players.has(sessionId) == false)
        {
            console.log("player have not");
            return;
        }

        this.players.get(sessionId).SetScore(score);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 30;
    OwnerId = "";

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());
        this.setMetadata(options).then(() => updateLobby(this));

        this.onMessage("PlayerSpawned", (client, data) => 
        {
            this.state.CreatePlayer(client.sessionId, data);
        });

        this.onMessage("PlayerDestroyed", (client) => 
        {
            this.state.RemovePlayer(client.sessionId);
        });

        this.onMessage("CreateApple", (client, data) => 
        {
            if(client.sessionId == this.OwnerId)
                this.state.CreateApple(data.Position, data.Reward);
            else
                console.log("dont host");
        });

        this.onMessage("RemoveApple", (client, key) => 
        {
            this.state.RemoveApple(key);
        });

        this.onMessage("EnemyDestroyed", (client, id) => 
        {
            this.state.RemovePlayer(id);
            this.broadcast("EnemyDead", id, {except: client});
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
        if(this.OwnerId == "")
        {
            this.OwnerId = client.sessionId;
        }
    }

    onLeave (client) 
    {
        this.state.RemovePlayer(client.sessionId);

        if(this.clients.length == 0)
            return;

        if(this.OwnerId == client.sessionId)
        {
            this.OwnerId = this.clients[0].sessionId;
        }
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
