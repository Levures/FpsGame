using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class playerShoot : NetworkBehaviour
{
    public playerGun gun;
    private float lastShot = 0f;
    [HideInInspector]
    public int currentAmmo;

    //Input
    private bool shoot, reload;
    private bool reloading;

    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private GameObject impactParicles, bullet, gunCanon;

    [SerializeField]
    private ParticleSystem localMuzzleFlash;
    [SerializeField]
    private GameObject localGunCanon;
    [SerializeField]
    private Animator wingmanAnimator;


    [SerializeField]
    private float bulletSpeed = 500f, forwardOffset = 0.1f, hitmarkerTime = 0.1f;
    private AudioSource audiosource;



    private void Start()
    {
        currentAmmo = gun.ammoCount;
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        shoot = Input.GetButtonDown("Fire1");
        reload = Input.GetButtonDown("Reload");

        if (shoot && Time.time >= lastShot + gun.rateOfFire && currentAmmo > 0 && !reloading)
            Shoot();
        
        else if (shoot && Time.time >= lastShot + gun.rateOfFire && currentAmmo <= 0 && !reloading)
            Reload();

        if (reload && !reloading && currentAmmo != gun.ammoCount && Time.time >= lastShot + 0.25f /*(0.25 = legnth of recoil anim)*/)        
            Reload();

        ammoDisplay(currentAmmo, gun.ammoCount);
    
    }

    [SerializeField]
    private Text ammo;

    public void ammoDisplay(int currentAmmo, int baseAmmo)
    {
        ammo.text = currentAmmo.ToString() + " / " + baseAmmo.ToString();
    }


//Info to the server when the player shoots
[Command]
    void cmdOnShoot()
    {
        RpcPlayShootParticles();
        RpcShootBullet();
    }
    //Play shooting particles on all clients
    [ClientRpc]
    void RpcPlayShootParticles()
    {
        muzzleFlash.Play();
        localMuzzleFlash.Play();
        
        audiosource.PlayOneShot(gun.shootSounds[Random.Range(0, gun.shootSounds.Length)]);
    }
    //Instantiate bullet
    [ClientRpc]
    void RpcShootBullet()
    {
        if (!isLocalPlayer)
        {
            //Instantiate bullet
            GameObject projectile = Instantiate<GameObject>(bullet, gunCanon.transform.position + transform.forward * forwardOffset, transform.rotation);
            //Give it's velcity
            projectile.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(projectile, 2);
        }
        else
        {
            //Instantiate bullet
            GameObject projectile = Instantiate<GameObject>(bullet, localGunCanon.transform.position + transform.forward * forwardOffset, transform.rotation);
            //Give it's velcity
            projectile.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(projectile, 2);
        }
        
    }
    //Same with impact particles
    [Command]
    void cmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcPlayHitParticles(pos, normal);
    }
    [ClientRpc]
    void RpcPlayHitParticles(Vector3 pos, Vector3 normal)
    {
        GameObject hitObject = Instantiate(impactParicles, pos, Quaternion.LookRotation(normal));
        Destroy(hitObject, 1f);
    }


    [Client]
    private void Shoot()
    {
        //Don't shoot if player isn't the local player
        if (!isLocalPlayer) return;

        lastShot = Time.time;

        cmdOnShoot();
        
        wingmanAnimator.Play("wingmanRecoil");

        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, gun.range, layerMask))
        {
            Debug.Log("hit " + hit.collider.name);
            if (hit.collider.tag == "Player")
            {
                Debug.Log("hit " + hit.collider.GetType());
                if (hit.collider.GetType() == typeof(SphereCollider))
                {
                    cmdPlayerShot(hit.collider.name, gun.headDamage, transform.name, hit.point);
                }
                else
                {
                    cmdPlayerShot(hit.collider.name, gun.damage, transform.name, hit.point);
                }
            }

            cmdOnHit(hit.point, hit.normal);
        }
        //Lose 1 bullet when shooting
        currentAmmo -= 1;
    }

    private void Reload()
    {
        //Don't reload if player isn't the local player
        if (!isLocalPlayer) return;

        reloading = true;
        wingmanAnimator.Play("reloadAnim");
        audiosource.PlayOneShot(gun.reloadSounds[Random.Range(0, gun.reloadSounds.Length)]);

        Invoke(nameof(ReloadEnd), 1.1f);
    }

    private void ReloadEnd()
    {
        currentAmmo = gun.ammoCount;
        reloading = false;
    }

    [Command]
    private void cmdPlayerShot(string playerId, float damage, string sourceID, Vector3 hitPosition)
    {
        Debug.Log(playerId + "hit");

        player player = gameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage, sourceID, hitPosition);

        HitMarker();

    }
    private void HitMarker()
    {
        Debug.Log("hitmarker");
        if (isLocalPlayer)
        {
            audiosource.PlayOneShot(gun.hitSounds[0]);
            hitMarkerGestion.hitMarkerState = true;
            Invoke(nameof(ResetHitMarker), hitmarkerTime);
        }        
    }

    private void ResetHitMarker()
    {
        hitMarkerGestion.hitMarkerState = false;
    }

}
