using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreboard : MonoBehaviour
{

    [SerializeField]
    private GameObject playerScoreLine;
    [SerializeField]
    private Transform playerScoreBoardList;

    //Update scoreboard value on enable
    private void OnEnable()
    {        
        player[] playersArray = gameManager.GetAllPlayers();

        foreach (player player in playersArray)
        {
            GameObject itemGO = Instantiate(playerScoreLine, playerScoreBoardList);
            playerScoreboardLine line = itemGO.GetComponent<playerScoreboardLine>();
            if (line != null)
            {
                line.Setup(player);
            }
        }
    }
    //Clear scoreboard on disable
    private void OnDisable()
    {
        foreach (Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }        
    }

}
