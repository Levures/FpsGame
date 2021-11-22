using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunLookAt : MonoBehaviour
{
    [SerializeField]
    private Camera playerCam;

    private void Update()
    {
        transform.LookAt(playerCam.transform.forward * 1000);
        
    }
}
