using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield,
    torpedo //possible addition sperate from missile
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;

    public GameObject projectilePref; //base system, use obj pooling

    public Color projectileColor = Color.white;
    public float damage = 0;
    public float DOTdamge = 0;
    public float shotDelay = 0;
    public float velocity;
}
public class Weapon : MonoBehaviour
{
    public static Transform PROJECTILE_ANCHOR;

    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;

    private Renderer collarRend;

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(def.type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGo = transform.root.gameObject;
        if(rootGo.GetComponent<Hero>() != null)
        {
            rootGo.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public void Fire()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }

        if(Time.time - lastShotTime < def.shotDelay)
        {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch(type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rb.velocity = vel;
                break;

            case WeaponType.spread:
                p = MakeProjectile();
                p.rb.velocity = vel;

                p = MakeProjectile();
                p.rb.velocity = vel;
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rb.velocity = p.transform.rotation * vel;

                p = MakeProjectile();
                p.rb.velocity = vel;
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rb.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public WeaponType type
    {
        get
        {
            return _type;
        }
        set
        {
            SetType(value);
        }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePref);
        if(transform.parent.gameObject.CompareTag("Hero"))
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();

        p.type = def.type;

        lastShotTime = Time.time;

        return p;
    }
}