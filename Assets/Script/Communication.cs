using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Communication : NetworkBehaviour {

    NetworkConnection[] players;
    int turno;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            turno = 0;
            players = new NetworkConnection[NetworkServer.connections.Count];
            NetworkServer.connections.CopyTo(players, 0);
            TargetCambioTurno(players[turno+1], 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    [TargetRpc]
    public void TargetCambioTurno(NetworkConnection target, int extra)
    {
        this.gameObject.GetComponent<OperativaInterfaccia>().setTurnoTrue();
    }
}
