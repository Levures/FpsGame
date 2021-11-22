using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject head;

    private void Update()
    {
        transform.position = head.transform.position;
    }
}
