using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localGunMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerCam;
    [SerializeField]
    private float upOffset = 0.3f, forwardOffset = 0.2f;

    private float originalLocalY, originalLocalX, camEulerX;

    private void Start()
    {
        originalLocalY = transform.localPosition.y;
        originalLocalX = transform.localPosition.x;
    }
    private void Update()
    {
        //Gets angle x of camera : 1 if player look full up, -1 ih he looks full down
        camEulerX = playerCam.localRotation.eulerAngles.x;
        if (camEulerX > 180) camEulerX -= 360;
        camEulerX = (camEulerX * -1) / 90;

        if (camEulerX > 0)
            transform.localPosition = (new Vector3(transform.localPosition.x, originalLocalY + (upOffset * camEulerX), transform.localPosition.z));
        else
            transform.localPosition = (new Vector3(originalLocalX - (forwardOffset * camEulerX), originalLocalY + (upOffset * camEulerX / 3), transform.localPosition.z));
    }

}
