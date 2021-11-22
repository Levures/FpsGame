using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class playerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] compToDisable;
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    [SerializeField]
    SkinnedMeshRenderer[] localMeshsToDisable, clientslMeshsToDisable;
    [SerializeField]
    ParticleSystem[] serverParticleSystemsToDisable, localParticleSystemsToDisable;
    [SerializeField]
    private Material playerMaterial;
    [SerializeField]
    private Color[] colorPalette;



    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComp();
            DisableOnClients();
            AssignRemoteLayer();
        }
        else
        {
            DisableOnLocal();
            playerMaterial.SetColor("_playerColor", colorPalette[Random.Range(0, colorPalette.Length)]);
        }

        GetComponent<player>().Setup();

        CmdSetUsername(transform.name, userLoggin.username);
    }
    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        player player = gameManager.GetPlayer(playerID);
        if (player != null)
        {
            Debug.Log(username + " joined");
            player.username = username;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        player player = GetComponent<player>();

        gameManager.RegisterPlayer(netID, player);
    }

    private void DisableComp()
    {

       for (int i = 0; i < compToDisable.Length; i++)
       {
           compToDisable[i].enabled = false;
       }
       foreach (ParticleSystem sys in localParticleSystemsToDisable)
       {
            sys.enableEmission = false;
       }      

    }
    private void DisableOnLocal()
    {
        foreach (SkinnedMeshRenderer mesh in localMeshsToDisable)
        {
            mesh.enabled = false;
        }
        foreach (ParticleSystem sys in serverParticleSystemsToDisable)
        {
            sys.enableEmission = false;
        }
    }
    private void DisableOnClients()
    {
        foreach (SkinnedMeshRenderer mesh in clientslMeshsToDisable)
        {
            mesh.enabled = false;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable()
    {
        gameManager.UnRegisterPlayer(transform.name);
    }
}
