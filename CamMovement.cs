using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public Transform playerCharacter;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minY;
    public float maxY;
    public float minX;
    public float maxX;
    void FixedUpdate()
    {
        Vector3 desiredPosition = playerCharacter.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        if (smoothPosition.x < minX)
        {
            smoothPosition.x = minX;
        } else if (smoothPosition.x > maxX)
        {
            smoothPosition.x = maxX;
        }

        if (smoothPosition.y < minY)
        {
            smoothPosition.y = minY;
        }
        else if (smoothPosition.y > maxY)
        {
            smoothPosition.y = maxY;
        }
        transform.position = smoothPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
