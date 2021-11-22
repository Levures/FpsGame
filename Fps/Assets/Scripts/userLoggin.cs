using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class userLoggin : MonoBehaviour
{
    public static userLoggin instance;
    public static string username;
    public string lobbyName = "Lobby";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void Loggin(Text _username)
    {
        username = _username.text;
        Debug.Log("Logged in as : " + username);
        SceneManager.LoadScene(lobbyName);
    }

}
