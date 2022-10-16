using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float weaponJamChance = .2f;
    public float health = 10;
    public int score;

    public Weapon[] weapons;
    public WeaponType weaponType;

    protected BoundsCheck bndCheck;

    public float showDamageDuration = .1f;
    public Color[] originalColors;
    public Material[] materials;

    public bool showingDamage;
    public float damageDoneTime;
    public bool notifiedOfDestruction = false;

    public float powerUpDropChance;

    private Vector3 tempPos;

    private bool canFire;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();

        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        Invoke("setWeapon", fireRate);
    }

    void setWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].type = weaponType;
        }

        canFire = true;
    }

    // This is a property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }

        if (canFire)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].Fire();
            }

            canFire = false;

            if (Random.value < weaponJamChance)
            {
                CancelInvoke("fireEnd");
                Invoke("fireEnd", (weaponJamChance * 10) * Mathf.Clamp(Random.value, .25f, 1));
            }
            else
            {
                CancelInvoke("fireEnd");
                Invoke("fireEnd", fireRate);
            }
        }
    }

    void fireEnd()
    {
        CancelInvoke("fireEnd");
        canFire = true;
    }

    public virtual void Move()
    {
        tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "ProjectileHero")
        {
            Projectile p = otherGO.GetComponent<Projectile>();
            if (!bndCheck.isOnScreen)
            {
                Destroy(otherGO);
                return;
            }

            ShowDamage();

            health -= Main.GetWeaponDefinition(p.type).damage;

            if (health <= 0)
            {
                if (!notifiedOfDestruction)
                {
                    Main.S.ShipDestroyed(this);
                    notifiedOfDestruction = true;
                }

                Destroy(gameObject);
            }

            Destroy(otherGO);
            return;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}