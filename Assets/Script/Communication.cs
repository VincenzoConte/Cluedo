using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Communication : NetworkBehaviour {

    NetworkConnection[] players;
    int turno;
    public static short msg = MsgType.Highest + 2;
    static OperativaInterfaccia oi;

    // Use this for initialization
    void Start () {
	}

    void OnEnable()
    {
        oi = GameObject.Find("GameManager").GetComponent<OperativaInterfaccia>();
        if (NetworkServer.active)
        {
            turno = -1;
            players = new NetworkConnection[NetworkServer.connections.Count];
            NetworkServer.connections.CopyTo(players, 0);
            /*for(int i = 0; i < players.Length; i++)
            {
                players[i].RegisterHandler(msg, InizioTurno);
            }*/
        }
    }

    public static void InizioTurno(NetworkMessage netMsg)
    {
        oi.setTurnoTrue();

    }

    public void CambioTurno()
    {
        if (NetworkServer.active)
        {
            turno = (turno + 1) % NetworkServer.connections.Count;
            players[turno].Send(msg, new EmptyMessage());
        }
    }

    // Update is called once per frame
    void Update () {
        
    }

}
