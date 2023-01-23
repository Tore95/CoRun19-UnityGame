using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Infected : MonoBehaviour
{

    public float m_Damping = 0.15f;
    public int health = 100;
    public bool healed = false;
    public int damageResistence = 30;
    public int damage = 5;
    public bool isRotating = false;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Material healedMat;
    [SerializeField] private GameObject healedIcon;
    [SerializeField]
    private Animator animator;
    [SerializeField] AudioClip[] infectedClips;
    [SerializeField] AudioClip normalHitClip;
    [SerializeField] AudioClip finalHitClip;
    private float velocity;
    private float previousFrameVelocity;

    private readonly int m_HashVelocityParam = Animator.StringToHash("Velocity");

    public Action AfterHealed { get; set; } = () => { };

    [SerializeField] GameObject UAV;

    // Start is called before the first frame update
    void Start()
    {
        UAV.SetActive(false);
        healedIcon.SetActive(false);
        velocity = 0;
        previousFrameVelocity = 0;
    }

    public void idle()
    {
        velocity = 0;
    }

    public void walking()
    {
        velocity = 0.5f;
    }

    public void running()
    {
        velocity = 1f;
    }

    public void turnRight90()
    {
        if (!healed) animator.SetFloat("TurnMix", 0.8f);
        else animator.SetFloat("TurnMix", 0.5f);
        startRotating(3f);
        animator.SetTrigger("Right");
    }

    public void turnRight45()
    {
        if (!healed) animator.SetFloat("TurnMix", 0.4f);
        else animator.SetFloat("TurnMix", 0.25f);
        startRotating(4f);
        animator.SetTrigger("Right");
    }

    public void turnRightMax()
    {
        animator.SetFloat("TurnMix", 1f);
        startRotating(1.5f);
        animator.SetTrigger("Right");
    }

    public void turnLeft90()
    {
        if (!healed) animator.SetFloat("TurnMix", 0.8f);
        else animator.SetFloat("TurnMix", 0.5f);
        startRotating(3f);
        animator.SetTrigger("Left");
    }

    public void turnLeft45()
    {
        if (!healed) animator.SetFloat("TurnMix", 0.4f);
        else animator.SetFloat("TurnMix", 0.25f);
        startRotating(4f);
        animator.SetTrigger("Left");
    }

    public void turnLeftMax()
    {
        animator.SetFloat("TurnMix", 1f);
        startRotating(1.5f);
        animator.SetTrigger("Left");
    }

    public void hit(int damage)
    {
        if (!healed)
        {
            if (health - damage <= 0)
            {
                healed = true;
                health = 0;
            }
            else
            {
                health -= damage;
            }
            AudioSource audioSource = GetComponent<AudioSource>();
            if (healed)
            {
                audioSource.Stop();
                audioSource.clip = finalHitClip;
                audioSource.Play();
                animator.SetTrigger("Healing");
                this.GetComponentInChildren<SkinnedMeshRenderer>().material = healedMat;
                AfterHealed();
                UAV.SetActive(false);
            }
            else { 
                animator.SetTrigger(damage < damageResistence ? "SoftHit" : "HardHit");
                audioSource.Stop();
                audioSource.clip = normalHitClip;
                audioSource.Play();
            }
            particle.Play();
        }

    }

    public void ActivateHealedIcon()
    {
        healedIcon.SetActive(true);
    }

    public void punch(bool value)
    {
        animator.SetBool("Punch", value);
        // se il collider nel pugno colpisce un oggetto player allora player.hit(damage * 1)
    }

    public void kick()
    {
        animator.SetTrigger("Kick");
        // se il collider nel piede colpisce un oggetto player allora player.hit(damage * 2)
    }

    public void sneeze()
    {
        animator.SetTrigger("Sneeze");
        // se il collider nello starnuto colpisce un oggetto player allora player.hit(damage * 3)
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(m_HashVelocityParam, velocity, m_Damping, Time.deltaTime);
        if (previousFrameVelocity != 1 && velocity == 1)
        {
            PlayInfectedClip();
        }
        previousFrameVelocity = velocity;
    }

    private void PlayInfectedClip()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = infectedClips[UnityEngine.Random.Range(0, infectedClips.Length)];
        audioSource.Play();
    }

    public void setTurningSpeed(float speed)
    {
        animator.SetFloat("TurningSpeed", speed);
    }

    public void finishRotating()
    {
        isRotating = false;
        setTurningSpeed(1f);
    }

    public void startRotating(float speed)
    {
        isRotating = true;
        setTurningSpeed(speed);
    }
    public void UAVactive(bool attivo)
    {
        if (!healed)
        {
            UAV.SetActive(attivo);
        }
    }
}