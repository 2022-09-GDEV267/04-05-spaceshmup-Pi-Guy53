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

    float t, sin;
    float initialVelocity;
    int phaseDirection;

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
}