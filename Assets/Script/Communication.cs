using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

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
            turno = 0;
            players = new NetworkConnection[NetworkServer.connections.Count];
            NetworkServer.connections.CopyTo(players, 0);
            /*for(int i = 0; i < players.Length; i++)
            {
                players[i].RegisterHandler(msg, InizioTurno);
            }*/
            players[turno].Send(msg, new EmptyMessage());
            Debug.Log("messaggio inviato");
        }
    }

    public static void InizioTurno(NetworkMessage netMsg)
    {
        oi.setTurnoTrue();
        Debug.Log("mio turno");
    }

    // Update is called once per frame
    void Update () {
        
    }

}
