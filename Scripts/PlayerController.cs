using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public enum States { UNARMED, MELEE, FIREARM };

public class PlayerController : MonoBehaviour {

    public int health = 100;
    public int shild = 10;
    public int damage = 1;
    public float speed = 1;
    public float stepDurationWalk = 0.42f;
    public float stepDurationRun = 0.28f;
    public float animationSmooth = 0.15f;
    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
    public Vector3 weaponScale;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private MeleeWeapon unarmed;
    [SerializeField] private MeleeWeapon melee;
    [SerializeField] private RangedWeapon firearm;
    [SerializeField] public Image crosshair;
    [SerializeField] public Image equipIcon;
    [SerializeField] public Image unequipIconOne;
    [SerializeField] public Image unequipIconTwo;
    [SerializeField] private SliderUpdater healthBar;
    [SerializeField] private SliderUpdater shieldBar;
    [SerializeField] private GameObject controller;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject notification;
    [SerializeField] public GameObject minimapIcon;
    [SerializeField] private AudioClip footstepConcrete;
    [Range(0.0f,1.0f)] public float footStepVolume = 0.05f;
    [SerializeField] private AudioClip hurtClip;
    [Range(0.0f, 1.0f)] public float hurtVolume = 1;
    private States state;
    private Weapon currentEquipped;
    private bool isDead = false;
    private Animator animator;
    private Weapon switchingWeapon = null;
    private WeaponSpawnPoint switchingWSP = null;
    public RunManager runManager { get; private set; }
    private float deathRotationY;
    private Color playerColor = Color.white;
    private readonly int m_HashHorizontalPara = Animator.StringToHash("Horizontal");
    private readonly int m_HashVerticalPara = Animator.StringToHash("Vertical");
    private AudioSource audioSource;
    private bool isStep = false;
    public bool IsStanding { get; set; }
    // Start is called before the first frame update
    void Start() {
        Debug.Log("Ho chiamato start");
        animator = GetComponent<Animator>();
        runManager = controller.GetComponent<RunManager>();
        IsStanding = false;
        unarmed.Equip();
        firearm.Unequip();
        melee.Unequip();

        currentEquipped = unarmed;

        setState(States.UNARMED);
        audioSource = GetComponent<AudioSource>();
        Collect(firearm,false);
        Collect(melee,false);
        SetWeaponIcons(unarmed.SpriteArma, firearm.SpriteArma, melee.SpriteArma);
        //unequipIconOne.sprite = melee.SpriteArma;
        //equipIcon.sprite = unarmed.SpriteArma;

        Debug.Log(runManager != null);

    }


    void Update() {
        movementInput();
        ChangeWeapon();
        attack();
        SetStatistics();
        SwitchWeapon(switchingWeapon, switchingWSP);
        if (isDead)
        {
            transform.rotation = Quaternion.Lerp(
                this.transform.rotation,
                Quaternion.Euler(this.transform.rotation.x,deathRotationY,this.transform.rotation.z),
                Time.deltaTime*10);
        }
        playerRenderer.material.color = Color.Lerp(playerRenderer.material.color,playerColor,Time.deltaTime*5);
    }

    public void equipRanged() {
        if (firearm != null) {
            unarmed.Unequip();
            firearm.Equip();
        }
        currentEquipped = firearm;
    }

    public void unequipRanged() {
        if (firearm != null) {
            firearm.Unequip();
        }
        currentEquipped = unarmed;
        unarmed.Equip();
    }

    public void equipMelee() {
        if (melee != null) {
            unarmed.Unequip();
            melee.Equip();
        }
        currentEquipped = melee;
    }

    public void unequipMelee() {
        if (melee != null) {
            melee.Unequip();
        }
        currentEquipped = unarmed;
        unarmed.Equip();
    }

    private void ChangeWeapon() {
        float changeInput = Input.GetAxis("Mouse ScrollWheel"); //Metti tasto che ti piace
        if (changeInput < 0) {
            prevState();
        } else if (changeInput > 0) {
            nextState();
        }
    }

    //Inserire trigger per raccogliere gli oggetti


    private void DropWeapon(Weapon weapon) {

    }

