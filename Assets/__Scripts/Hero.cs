using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float shieldStrength; //the amount of damage the shield can take before dropping one level;
    private float shieldDamage;
    public Image shieldDamageImg;
    public GameObject shieldFlash;

    private GameObject lastTriggerGo = null;

    float xAxis, yAxis;
    Vector3 pos;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private BoundsCheck bndCheck;

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

        bndCheck = GetComponent<BoundsCheck>();

        shieldDamage = 0;
    }

    void Start()
    {
        ClearWeapons();
        weapons[0].type = WeaponType.blaster;

        shieldFlash.SetActive(false);
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

        shieldDamageImg.fillAmount = (1 / shieldStrength) * shieldDamage;

        shieldDamageImg.transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject.transform.root.gameObject;

        if(go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        if (go.CompareTag("Enemy"))
        {
            shieldLevel--;
            //if () //FIX SELECTOR, SHOULD NOT KILL BOSS (enemy_5)
            {
                Destroy(go);
            }
        }
        else if (go.CompareTag("PowerUp"))
        {
            AbsorbPowerUp(go);
        }
        else if (other.CompareTag("ProjectileEnemy")) //using other directly to bypass the fact that a projectiles root is the projectile anchor
        {
            shieldDamage += Main.GetWeaponDefinition(other.GetComponent<Projectile>().type).damage;

            Destroy(other.gameObject);

            if (shieldDamage >= shieldStrength)
            {
                shieldLevel--;
                shieldDamage = 0;

                shieldFlash.SetActive(true);
                Invoke("endFlash", .25f);
            }

            lastTriggerGo = other.gameObject;
        }
    }

    void endFlash()
    {
        shieldFlash.SetActive(false);
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();

        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                shieldDamage = 0;
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
