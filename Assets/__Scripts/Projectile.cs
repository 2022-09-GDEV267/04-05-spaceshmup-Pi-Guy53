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
    Transform endTarget;

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
        if (bndCheck.offUp)
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
                Destroy(gameObject);
            }
            else
            {
                endTarget.transform.position = Vector3.Lerp(endTarget.transform.position, target.transform.position, .025f);

                rb.velocity = (endTarget.transform.position - transform.position).normalized * initialVelocity;
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

    public void setUpMissile(float vel, Transform targ)
    {
        initialVelocity = vel;
        target = targ;

        endTarget = new GameObject("endTarget").transform;
        endTarget.transform.position = target.transform.position;

        TrailRenderer tr = gameObject.AddComponent<TrailRenderer>();
        tr.material = rend.material;
        tr.time = .5f;
        tr.startWidth = .5f;
        tr.endWidth = 0;
    }

    private void OnDestroy()
    {
        if (endTarget!=null)
        {
            Destroy(endTarget.gameObject);
        }
    }
}