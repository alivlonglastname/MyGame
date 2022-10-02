using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Abilities : MonoBehaviour
{
    public GameObject playerChar;
    public GameObject sen1;
    public GameObject sen2;
    public BoxCollider2D senHitBox;
    public LineRenderer sentinelLine;
    public bool drawLine = true;
    public int senLag = 10;

    bool trap = false;
    int currSenLag = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            trap = true;
        }
    }

    void FixedUpdate()
    {
        if (trap && currSenLag == 0)
        {
            if (sen1.transform.position.x == -50) // default x for sen1
            {
                sen1.transform.position = playerChar.transform.position;
            }
            else
            {
                sen2.transform.position = playerChar.transform.position;
                // Move sentinels to desired position
                if (drawLine)
                {
                    sentinelLine.SetPosition(0, sen2.transform.position);
                    sentinelLine.SetPosition(1, sen1.transform.position);
                }

                // Move hitbox to desired position
                float angle = 0;
                angle = Mathf.Atan2(sen2.transform.position.y - sen1.transform.position.y,
                    sen2.transform.position.x - sen1.transform.position.x) * 180 / Mathf.PI;
                
                float length = Vector3.Distance(sen1.transform.position, sen2.transform.position);
                float midpX = (sen2.transform.position.x + sen1.transform.position.x) / 2;
                float midpY = (sen2.transform.position.y + sen1.transform.position.y) / 2;

                senHitBox.transform.position = new Vector3(midpX, midpY, 0);
                senHitBox.size = new Vector2(length, 1);
                senHitBox.transform.rotation = Quaternion.Euler(0,0,0);
                senHitBox.transform.Rotate(0, 0, angle);
                currSenLag = senLag;
            }
        }

        if (currSenLag != 0)
        {
            currSenLag -= 1;
        }
        else if (sen2.transform.position.x != -51)
        {
            sen1.transform.position = new Vector3(-50, 0, 0);
            sen2.transform.position = new Vector3(-51, 0, 0);
            sentinelLine.SetPosition(0, sen2.transform.position);
            sentinelLine.SetPosition(1, sen1.transform.position);
            senHitBox.transform.position = new Vector3(-50.5f, 0, 0);
            senHitBox.size = new Vector2(0.5f, 1);
            senHitBox.transform.Rotate(0, 0, 0);
        }

        trap = false;
    }
}
