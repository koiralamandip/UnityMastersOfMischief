using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antiStreatch : MonoBehaviour
{
    Vector3 startPosition;
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.localPosition = startPosition;
    }
}
