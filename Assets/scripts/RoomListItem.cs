using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviourPunCallbacks
{
    string roomName;
    public TMP_Text roomNameLabel;
    public RoomInfo roomInfo;
    public Button joinBtn;
    public TMP_Text teamCatText, teamRatText;

    public string GetName()
    {
        return roomName;
    }

    public void SetName(string name)
    {
        roomName = name;
        roomNameLabel.text = roomName;
    }

    public void JoinRoom()
    {
        LobbyManager.JoinRoom(roomName);
    }

    public void SetRoomInfo(RoomInfo info)
    {
        roomInfo = info;
        joinBtn.GetComponentInChildren<TMP_Text>().text = "Join (" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
        teamCatText.text = "Team Cat (" + roomInfo.CustomProperties["Cat"] + "/4)";
        teamRatText.text = "Team Rat (" + roomInfo.CustomProperties["Rat"] + "/4)";
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["mode"]);
        if (roomInfo != null && (roomInfo.PlayerCount == roomInfo.MaxPlayers))
        {
            joinBtn.interactable = false;
        }
        else
        {
            joinBtn.interactable = true;
        }

        if (roomInfo != null && (int)roomInfo.CustomProperties[PhotonNetwork.LocalPlayer.CustomProperties["mode"]] >= 4)
        {
            joinBtn.interactable = false;
        }
        else
        {
            joinBtn.interactable = true;
        }

    }

}
