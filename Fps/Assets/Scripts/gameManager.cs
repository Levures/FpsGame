using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class gameManager     : MonoBehaviour
{

    private const string playerIdPrefix = "Player";
    private static Dictionary<string, player> players = new Dictionary<string, player>();

    public gameVariables gameVariables;

    public static gameManager instance;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    private void Awake()
    {
        if (instance == null) instance = this; return;
        Debug.LogError("Multiple game managers");
    }

    public static void RegisterPlayer(string netID,  player player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void UnRegisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static player GetPlayer(string playerId)
    {
        return players[playerId];
    }

    public static player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}
