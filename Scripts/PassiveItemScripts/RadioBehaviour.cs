using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBehaviour : PassiveItem
{

    [SerializeField] public int power = 20;
    [SerializeField] public float radius = 20;
    [SerializeField] private ParticleSystem waveParticle;

    public override void Operate(PlayerController player)
    {
        StartCoroutine(PrimaOndata(player));
        StartCoroutine(SecondaOndata(player));
        StartCoroutine(TerzaOndata(player));
        StartCoroutine(QuartaOndata(player));
        StartCoroutine(Duration());
        HideSpawn();
    }
    private void GenerateParticleWave (PlayerController player)
    {
        GameObject onda = Instantiate(waveParticle.gameObject, player.transform.position, player.transform.rotation, player.transform);
        Destroy(onda, 8f);
    }
    private IEnumerator Duration()
    {
        yield return new WaitForSeconds(8);
        DeleteSpawn();
    }
    public override void DeleteSpawn()
    {
        Destroy(spawn);
    }
    private IEnumerator PrimaOndata(PlayerController player)
    {
        yield return new WaitForSeconds(1);
        GenerateParticleWave(player);
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<Infected>().hit(power);
            }
        }
    }
    private IEnumerator SecondaOndata(PlayerController player)
    {
        yield return new WaitForSeconds(3);
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Enemy") {
                c.gameObject.GetComponent<Infected>().hit(power);
            }
        }

    }
    private IEnumerator TerzaOndata(PlayerController player)
    {
        yield return new WaitForSeconds(5);
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<Infected>().hit(power);
            }
        }

    }
    private IEnumerator QuartaOndata(PlayerController player)
    {
        yield return new WaitForSeconds(7);
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<Infected>().hit(power);
            }
        }

    }
}
