using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int health = 10000;
    private bool regen = false;
    public void damage(int damageAmount)
    {
        
        DamagePopup.Create(transform.position, damageAmount, false);
        health -= damageAmount;
    }

    public void heal(int damageAmount)
    {
        DamagePopup.Create(transform.position, damageAmount, true);
        health += damageAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 9000)
        {
            regen = false;
        } else if (health < 5000)
        {
            regen = true;
        }

        if (regen)
        {
            heal(100);
        }
    }
}
