using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TileUtils : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private WeaponSpawnPoint weaponSpawnPoint;
    public Transform EnemySpawnPoint => enemySpawnPoint;
    public WeaponSpawnPoint WeaponSpawnPoint => weaponSpawnPoint;

    private void Awake()
    {
        weaponSpawnPoint.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
