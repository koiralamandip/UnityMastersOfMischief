using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InitializeGame : MonoBehaviour
{
    public TMP_Text playerName;
    PlayerDataSer playerData;
    // Start is called before the first frame update

    void Start()
    {
        if (!CheckForAuthenticatedPlayer())
        {
            LobbyManager.Disconnect();
            ManageAccount();
            return;
        }

        LobbyManager.Init(playerData);
    }

    private bool CheckForAuthenticatedPlayer()
    {
        try
        {
            if (PlayerPrefs.HasKey("linked_account"))
            {
                playerData = JsonUtility.FromJson<PlayerDataSer>(PlayerPrefs.GetString("linked_account"));
                playerName.text = playerData.username;
                return true;
            }

            return false;
        }
        catch(Exception e)
        {
            Debug.Log(e);
            PlayerPrefs.DeleteKey("linked_account");
            return false;
        }
    }

    public void ManageAccount()
    {
        SceneManager.LoadScene("Authentication");
    }
}
