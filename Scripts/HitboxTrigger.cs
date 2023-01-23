using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxTrigger : MonoBehaviour
{
    private int _damage;

    void Start()
    {
        Infected infectedParent = this.GetComponentInParent<Infected>();
        if (infectedParent != null)
        {
            _damage = infectedParent.damage;
        }
        else
        {
            Debug.LogError("Component Infected non presente nel parent");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            player.hit(_damage,this.transform);
            //Debug.Log(other.gameObject.name +" colpito: danno "+_damage); // poi qua ci va player.hit(damage * 1)
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
