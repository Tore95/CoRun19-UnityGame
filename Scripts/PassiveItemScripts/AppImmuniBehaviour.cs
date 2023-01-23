using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppImmuniBehaviour : PassiveItem
{

    [SerializeField] public float duration = 20;
    private GameObject enemyRoot;
    private void Start()
    {
        enemyRoot = GameObject.Find("Map Root");
    }
    public override void Operate(PlayerController player)
    {
        enemyRoot.BroadcastMessage("UAVactive", true);
        StartCoroutine(Duration());
        HideSpawn();
    }
    private IEnumerator Duration()
    {
        yield return new WaitForSeconds(duration);
        enemyRoot.BroadcastMessage("UAVactive", false);
        DeleteSpawn();
    }
    public override void DeleteSpawn()
    {
        Destroy(spawn);
    }
}
