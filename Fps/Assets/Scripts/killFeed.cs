using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killFeed : MonoBehaviour
{
    [SerializeField]
    GameObject killFeedItemsPrefab;

    void Start()
    {
        gameManager.instance.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = Instantiate(killFeedItemsPrefab, transform);
        go.GetComponent<killFeedItems>().Setup(player, source);
        Destroy(go, 5f);
    }
}
