using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    public float speed = 30f;
    public float rollMulti = -45;
    public float pitchMulti = 30;

    [Header("Set Dynamically")]
    private float shieldLEvel = 1;

    float xAxis, yAxis;
    Vector3 pos;

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
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void AbsorbPowerUp(GameObject go)
    {

    }

    public float shieldLevel
    {
        get;set;
    }

}
