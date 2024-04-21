using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class playerTestMove : MonoBehaviourPunCallbacks
{
    public GameObject camera;
    void Start()
    {
        if (!photonView.IsMine)
        {

            camera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float x = Input.GetAxis("Horizontal") * 5 *Time.deltaTime;

            float z = Input.GetAxis("Vertical") * 5 * Time.deltaTime;


            transform.Translate(-x, 0, -z);
        }








    }
}
