using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_5 : Enemy
{
    public List<WingArmor> pieces;
    public GameObject wingParent;

    private Vector3 pos1, pos2;
    private float timeStart, duration = 8;

    public float expChance;
    public float exRot;
    public Weapon explosionWeapon;

    private void Start()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].setUp(this);
        }

        pos2 = transform.position;
        setMovePoints();
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            setMovePoints();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = ((1 - u) * pos1) + (u * pos2);

        logicMethod();

        wingParent.transform.position = transform.position;
    }

    void setMovePoints()
    {
        pos1 = pos2;

        pos2.x = Random.Range((-bndCheck.camWidth / 2) + bndCheck.radius, (bndCheck.camWidth / 2) - bndCheck.radius);
        pos2.y = Random.Range(bndCheck.camHeight / 2, bndCheck.camHeight);

        timeStart = Time.time;
    }

    void logicMethod()
    {
        if(Random.value < expChance)
        {
            createExplosion();
        }
    }

    [ContextMenu("explosion")]
    void createExplosion()
    {
        int exCount = 20;
        exRot += 50 * Time.deltaTime;

        for(int i = 0; i < exCount; i++)
        {
            Projectile p = explosionWeapon.explodeProjectile();
            p.type = WeaponType.laser;
            p.transform.position = transform.position;
            p.transform.rotation = Quaternion.Euler(0, 0, ((360 / exCount) * i) + exRot);
            p.transform.position += p.transform.up * 5;

            p.rb.velocity = p.transform.up * 40;
            Destroy(p.gameObject, .75f);
        }
    }

    public void wingDestroyed(WingArmor lostWing)
    {
        pieces.Remove(lostWing);
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "ProjectileHero")
        {
            if (pieces.Count <= 0)
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