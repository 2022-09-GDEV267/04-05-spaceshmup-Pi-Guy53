using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float lerpSpeed;

    private GameObject hero;

    public bool isAuto;
    public float fireInterval;
    public float jamChance;
    private bool fired = false;

    public Weapon weapon;
    public WeaponType weaponType;

    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        if (weapon != null)
        {
            weapon.type = weaponType;
        }
    }

    private void Update()
    {
        if (hero != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(hero.transform.position - transform.position), lerpSpeed);
            if(isAuto)
            {
                shoot();
            }
        }
    }

    void shoot()
    {
        if(!fired)
        {
            weapon.Fire();
            fired = true;

            if(Random.value < jamChance)
            {
                CancelInvoke("recoil");
                Invoke("recoil", (jamChance * 10) * Mathf.Clamp(Random.value, .25f, 1));
            }
            else
            {
                Invoke("recoil", fireInterval);
            }
        }
    }

    void recoil()
    {
        CancelInvoke("recoil");
        fired = false;
    }
}