using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RangedWeapon : Weapon
{

    public float gittata = 10f;
    public float coolDownTime = 1.5f;
    private Camera _camera;
    private bool _isReloading;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem particlePuntoDiHit;

    public override void Attack()
    {
        if (!_isReloading)
        {
            particle.Play();
            GetComponent<AudioSource>().Play();
            RaycastHit hit;
            Vector3 pos = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(pos);
            Debug.Log(gittata + Camera.main.GetComponent<CameraZoom>().Distance);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, gittata+Camera.main.GetComponent<CameraZoom>().Distance))
            {
                GameObject hitPart = Instantiate(particlePuntoDiHit.gameObject, hit.point, Quaternion.identity);
                Destroy(hitPart, 2f);
                Infected enemy = hit.transform.gameObject.GetComponent<Infected>();

                if (enemy != null)
                {
                    enemy.hit(_player.damage * danno);

                }

            }
            StartCoroutine(HandleCoolDown());
        }
    }

    private IEnumerator HandleCoolDown()
    {
        _isReloading = true;
        _player.crosshair.color = Color.black;
        yield return new WaitForSeconds(coolDownTime);
        _isReloading = false;
        _player.crosshair.color = Color.white;
    }

    public override void Equip()
    {
        base.Equip();
        _isReloading = false;
    }
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
