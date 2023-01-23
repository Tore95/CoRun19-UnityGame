using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneBehaviour : PassiveItem
{
    
    [SerializeField] public int duration = 20;
    [SerializeField] private GameObject particlePrefab;
    public override void Operate(PlayerController player)
    {
        player.speed *= 2;
        GameObject particleInstance = Instantiate(
            particlePrefab, 
            player.transform.position + new Vector3(0, 0.5f, 0),
            particlePrefab.transform.rotation,
            player.transform);
        StartCoroutine(Duration(player,particleInstance));
        //DeleteSpawn();
        //Destroy(this);
        HideSpawn();
    }
    private IEnumerator Duration(PlayerController player,GameObject particle)
    {
        yield return new WaitForSeconds(20);
        player.speed /= 2;
        Destroy(particle);
        DeleteSpawn();
    }
    public override void DeleteSpawn()
    {
        Destroy(spawn);
    }
}
