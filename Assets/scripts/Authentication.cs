using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Authentication : MonoBehaviour
{
    public TMP_InputField reg_username, reg_password, log_username, log_password;
    public TextMeshProUGUI infoReg, infoLog;
    public Button logButton, regButton, logoutButton;
    public GameObject linkedPanel, loginPanel;
    public TMP_Text linkedText;
    TMP_Text logBtnText, regBtnText;

    // Start is called before the first frame update 
    void Start()
    {
        logBtnText = logButton.gameObject.GetComponentInChildren<TMP_Text>();
        regBtnText = regButton.gameObject.GetComponentInChildren<TMP_Text>();
        CheckAndApplyForLinkedAccount();
    }


    void CheckAndApplyForLinkedAccount()
    {
        if (PlayerPrefs.HasKey("linked_account"))
        {
            linkedPanel.SetActive(true);
            PlayerDataSer playerData = JsonUtility.FromJson<PlayerDataSer>(PlayerPrefs.GetString("linked_account"));
            linkedText.text = playerData.username;
            loginPanel.SetActive(false);
            return;
        }

        linkedPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void AuthenticateStart()
    {
        StartCoroutine(Authenticate());
    }

    IEnumerator Authenticate(){

        string jsonString = "{\"username\":\"" + log_username.text + "\", \"password\":\"" + log_password.text + "\"}";
        UnityWebRequest request = UnityWebRequest.Post("https://masters-of-mischief-3.onrender.com/authenticate", jsonString, "application/json");

        logBtnText.text = "Authenticating ...";
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            infoLog.text = "Couldn't authenticate. Try again";
            infoLog.color = Color.red;
        }
        else
        {
            string data = request.downloadHandler.text;
            PlayerPrefs.SetString("linked_account", data);
        }
        logBtnText.text = "Link Account";
        log_username.text = "";
        log_password.text = "";

        CheckAndApplyForLinkedAccount();
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("linked_account");

        CheckAndApplyForLinkedAccount();
    }

    public void RegisterStart()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {

        string jsonString = "{\"username\":\"" + reg_username.text + "\", \"password\":\"" + reg_password.text + "\"}";
        UnityWebRequest request = UnityWebRequest.Post("https://masters-of-mischief-3.onrender.com/register", jsonString, "application/json");

        regBtnText.text = "Registering ...";
        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
        if (request.result != UnityWebRequest.Result.Success)
        {
            infoReg.text = "Couldn't register. Try again";
            infoReg.color = Color.red;
        }
        else
        {
            infoReg.text = "Registration Successful! username: " + reg_username.text;
            infoReg.color = Color.green;
        }
        regBtnText.text = "Register";
        reg_username.text = "";
        reg_password.text = "";
    }
}
