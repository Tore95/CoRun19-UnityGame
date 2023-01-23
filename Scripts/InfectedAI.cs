using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfectedAI : MonoBehaviour {

    public float radiusAllert = 10f;        //raggio di allerta //valori di prova
    public float attackAllert = 1;
    //public Transform spawn;                 //posizione del punto di spawn in cui sarà centrato il proprio wander
    public float wanderRadius = 40f;        //distanza dal punto di spawn in cui può vagare //valori di prova
    
    public float obstacleRange = 1f;

    private bool _isPlayerAquired = false;    //definisce se ha un target
    private GameObject _player;               //GameObject del player che seguirà
    //private float distanceFromStart = 20f;  //distanza massima che può raggiungere, probabilmente non verrà usata
    //private GameObject lastPlayer;
    private Infected _infected;
    private EnemyAttack attackController;
    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;
    private bool healed = false;
    private float _distance;
    private readonly int[] rayMapping = { 0, 1, -1, 2, -2, 3, -3 };

   

    void Start() {
        
        _infected = gameObject.GetComponent<Infected>();
        attackController = GetComponent<EnemyAttack>();
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update() {
        float distance = SearchForPlayer();
        healed = _infected.healed;
        if (healed) attackPlayer(false);
        if (_isPlayerAquired && !healed) {
            //float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < attackAllert) attackPlayer(true);
            else followPlayer();
        } else {
            Wander();
        }
    }

    private void attackPlayer(bool value) {
        attackController.attack(value);
    }

    private void followPlayer() {
        attackPlayer(false);
        Quaternion rotTarget = Quaternion.LookRotation(new Vector3(_player.transform.position.x - transform.position.x, 0, _player.transform.position.z - transform.position.z));
        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, 400f * Time.deltaTime);
        _infected.running();
    }

    private void LateUpdate() {
        Lost();
    }

    float SearchForPlayer() {
        /*if (isPlayerAquired) {
            if (Vector3.Distance(transform.position, player.transform.position) > radiusAllert) {
                isPlayerAquired = false;
                //player = null;
            }
        } else {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radiusAllert);
            foreach (Collider c in colliders) {
                if (c.gameObject.tag == "Player") {
                    isPlayerAquired = true;
                    player = c.gameObject;
                }
            }
        }*/
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        _isPlayerAquired =  distance <= radiusAllert;
        return distance;
    }

    void Wander() {
        //if ((Vector3.Distance(transform.position, puntoSpawn) < wanderRadius))
        if (!_infected.isRotating && detectCollider()) {
            int angle = findAngle();
            //Debug.Log(angle);
            switch (angle) {
                case 1: 
                    _infected.turnRight45(); 
                    break;
                case -1: 
                    _infected.turnLeft45(); 
                    break;
                case 2: 
                    _infected.turnRight90(); 
                    break;
                case -2: 
                    _infected.turnLeft90(); 
                    break;
                case 3: 
                    _infected.turnRightMax(); 
                    break;
                case -3: 
                    _infected.turnLeftMax(); 
                    break;
                default: 
                    //Debug.Log("Direzione non trovata"); 
                    _infected.turnRightMax(); 
                    break;
            }
        } else {
            _infected.walking();
        }

    }
    void backtoSpawn() {
        transform.position = _spawnPosition;
        transform.rotation = _spawnRotation;
    }

    void Lost() {
        if (!_isPlayerAquired && (Vector3.Distance(transform.position, _spawnPosition) > wanderRadius)) {
            //Debug.Log("entro nel back to spawn");
            backtoSpawn();
        }

    }

    private bool detectCollider() {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.5f, out hit, obstacleRange)) {
            return true;
        }
        return false;
    }

    private int findAngle() {
        RaycastHit hit;
        //Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        foreach (int i in rayMapping) {
            Quaternion delta = Quaternion.Euler(0, 45 * i, 0);
            Vector3 direction = delta * transform.forward;
            Vector3 origin = transform.position; //+ new Vector3(0,1,0);
            Ray ray = new Ray(origin, direction);
            if (!Physics.SphereCast(ray, 0.5f, out hit, obstacleRange)) {
                //Debug.Log("Angolo libero trovato");
                return i;
            }
        }
        return 0;
    }
    public void Decreto(bool fermi)
    {
        radiusAllert = fermi ? 0 : 10;
    }
}

