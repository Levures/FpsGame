    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "weaponData")]
public class playerGun : ScriptableObject
{
    public string name = "Gun";
    public float damage = 45f, 
                 headDamage = 85f, 
                 range = 1000, 
                 rateOfFire = 0.35f;
    public int ammoCount = 6;
    public AudioClip[] shootSounds,
                     reloadSounds,
                     hitSounds;
}
