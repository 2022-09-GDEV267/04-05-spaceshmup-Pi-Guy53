using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    public float speed = 30f;
    public float rollMulti = -45;
    public float pitchMulti = 30;

    public float gameRestartDelay = 2;

    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 4;

    private GameObject lastTriggerGo = null;

    float xAxis, yAxis;
    Vector3 pos;

    public GameObject projectile;
    public float projectileSpeed = 40;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private void Awake()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign a second Hero.S!");
        }
    }

    void Start()
    {
        ClearWeapons();
        weapons[0].type = WeaponType.missile;
    }

    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");

        pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;

        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMulti, xAxis * rollMulti, 0);

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    void TempFire()
    {
        GameObject projGo = Instantiate(projectile);
        projGo.transform.position = transform.position;

        Projectile proj = projGo.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        projGo.GetComponent<Rigidbody>().velocity = Vector3.up * Main.GetWeaponDefinition(proj.type).velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject.transform.root.gameObject;
        if(go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        if(go.CompareTag("Enemy"))
        {
            shieldLevel--;
            Destroy(go);
        }
        else if(go.CompareTag("PowerUp"))
        {
            AbsorbPowerUp(go);
        }

    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();

        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;

            default:
                if(pu.type == weapons[0].type)
                {
                    Weapon w = getEmptyWeaponSlot();
                    if(w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }

        pu.AbsorbedBy(gameObject);
    }

    public float shieldLevel
    {
        get
        {
            return _shieldLevel;
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if(value < 0)
            {
                Destroy(gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    Weapon getEmptyWeaponSlot()
    {
        for(int i = 0; i<weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }

        return null;
    }

    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }

    public Vector2 get2DPos()
    {
        return new Vector2(pos.x, pos.y);
    }

}
