using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : Weapon
{

    [SerializeField] private Collider _meleeCollider;
    private bool _isAttacking;
    private string key;
    private float startCollider = 0.49f;
    private float endCollider = 1.05f;
    [SerializeField] private AudioClip clip;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _meleeCollider.enabled = false;
        _isAttacking = false;
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame


    public override void Attack()
    {
        if (!_isAttacking)
        {
            StartCoroutine(ActiveCollider(_player.speed));
            _isAttacking = true;
        }
    }

    private IEnumerator ActiveCollider(float speed)
    {
        yield return new WaitForSeconds(startCollider/speed);
        GetComponent<AudioSource>().Play();
        _meleeCollider.enabled = true;
        yield return new WaitForSeconds((endCollider - startCollider)/speed);
        _meleeCollider.enabled = false;
    }

    public void DisableHitbox()
    {
        //_meleeCollider.enabled = false;
        _isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Infected script = other.gameObject.GetComponent<Infected>();
        if (script != null)
        {
            if (!script.healed)
            {
                GetComponent<AudioSource>().PlayOneShot(clip);
            }
            script.hit(danno * _player.damage);
        }
    }

}
