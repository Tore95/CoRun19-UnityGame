using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform weaponHandler;
    [SerializeField] private ParticleSystem spawnParticle;
    public Transform WeaponHandler => weaponHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadWeapon(Weapon weapon)
    {
        weapon.transform.parent.parent = weaponHandler;
        Transform weaponGrip = weapon.gameObject.transform.parent;
        weaponGrip.localPosition = Vector3.zero;
        weaponGrip.localEulerAngles = Vector3.zero;
        weaponGrip.localScale = Vector3.one;
        weapon.gameObject.SetActive(true);
        spawnParticle.Play();
    }
}
