using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Communication : NetworkBehaviour {

    NetworkConnection[] players;
    public int turno;
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
            players = new NetworkConnection[NetworkServer.connections.Count];
            NetworkServer.connections.CopyTo(players, 0);
            turno = Random.Range(0, NetworkServer.connections.Count);
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
            turno = (turno + 1) % players.Length;
            while (turno != 0 && players[turno] == null)
            {
                turno = (turno + 1) % players.Length;
            }
            players[turno].Send(msg, new EmptyMessage());
        }
    }

    public void RefreshConnections()
    {
        players = new NetworkConnection[NetworkServer.connections.Count];
        NetworkServer.connections.CopyTo(players, 0);
    }

}
