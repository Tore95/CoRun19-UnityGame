using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascherinaBehaviour : PassiveItem
{
    [SerializeField] public int power = 20;
    [SerializeField] GameObject particlePrefab;
    public override void Operate(PlayerController player)
    {
        if ((player.shild + power) > 50)
        {
            player.shild = 50;
        }
        else
            player.shild += power;
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
