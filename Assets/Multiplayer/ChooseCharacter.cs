using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{

    public class ChooseCharacter : LobbyHook
    {

        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //gamePlayer.transform.Rotate(new Vector3(0, 0, 270));
            GamePlayer player = gamePlayer.GetComponent<GamePlayer>();
            LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            player.color = lobby.playerColor;
            player.character = lobby.playerName;
        }
    }
}
