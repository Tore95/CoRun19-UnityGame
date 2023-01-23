using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : Item
{
    public GameObject spawn;
    public abstract void Operate(PlayerController player);
    public abstract void DeleteSpawn();

    public void HideSpawn()
    {
        spawn.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 10, this.transform.position.z);
    }
    
}
