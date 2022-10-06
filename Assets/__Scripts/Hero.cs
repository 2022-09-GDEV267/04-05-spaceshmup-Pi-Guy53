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

        fireDelegate += TempFire;
    }

    void Start()
    {

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

    }

    public void AbsorbPowerUp(GameObject go)
    {

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

}
