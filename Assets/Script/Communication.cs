using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Communication : NetworkBehaviour {

    public int turno;
    public static short msg = MsgType.Highest + 2;
    static OperativaInterfaccia oi;

    // Use this for initialization
    void Start () {
        if (NetworkServer.active)
        {
            turno = Random.Range(0, NetworkServer.connections.Count);
        }
	}

    void OnEnable()
    {
        oi = GameObject.Find("GameManager").GetComponent<OperativaInterfaccia>();
    }

    public static void InizioTurno(NetworkMessage netMsg)
    {
        oi.setTurnoTrue();
    }

    public void CambioTurno()
    {
        if (NetworkServer.active)
        {
            NetworkConnection[] players = gameObject.GetComponent<Connections>().connections;
            turno = (turno + 1) % players.Length;
            while (turno != 0 && players[turno] == null)
            {
                turno = (turno + 1) % players.Length;
            }
            players[turno].Send(msg, new EmptyMessage());
        }
    }

}
