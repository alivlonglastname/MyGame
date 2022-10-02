using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopup : MonoBehaviour
{

    float speedX = -10;
    float speedY = 0;
    //create a damage popup
    public static DamagePopup Create(Vector3 position, int damageAmount, bool healing)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.DamageNumObj, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, healing);

        return damagePopup;
    }

    private TextMeshPro text;
    private float disappearTimer;
    private Color textColor;
    private void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int damage, bool healing)
    {
        if (healing)
        {
            textColor = new Color32(3, 195, 17, 255);
        } else
        {
            textColor = new Color32(156, 20, 20, 255);
        }
        text.SetText(damage.ToString());
        text.color = textColor;
        disappearTimer = 0.3f;
    }

    private void Update()
    {
        if (speedX == -10)
        {
            
            speedX = Random.Range(-4, 5);
            speedY = 10f;
        } else
        {
            speedX += speedX * 10 / Random.Range(5, 100) * Time.deltaTime;
        }
       
        transform.position += new Vector3(speedX, speedY) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            text.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
