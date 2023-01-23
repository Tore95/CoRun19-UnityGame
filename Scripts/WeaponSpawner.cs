using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] meleeWeapons;
    public int maxMeleeWeapons;
    [SerializeField] private GameObject[] rangedWeapons;
    public int maxRangedWeapons;

    // Start is called before the first frame update
    void Start()
    {
        maxMeleeWeapons = Math.Min(maxMeleeWeapons, meleeWeapons.Length);
        maxRangedWeapons = Math.Min(maxRangedWeapons, rangedWeapons.Length);
    }

    public void SpawnWeaponInMap(Transform mapRoot)
    {
        int[] tileIndexes = Enumerable.Range(1, mapRoot.childCount - 2).ToArray();
        tileIndexes = ShuffleArray(tileIndexes);
        GameObject[] meleeShuffled = ShuffleArray(meleeWeapons);
        GameObject[] rangedShuffled = ShuffleArray(rangedWeapons);
        for (int i = 0; i < maxMeleeWeapons; i++)
        {
            WeaponSpawnPoint wsp = mapRoot.GetChild(tileIndexes[i]).GetComponent<TileUtils>().WeaponSpawnPoint;
            wsp.gameObject.SetActive(true);
            GameObject newWeapon = Instantiate(meleeShuffled[i]);
            wsp.LoadWeapon(newWeapon.GetComponentInChildren<Weapon>());
        }
        for (int i = 0; i < maxRangedWeapons; i++)
        {
            WeaponSpawnPoint wsp = mapRoot.GetChild(tileIndexes[i+maxMeleeWeapons]).GetComponent<TileUtils>().WeaponSpawnPoint;
            wsp.gameObject.SetActive(true);
            GameObject newWeapon = Instantiate(rangedShuffled[i]);
            wsp.LoadWeapon(newWeapon.GetComponentInChildren<Weapon>());
        }
    }

    //preso dalla Lezione 14
    private T[] ShuffleArray<T>(T[] numbers)
    {
        T[] newArray = numbers.Clone() as T[];
        for (int i = 0; i < newArray.Length; i++)
        {
            T tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
