using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : Item
{
    
    public int danno = 10;
    [SerializeField] private Sprite spriteMirino;
    //[SerializeField] private Sprite spriteArma;
    //[SerializeField] public Collider pickCollider;
    protected PlayerController _player;
    public Sprite SpriteArma => sprite;


    public abstract void Attack();

    public virtual void Equip()
    {
        this.gameObject.SetActive(true);
        //pickCollider.enabled = false;
        _player.crosshair.sprite = spriteMirino;
        _player.crosshair.color = Color.white;
        //_player.iconaEquip.sprite = spriteArma;
    }
    public void Unequip()
    {
        this.gameObject.SetActive(false);
        //pickCollider.enabled = true;
        //_player.iconaUnequip.sprite = spriteArma;
    }
}
