using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject connectionPanel;
    public TMP_InputField roomNameField;
    public TMP_Text roomNameLabel, roomOwnerLabel;
    public GameObject roomPanel, lobbyPanel;

    public RoomListItem roomListItemPrefab;
    public GameObject roomListItemHolder;
    List<RoomListItem> roomListItems = new();
    private float timeBetweenRoomListUpdates = 1f;
    private float nextUpdateTime;

    public PlayerListItem playerListItemPrefab;
    public GameObject playerListItemCatHolder, playerListItemRatHolder;
    List<PlayerListItem> playerListItems = new();

    public Button playButton, createButton;
    public TMP_Dropdown teamDropdown;

    public TMP_Text teamCatText, teamRatText;

    public static void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (connectionPanel)
                connectionPanel.SetActive(false);
            if (PhotonNetwork.InRoom)
            {
                AfterJoiningRoom();
            }
            else
            {
                createButton.interactable = true;
                roomPanel.SetActive(false);
                lobbyPanel.SetActive(true);
            }

            return;
        }
        playButton.interactable = false;

        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);

        if (connectionPanel)
            connectionPanel.SetActive(true);

    }

    public static void Init(PlayerDataSer playerData)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (!PhotonNetwork.InRoom)
            {
                PhotonNetwork.JoinLobby();
            }
            return;
        }
        PhotonNetwork.NickName = playerData.username;
        PhotonNetwork.EnableCloseConnection = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public static void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void CreateRoom()
    {
        if (roomNameField.text.Length < 1) return;
        Hashtable roomProperties = new()
        {
            {"Cat", 0},
            { "Rat", 0}
        };

        roomProperties[PhotonNetwork.LocalPlayer.CustomProperties["mode"]] = 1;

        PhotonNetwork.CreateRoom(roomNameField.text, new RoomOptions { CustomRoomProperties=roomProperties, MaxPlayers = 8, BroadcastPropsChangeToAll = true, CustomRoomPropertiesForLobby = new string[] { "Cat", "Rat" } });
        roomNameField.text = "";
    }

    public void LeaveRoom()
    {
        Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
        int count = (int)props[PhotonNetwork.LocalPlayer.CustomProperties["mode"]];
        props[PhotonNetwork.LocalPlayer.CustomProperties["mode"]] = count - 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        PhotonNetwork.LeaveRoom();
    }

    public void PlayGame()
    {
        if (!PhotonNetwork.InRoom) return;
    }

    void UpdatePlayerList()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        foreach (PlayerListItem playerListItem in playerListItems)
        {
            Destroy(playerListItem.gameObject);
        }
        playerListItems.Clear();

        foreach(KeyValuePair<int, Player> _player in PhotonNetwork.CurrentRoom.Players)
        {
            Hashtable playerProps = _player.Value.CustomProperties;
            Transform holder = (string)playerProps["mode"] != "Cat" ? playerListItemRatHolder.transform : playerListItemCatHolder.transform;
            PlayerListItem playerListItem = Instantiate(playerListItemPrefab, holder);
            playerListItem.SetPlayer(_player.Value);
            playerListItems.Add(playerListItem);
        }

        roomOwnerLabel.text = "Master: " + PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.MasterClientId).NickName;
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        if (Time.time < nextUpdateTime) return;

        nextUpdateTime = Time.time + timeBetweenRoomListUpdates;
        foreach (RoomListItem roomListItem in roomListItems)
        {
            Destroy(roomListItem.gameObject);
        }

        roomListItems.Clear();

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                continue;
            }
            RoomListItem roomListItem = Instantiate(roomListItemPrefab, roomListItemHolder.transform);
            roomListItem.SetName(roomInfo.Name);
            roomListItem.SetRoomInfo(roomInfo);
            roomListItems.Add(roomListItem);
        }
    }

    void AfterJoiningRoom()
    {
        createButton.interactable = false;
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomNameLabel.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        teamCatText.text = "Team Cat (" + PhotonNetwork.CurrentRoom.CustomProperties["Cat"] + "/4)";
        teamRatText.text = "Team Rat (" + PhotonNetwork.CurrentRoom.CustomProperties["Rat"] + "/4)";
        UpdatePlayerList();
        PlayButton();
    }

    void PlayButton()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.MasterClientId))
        {
            playButton.interactable = true;
            playButton.GetComponentInChildren<TMP_Text>().text = "Play";

            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Cat"] != (int)PhotonNetwork.CurrentRoom.CustomProperties["Rat"])
            {
                playButton.interactable = false;
                playButton.GetComponentInChildren<TMP_Text>().text = "Inequal team. Can't play";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        createButton.interactable = true;
        playButton.interactable = false;
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        if (connectionPanel)
            connectionPanel.SetActive(false);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("mode"))
        //    PhotonNetwork.LocalPlayer.CustomProperties.Add("mode", teamDropdown.options[teamDropdown.value].text);
        PhotonNetwork.SetPlayerCustomProperties(new Hashtable { { "mode", teamDropdown.options[teamDropdown.value].text } });
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (connectionPanel)
            connectionPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            int count = (int)roomProperties[PhotonNetwork.LocalPlayer.CustomProperties["mode"]];
            roomProperties[PhotonNetwork.LocalPlayer.CustomProperties["mode"]] = count + 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }

        AfterJoiningRoom();
    }

    public override void OnLeftRoom()
    {
        if (roomPanel)
            roomPanel.SetActive(false);
        if (lobbyPanel)
            lobbyPanel.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        teamCatText.text = "Team Cat (" + propertiesThatChanged["Cat"] + "/4)";
        teamRatText.text = "Team Rat (" + propertiesThatChanged["Rat"] + "/4)";
        playButton.GetComponentInChildren<TMP_Text>().text = "Play";
        PlayButton();
    }

}
