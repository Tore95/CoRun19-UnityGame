using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecretoBehaviour : PassiveItem
{

    [SerializeField] public float duration = 20;
    [SerializeField] private GameObject particlePrefab;
    private GameObject enemyRoot;
    private void Start()
    {
        enemyRoot = GameObject.Find("Map Root");
    }
    public override void Operate(PlayerController player)
    {
        enemyRoot.BroadcastMessage("Decreto", true);
        GameObject particleInstance = Instantiate(
            particlePrefab,
            player.transform.position + new Vector3(0, 0.5f, 0),
            particlePrefab.transform.rotation,
            player.transform);
        StartCoroutine(Duration(particleInstance));
        HideSpawn();
    }
    private IEnumerator Duration(GameObject particleInstance)
    {
        yield return new WaitForSeconds(duration);
        enemyRoot.BroadcastMessage("Decreto", false);
        Destroy(particleInstance);
        DeleteSpawn();
    }
    public override void DeleteSpawn()
    {
        Destroy(spawn);
    }
}
