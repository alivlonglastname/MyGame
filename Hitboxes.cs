using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitboxes : MonoBehaviour
{
    Enemy temp;
    bool dot = false;
    int cd = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.damage(500);
            temp = enemyComponent;
            dot = true;
            cd = 100;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (dot && cd != 0)
        {
            if (cd % 10 == 0)
            {
                temp.damage(10);
            }
            
            cd--;
        }
    }

}
