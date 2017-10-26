using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connections : NetworkBehaviour {

    public NetworkConnection[] connections;

	// Use this for initialization
	void Start () {
        if (NetworkServer.active)
        {
            connections = new NetworkConnection[NetworkServer.connections.Count];
            NetworkServer.connections.CopyTo(connections, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshConnections()
    {
        connections = new NetworkConnection[NetworkServer.connections.Count];
        NetworkServer.connections.CopyTo(connections, 0);
        Communication com = gameObject.GetComponent<Communication>();
        if (connections[com.turno] == null)
            com.CambioTurno();
    }
}
