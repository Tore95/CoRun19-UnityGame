using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAssigner : MonoBehaviour
{
    [SerializeField] PassiveSpawnPoint passiveSpawnPrefab;
    [SerializeField] GameObject[] dropPrefabs;
    [Range(0,100)] public int dropPercentage = 34;

    private int _remainingPercentage;

    public void SetDrop(GameObject enemy)
    {
        Infected infected = enemy.GetComponent<Infected>();
        if(infected != null)
        {
            //Debug.Log(_remainingPercentage);
            _remainingPercentage += dropPercentage;
            if (_remainingPercentage >= 100)
            {
                //infected.SetDrop(passiveSpawnPrefab, dropPrefabs[0]);
                int dropIndex = Random.Range(0, dropPrefabs.Length);
                infected.AfterHealed = () => SpawnDrop(dropPrefabs[dropIndex], infected);
                _remainingPercentage %= 100;
            }
        }
    }

    private void SpawnDrop(GameObject drop, Infected infected)
    {
        PassiveSpawnPoint newPassiveSp = Instantiate(passiveSpawnPrefab, infected.transform.position, Quaternion.identity, infected.transform.parent);
        newPassiveSp.GetComponent<PassiveSpawnPoint>().InstantiatePassiveObj(drop);
    }

    private void Awake()
    {
        _remainingPercentage = Random.Range(0, 100);
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
