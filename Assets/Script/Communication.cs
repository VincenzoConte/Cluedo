using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Communication : NetworkBehaviour {

    NetworkConnection[] players;
    int turno;
    public const short msg = 1000;

	// Use this for initialization
	void Start () {
        //NetworkManager.singleton.client.RegisterHandler(MsgType.NetworkInfo, InizioTurno);
        if (NetworkServer.active)
        {
            turno = 0;
            players = new NetworkConnection[NetworkServer.connections.Count];
            NetworkServer.connections.CopyTo(players, 0);
            /*for (int i = 0; i < players.Length; i++)
            {
                TargetCambioTurno(players[i], i);
            }*/
            /*players[1].RegisterHandler(1000, InizioTurno);
            players[1].InvokeHandler(1000, new NetworkReader(), Channels.DefaultReliable);
            NetworkServer.SendToClient(players[1].connectionId, MsgType.Highest+5, new EmptyMessage());*/
            TargetCambioTurno(players[1], 0);
        }
	}

    /*private void InizioTurno(NetworkMessage netMsg)
    {
        this.gameObject.GetComponent<OperativaInterfaccia>().setTurnoTrue();
        Debug.Log("mio turno");
    }*/

    // Update is called once per frame
    void Update () {
	}

    [TargetRpc]
    public void TargetCambioTurno(NetworkConnection target, int extra)
    {
        this.gameObject.GetComponent<OperativaInterfaccia>().setTurnoTrue();
    }
}
