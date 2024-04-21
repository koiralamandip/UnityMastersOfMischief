using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Luncher : MonoBehaviourPunCallbacks
{
    public PhotonView RatplayerPrefab;
    public PhotonView CatplayerPrefab;
    public GameObject Botton;
    public GameObject CameraScene;
    public GameObject RatBotton;
    public GameObject CatBotton;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); 
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");


        PhotonNetwork.JoinRandomOrCreateRoom();








    }

    public override void OnJoinedRoom()
    {
        Debug.Log("you joind a room ");

         

       
    }


   public void goingToRooms()
    {


        CameraScene.SetActive(false);
        Botton.transform.gameObject.SetActive( false);
    }

    public void imRat()
    {

       
        PhotonNetwork.Instantiate(RatplayerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);

        RatBotton.transform.gameObject.SetActive(false);
    }

    public void imCat()
    {


        PhotonNetwork.Instantiate(CatplayerPrefab.name, new Vector3(28, 0, 0), Quaternion.identity);

        CatBotton.transform.gameObject.SetActive(false);
    }


}
