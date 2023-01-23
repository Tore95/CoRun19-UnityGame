using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombietest : MonoBehaviour {
    private Infected infected;
    // Start is called before the first frame update
    void Start() {
        infected = GetComponent<Infected>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            infected.walking();
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            infected.running();
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            infected.turnLeft45();
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            infected.turnRight45();
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            infected.hit(10);
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            infected.hit(40);
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            infected.punch(true);
            infected.punch(false);
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            infected.kick();
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            infected.sneeze();
        } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            infected.idle();
        }

    }
}
