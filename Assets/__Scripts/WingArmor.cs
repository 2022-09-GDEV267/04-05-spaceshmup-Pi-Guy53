using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingArmor : MonoBehaviour
{
    public float health;

    public float showDamageDuration = .1f;
    private Color[] originalColors;
    private Material[] materials;

    private bool showingDamage;
    public float damageDoneTime;

    private Enemy_5 parentShip;

    private void Awake()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    public void setUp(Enemy_5 _parentShip)
    {
        parentShip = _parentShip;
    }

    private void Update()
    {
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "ProjectileHero")
        {
            Projectile p = otherGO.GetComponent<Projectile>();

            ShowDamage();

            health -= Main.GetWeaponDefinition(p.type).damage;

            if (health <= 0)
            {
                parentShip.wingDestroyed(this);
                Destroy(gameObject);
            }

            Destroy(otherGO);
            return;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}