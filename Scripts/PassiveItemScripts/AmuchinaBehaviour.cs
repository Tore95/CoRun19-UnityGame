using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuchinaBehaviour : PassiveItem
{
    [SerializeField] public int power = 20;
    [SerializeField] GameObject particlePrefab;
    public override void Operate(PlayerController player)
    {
        if ((player.health + power) > 100)
        {
            player.health = 100;
        }
        else
            player.health += power;
        GameObject instance = Instantiate(particlePrefab, player.transform.position, particlePrefab.transform.rotation, player.transform);
        Destroy(instance, 2f);
        DeleteSpawn();
        Destroy(this);
    }
    public override void DeleteSpawn()
    {
        Destroy(spawn);

    }
}
