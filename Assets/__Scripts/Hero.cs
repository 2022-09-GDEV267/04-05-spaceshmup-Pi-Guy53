using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    private bool placeHolderDELETE;

    [Header("Set Dynamically")]
    private bool placeHolderDELETE2;

    void Start()
    {

    }
	
	// Update is called once per frame
	void Update()
    {
    
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