    //Da completare
    public void hit(int damage, Transform enemy) {
        playerRenderer.material.color = Color.green;
        audioSource.PlayOneShot(hurtClip, hurtVolume);
        if (shild == 0) {
            if ((health - damage) < 0) {
                health = 0;
            } else {
                health -= damage;
            }
            Debug.Log("danno alla vita: " + damage);

        } else {
            //int damageVita = damage % 2 == 0 ? damage / 2 : (damage / 2) + 1;
            int damageScudo = damage / 2;
            int damageVita = damage - damageScudo;
            if ((shild - damageScudo) < 0)
            {
                damageVita += damageScudo - shild;
                shild = 0;
            }
            else
            {
                shild -= damageScudo;
            }
            if ((health - damageVita) < 0) {
                health = 0;
            } else {
                health -= damageVita;
            }
            Debug.Log("danno alla vita: " + damageVita);
            Debug.Log("danno allo scudo: " + damageScudo);

        }
        
        Death(enemy);
    }

    void movementInput() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            animator.SetBool("Running", true);
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            animator.SetBool("Running", false);
        }
        if((Mathf.Abs(horizontal)> 0.5f || Mathf.Abs(vertical)>0.5 ) && !isStep && !IsStanding)
        {
            StartCoroutine(StepSound(speed));
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            runManager.Pause();
        }
        Vector2 input = new Vector2(horizontal, vertical).normalized;

        animator.SetFloat(m_HashHorizontalPara, input.x, animationSmooth, Time.deltaTime);
        animator.SetFloat(m_HashVerticalPara, input.y, animationSmooth, Time.deltaTime);
    }

    private IEnumerator StepSound(float speed)
    {
        isStep = true;
        audioSource.PlayOneShot(footstepConcrete,footStepVolume);
        yield return new WaitForSeconds(stepDurationRun/speed);
        if (animator.GetBool("Running"))
        {
            isStep = false;
            yield break;
        }
        yield return new WaitForSeconds((stepDurationWalk-stepDurationRun)/ speed);
        isStep = false;
    }

    void attack() {
        if (Input.GetMouseButtonDown(0) && currentEquipped != null && !runManager.IsPaused && !runManager.IsEnded) {
            animator.SetTrigger("Attack");
            currentEquipped.Attack();
            Debug.Log("Sparo con" + (state == States.MELEE ? "mischia" : "pistola"));
        }
        //Inserire parte di animazioni
    }

    private void OnTriggerEnter(Collider other) {
        WeaponSpawnPoint otherSP = other.gameObject.GetComponent<WeaponSpawnPoint>();
        if (otherSP != null)
        {
            switchingWSP = otherSP;
            switchingWeapon = otherSP.WeaponHandler.GetComponentInChildren<Weapon>();
        }
        if (other.gameObject.tag == "PassiveItem")
        {
            PassiveItem passive = other.gameObject.GetComponent<PassiveItem>();
            passive.Operate(this);
            Collect(passive);

        }
        if (other.gameObject.tag == "BlockingFence") {
            runManager.FinalTileEntrance();
        }
        if (other.gameObject.tag == "Tutorial")
        {
            Tutorial tuto = other.gameObject.GetComponent<Tutorial>();
            tuto.execute(notification);
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            switchingWeapon = null;
            switchingWSP = null;
        }
    }

    private void SwitchWeapon(Weapon weapon, WeaponSpawnPoint otherSpawnPoint) {

        if (Input.GetKeyDown(KeyCode.E) && weapon != null && otherSpawnPoint != null) {
            weapon.transform.parent.parent = hand;
            weapon.gameObject.transform.parent.localPosition = weaponPosition;
            weapon.gameObject.transform.parent.localEulerAngles = weaponRotation;
            weapon.gameObject.transform.parent.localScale = weaponScale;
            Collect(weapon);
            if (weapon is MeleeWeapon) {
                otherSpawnPoint.LoadWeapon(melee);
                switchingWeapon = melee;
                //mischia.pickCollider.enabled = true;
                melee = weapon as MeleeWeapon;
            } else {
                otherSpawnPoint.LoadWeapon(firearm);
                switchingWeapon = firearm;
                //daFuoco.pickCollider.enabled = true;
                firearm = weapon as RangedWeapon;
            }
            switch (state)
            {
                case States.FIREARM:
                    SetWeaponIcons(firearm.SpriteArma, melee.SpriteArma, unarmed.SpriteArma);
                    firearm.Equip();
                    melee.Unequip();
                    unarmed.Unequip();
                    currentEquipped = firearm;
                    break;
                case States.MELEE:
                    SetWeaponIcons(melee.SpriteArma, unarmed.SpriteArma, firearm.SpriteArma);
                    melee.Equip();
                    firearm.Unequip();
                    unarmed.Unequip();
                    currentEquipped = melee;
                    break;
                case States.UNARMED:
                    SetWeaponIcons(unarmed.SpriteArma, firearm.SpriteArma, melee.SpriteArma);
                    unarmed.Equip();
                    firearm.Unequip();
                    melee.Unequip();
                    currentEquipped = unarmed;
                    break;
            }
        }
    }

    void Death(Transform enemy) {
        if (health == 0 && !isDead) {
            Debug.Log("morto");
            //runManager.Death();
            animator.SetTrigger("Death");
            runManager.playDeathMusic();
            Quaternion oldRotation = transform.rotation;
            transform.LookAt(enemy);
            deathRotationY = this.transform.eulerAngles.y;
            transform.rotation = oldRotation;
            isDead = true;
            foreach(MouseLook ml in GetComponentsInChildren<MouseLook>())
            {
                ml.enabled = false;
            }
        }
    }

    public void DisableMeleeHitbox() {
        melee.DisableHitbox();
    }
    void Collect(Item item, bool showNotification = true) {
        Dictionary<string, Item> lista = controller.GetComponent<CollectiblesManager>().Collectibles;
        if (!lista.ContainsKey(item.itemName)) {
            lista.Add(item.itemName, item);
            
        }
        List<string> CollectLista = controller.GetComponent<CollectiblesManager>().CollectiblesList;
        if (!CollectLista.Contains(item.itemName))
        {
            CollectLista.Add(item.itemName);
        }
        Debug.Log("Raccolto Item");
        //notification.GetComponent<NotificationExample>().descriptionText = "Che forza!";
        //notification.GetComponent<NotificationExample>().titleText = name;
        if (showNotification)
        {
            notification.GetComponent<NotificationExample>().ShowNotification(item.itemName, item.description, item.sprite);
        }
        controller.GetComponent<CollectiblesManager>().Save();
    }
    void CollectPassive(Item item)
    {

    }

    private void SetWeaponIcons(Sprite equip, Sprite unequipOne, Sprite unequipTwo) {
        equipIcon.sprite = equip;
        unequipIconOne.sprite = unequipOne;
        unequipIconTwo.sprite = unequipTwo;
    }

    private void nextState() {
        switch (state) {
            case States.UNARMED:
                setState(States.MELEE);
                SetWeaponIcons(melee.SpriteArma, unarmed.SpriteArma,firearm.SpriteArma);
                break;
            case States.MELEE:
                setState(States.FIREARM);
                SetWeaponIcons(firearm.SpriteArma, melee.SpriteArma,unarmed.SpriteArma);
                break;
            case States.FIREARM:
                setState(States.UNARMED);
                SetWeaponIcons(unarmed.SpriteArma, firearm.SpriteArma,melee.SpriteArma);
                break;
        }
    }

    private void prevState() {
        switch (state) {
            case States.UNARMED:
                setState(States.FIREARM);
                SetWeaponIcons(firearm.SpriteArma, melee.SpriteArma,unarmed.SpriteArma);
                break;
            case States.MELEE:
                setState(States.UNARMED);
                SetWeaponIcons(unarmed.SpriteArma, firearm.SpriteArma,melee.SpriteArma);
                break;
            case States.FIREARM:
                setState(States.MELEE);
                SetWeaponIcons(melee.SpriteArma, unarmed.SpriteArma,firearm.SpriteArma);
                break;
        }
    }

    private void setState(States newState) {
        switch (newState) {
            case States.UNARMED:
                animator.SetBool("MeleeWeapon", false);
                animator.SetBool("RangedWeapon", false);
                animator.SetBool("NoWeapon", true);
                //SetWeaponIcons(unarmed.SpriteArma, currentEquipped.SpriteArma);
                break;
            case States.MELEE:
                animator.SetBool("RangedWeapon", false);
                animator.SetBool("NoWeapon", false);
                animator.SetBool("MeleeWeapon", true);
                //SetWeaponIcons(melee.SpriteArma, currentEquipped.SpriteArma);
                break;
            case States.FIREARM:
                animator.SetBool("MeleeWeapon", false);
                animator.SetBool("NoWeapon", false);
                animator.SetBool("RangedWeapon", true);
                //SetWeaponIcons(firearm.SpriteArma, currentEquipped.SpriteArma);
                break;
        }
        state = newState;
        animator.ResetTrigger("Attack");
    }

    void SetStatistics()
    {
        healthBar.SetValue(health);
        shieldBar.SetValue(shild);
        animator.speed = speed;
    }
}
