using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScoreboardLine : MonoBehaviour
{

    [SerializeField]
    private Text nameText, killsText, deathsText;

    public void Setup(player player)
    {
        nameText.text = player.username;
        killsText.text = player.kills.ToString();
        deathsText.text = player.deaths.ToString();
    }
}
