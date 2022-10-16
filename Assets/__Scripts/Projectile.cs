using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    public Rigidbody rb;
    [SerializeField]
    private WeaponType _type;

    //any weapon modifing velocity
    float initialVelocity;

    //phaser
    float t, sin;
    int phaseDirection;

    //missile
    Transform target;
    private Weapon firingWeapon;

    //laser
    float DoT;

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

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bndCheck.offUp || bndCheck.offDown)
        {
            Destroy(gameObject);
        }

        if (type == WeaponType.phaser)
        {
            t += Time.deltaTime * 15;
            sin = (Mathf.Sin(t) * 10);

            rb.velocity = (transform.up * initialVelocity) + (transform.right * sin * phaseDirection);
        }
        else if (type == WeaponType.missile)
        {
            if (target == null)
            {
                setTarget();
            }
            else
            {
                rb.velocity = (target.transform.position - transform.position).normalized * initialVelocity;
            }
        }
    }

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }

    public void setPhaserVelocity(float vel, int phDir)
    {
        initialVelocity = vel;
        phaseDirection = phDir;
    }

    public void setUpMissile(float vel, Weapon fw)
    {
        initialVelocity = vel;
        firingWeapon = fw;

        setTarget();

        TrailRenderer tr = gameObject.AddComponent<TrailRenderer>();
        tr.material = rend.material;
        tr.time = .5f;
        tr.startWidth = .5f;
        tr.endWidth = 0;

        Destroy(gameObject, 3);
    }

    public void setLaser(float dmgOverTime)
    {
        DoT = dmgOverTime;
    }

    public float getDoT()
    {
        return DoT;
    }

    void setTarget()
    {
        Enemy[] goList = GameObject.FindObjectsOfType<Enemy>();
        if (goList.Length > 0)
        {
            target = goList[Random.Range(0, goList.Length)].transform;
        }
    }

    private void OnDestroy()
    {
        if(type == WeaponType.missile)
        {
            int explosionShots = 12;

            for(int i = 0; i < explosionShots; i++)
            {
                Projectile p = firingWeapon.explodeProjectile();
                p.type = WeaponType.blaster;
                p.transform.position = transform.position;
                p.transform.rotation = Quaternion.Euler(0, 0, (360 / explosionShots) * i);
                p.transform.position += p.transform.up * 5;

                p.rb.velocity = p.transform.up * 40;

                Destroy(p.gameObject, .25f);
            }
        }
    }
}