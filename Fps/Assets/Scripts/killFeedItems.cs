using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class killFeedItems : MonoBehaviour
{
    [SerializeField]
    private Text killer, Dieur;

    public void Setup(string player, string source)
    {
        killer.text = source;
        Dieur.text = player;

    }
}
