using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUI : MonoBehaviour
{

    [SerializeField]
    private GameObject scoreboard, statsLine;

    private void Start()
    {
        if (scoreboard != null && statsLine != null)
        {
            scoreboard.SetActive(false);
            statsLine.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (scoreboard != null && statsLine != null)
            {
                scoreboard.SetActive(true);
                statsLine.SetActive(true);
            }

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (scoreboard != null && statsLine != null)
            {
                scoreboard.SetActive(false);
                statsLine.SetActive(false);
            }

        }
    }
}

