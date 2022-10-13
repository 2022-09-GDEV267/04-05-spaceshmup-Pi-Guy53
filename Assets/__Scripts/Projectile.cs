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

        if(type == WeaponType.phaser)
        {
            t += Time.deltaTime * 15;
            sin = (Mathf.Sin(t) * 10);

            rb.velocity = (transform.up * initialVelocity) + (transform.right * sin * phaseDirection);
        }
        else if(type == WeaponType.missile)
        {
            transform.Rotate(Vector3.forward * trackDirection(target) * 5 * Time.deltaTime);

            rb.velocity = transform.up * initialVelocity;
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
    }

    float trackDirection(Transform pos)
    {
        if(Vector3.Angle(transform.position + transform.forward, target.transform.position) > 15)
        {
            float anglgeRot = Quaternion.LookRotation(target.transform.position, transform.position).eulerAngles.y;

            print(anglgeRot);

            if (anglgeRot >= 180)
            {
                return -1;
            }
            else if(anglgeRot < 180)
            {
                return 1;
            }
        }

        return -999;
    }
}