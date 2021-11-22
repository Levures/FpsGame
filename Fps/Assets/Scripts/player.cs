using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;
using System.Linq;

public class player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead { get { return _isDead;  } protected set { _isDead = value; } }

    private int _kills = 0;
    public int kills { get { return _kills; } set { _kills = value; } }

    private int _deaths = 0;
    public int deaths { get { return _deaths; } set { _deaths = value; } }

    Vector3 debugVect;

    [SerializeField]
    private float maxHealth = 100f, deathImpulseForce = 30, deathTorqueForce = 1000;
    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabledOnStart;

    [SerializeField]
    private Transform middlePlayerTransform;
    private Rigidbody rb;

    [SerializeField]
    private playerShoot _playerShoot;

    private AudioSource audiosource;
    [SerializeField]
    private AudioClip[] deathSounds, killSounds;

    [SyncVar]
    public string username = "Noob";

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    public void Setup()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        if (rb == null)
            rb = gameObject.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.freezeRotation = true;
        //Reset ammo count on respawn
        _playerShoot.currentAmmo = _playerShoot.gun.ammoCount;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(gameManager.instance.gameVariables.respawnTime);        

        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(float gunDamage, string sourceID, Vector3 hitPosition)
    {
        if (isDead) return;

        currentHealth -= gunDamage;
        //Debug.Log(transform.name + "health : " + currentHealth);

        if (currentHealth <= 0) Die(sourceID, hitPosition);
    }

    private void Die(string sourceID, Vector3 hitPosition)
    {

        player sourcePlayer = gameManager.GetPlayer(sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
            sourcePlayer.CmdSoundKill();
            gameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }
        deaths++;


        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        rb.freezeRotation = false;

        //Calculates direction vector from self to shooter
        Vector3 playerkiller = sourcePlayer.transform.position;
        Vector3 deathImpulseDirection = playerkiller - transform.position;

        //Rotation force
        float rotDelta = hitPosition.y - middlePlayerTransform.position.y;
        Vector3 rotationAxis = Vector3.Cross(-deathImpulseDirection, Vector3.up);


        rb.AddTorque(-rotationAxis * deathTorqueForce * rotDelta);
        rb.AddForce(-deathImpulseDirection.normalized * deathImpulseForce, ForceMode.Impulse);

        CmdSoundDie();


        StartCoroutine(Respawn());
    }


    [Command]
    private void CmdSoundDie()
    {
        RpcSoundDie();
    }
    [Command]
    private void CmdSoundKill()
    {
        RpcSoundKill();
    }
    [ClientRpc]
    private void RpcSoundDie()
    {
        audiosource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
    }
    [ClientRpc]
    private void RpcSoundKill()
    {
        audiosource.PlayOneShot(killSounds[Random.Range(0, killSounds.Length)]);
    }


}