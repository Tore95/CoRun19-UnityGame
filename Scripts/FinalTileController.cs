using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTileController : MonoBehaviour
{
    [SerializeField] private Transform[] enemySpawnPoints;
    public int totalEnemies;
    [SerializeField] VictoryPortal victoryPortal;
    private int _killCount;
    [SerializeField] private Sprite victorySprite;

    private RunManager runManager;

    // Start is called before the first frame update
    void Start()
    {
        _killCount = 0;
       // SpawnEnemies();
        runManager = GameObject.Find("Controller").GetComponent<RunManager>();
    }

    //Questa funzione spawna gli enemy nel tile finale
    public void SpawnEnemies()
    {
        int quoziente = totalEnemies / enemySpawnPoints.Length;
        int resto = totalEnemies % enemySpawnPoints.Length;
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            int numEnemies = quoziente;
            if(resto > 0)
            {
                numEnemies++;
                resto--;
            }
            GetComponent<EnemySpawner>().SpawnEnemiesInTile(numEnemies, enemySpawnPoints[i], ConfFinalEnemy);
        }
    }

    void ConfFinalEnemy(GameObject instance)
    {
        Infected infected = instance.GetComponent<Infected>();
        if(infected != null)
        {
            infected.AfterHealed = ReceiveDeath;
        }
    }

    //Questa funzione viene chiamata quando un enemy nel tile finale viene curato
    void ReceiveDeath()
    {
        _killCount++;
        if(_killCount >= totalEnemies)
        {
            HandleVictory();
        }
    }

    //Questa funzione viene chiamata quanto tutti gli enemy nel tile finale sono curati
    void HandleVictory()
    {
        victoryPortal.gameObject.SetActive(true);
        runManager.Victory();
        runManager.Notification.ShowNotification(
            "Orda finale sanificata", 
            "È possibile accedere alla stazione per il prossimo livello",
            victorySprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
