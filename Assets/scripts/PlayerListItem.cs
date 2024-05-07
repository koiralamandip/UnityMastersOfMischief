using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    string playerName;
    public TMP_Text playerNameLabel;
    public Button kickButton;
    Player player;

    public void SetPlayer(Player _player)
    {
        player = _player;
        playerName = _player.NickName;
        playerNameLabel.text = player.IsLocal ? playerName + " (Me)" : playerName;
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            kickButton.gameObject.SetActive(false);
        }

        if (player.IsMasterClient)
        {
            kickButton.gameObject.SetActive(false);
        }
    }

    public void OnGameModeChange(TMP_Dropdown teamDropdown)
    {
        if (PhotonNetwork.InRoom)
        {
            Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            string new_mode = teamDropdown.options[teamDropdown.value].text;
            if ((int)roomProperties[new_mode] >= 4) return;

            string old_mode = (string)PhotonNetwork.LocalPlayer.CustomProperties["mode"];
            int countPrevTeam = ((int)roomProperties[old_mode]);
            roomProperties[old_mode] = countPrevTeam - 1;

            Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            playerProperties["mode"] = new_mode;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);

            int countNewTeam = (int)roomProperties[new_mode];
            roomProperties[new_mode] = countNewTeam + 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(PhotonNetwork.CurrentRoom.CustomProperties);

        }
        else
        {
            string new_mode = teamDropdown.options[teamDropdown.value].text;
            Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            playerProperties["mode"] = new_mode;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
    }

    public void KickPlayer()
    {
        Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
        int count = (int)props[player.CustomProperties["mode"]];
        props[player.CustomProperties["mode"]] = count - 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        PhotonNetwork.CloseConnection(player);
    }

}
