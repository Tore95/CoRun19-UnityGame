using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Infected))]

public class EnemyAttack : MonoBehaviour {

    public Collider punchHitbox;
    [SerializeField] private AudioClip[] attackClips;
    private Infected infected;

    // Start is called before the first frame update
    void Start() {
        DisableHitbox();
        infected = GetComponent<Infected>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void attack(bool value) {
        infected.punch(value);
    }

    public void EnableHitbox() {
        punchHitbox.enabled = true;
    }
    
    //La chiamo come event dell'animazione
    public void Scream()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = attackClips[UnityEngine.Random.Range(0, attackClips.Length)];
        audioSource.Play();
    }

    public void DisableHitbox() {
        punchHitbox.enabled = false;
    }

}
