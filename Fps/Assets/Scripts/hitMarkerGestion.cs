using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class hitMarkerGestion : MonoBehaviour
{
    public static bool hitMarkerState;
    private RawImage hitMarkerImage;
    private void Start()
    {
        hitMarkerImage = gameObject.GetComponent<RawImage>();
    }
    void Update()
    {
        hitMarkerImage.enabled = hitMarkerState;
    }
}
