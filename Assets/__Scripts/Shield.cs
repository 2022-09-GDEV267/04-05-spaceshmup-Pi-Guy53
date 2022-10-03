using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    [Header("Set in Inspector")]
    public float rotationSpeed;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    private int currentLevel;
    private float rotZ;

    Material mat;

    // Use this for initialization
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update()
    {
        currentLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        if(levelShown != currentLevel)
        {
            levelShown = currentLevel;
            mat.mainTextureOffset = new Vector2(.2f * levelShown, 0);
        }

        rotZ = -(rotationSpeed * Time.time * 360) % 360;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

    }
}
