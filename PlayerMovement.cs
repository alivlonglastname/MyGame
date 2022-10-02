using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D player;
    public Collider2D playerSides;

    public TilemapCollider2D floor;
    public TilemapCollider2D walls;
    

    public int jumpCount = 2; // amount of jumps player has
    public float maxSpeed = 20f; // maximum speed player can reach
    public float accelRate = 0.125f; // at what rate does char move (relative to maxSpeed)
    public float airSpeedModifier = 1f; // at what rate does character slow down in air
    public float airSpeedDecay = 0.995f; // for every frame in air, airSpeedModifier is multiplied by airSpeedDecay
    public float minAirSpeed = 15f; //  minimum air speed when trying to move
    public float jumpHeight = 20f; // velocity given to object when jumping
    public int dashFrames = 10; // how many frames the dash is active
    public float dashSpeed = 3.0f; // modifier for speed when dashing
    public int dashLag = 20; // how long before player can dash again

    


    float speedY = 0f;
    public float speedX = 0f;
    int airtime = 0; // amount of jumps the player has made before touching the ground
    bool canJump = true; // weather the player has jumps available or not
    bool jump = false;
    bool left = false;
    bool right = false;
    //bool dash = false;
    bool wallCling = false;
    bool lockCtrl = false;
    bool canDash = true;
    bool inAir = false;
    bool dashJump = false;
    
    int currDashLag = 0;
    int dash = 0;

    


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // Reset some values
            // Velocity & Movement //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }
        if (!lockCtrl)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dash = 10;
            }
        }

    }
    // TODO: fix dash jump mechanics
    void FixedUpdate()
    {
        
        if (dash != 0 && canDash) // only true if dash key is pressed
        {
            lockCtrl = true; // while dashing, lock all controls
            if (left) // only dash left if holding left, otherwise dash forward by default
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-dashSpeed * maxSpeed, 0);
            } else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(dashSpeed * maxSpeed, 0);
            }
            dash--;
            if (dash < 8 && jump)
            {
                jump = false;
                speedX = 2 * maxSpeed;
                if (left)
                {
                    speedX = -speedX;
                }
                GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, jumpHeight / 1.5f);
                dashJump = true;
                dash = 0;
            }
            return;
        } else
        {
            if (lockCtrl) // can only get to this part of the code after dash is over
            {
                canDash = false;
                currDashLag = dashLag;
            }
            if (currDashLag != 0)
            {
                currDashLag -= 1;

            } else if (floor.IsTouching(player))
            {
                canDash = true;
            }
            lockCtrl = false;
        }
        // Update for gravity
        speedY = GetComponent<Rigidbody2D>().velocity.y;

        inAir = !floor.IsTouching(player);
        wallCling = (walls.IsTouching(playerSides) && inAir);

        if (!inAir && speedY == 0) // if the player is touching the floor, reset airtime, and allow jumping
        {                                                                            // checking for velocity because on the frame were jumping, even though 
            airtime = 0;                                                             // airtime should be incremented its reset to 0 because the char is still
            canJump = true;                                                          // touching the ground technically
            airSpeedModifier = 1f; /// Bad practice to hardcode this, change later ///
        }
        else if (airtime < jumpCount) // if airtime < jump count, allow jumping
        {
            canJump = true;
        }

        if ((jump && canJump) || (jump && wallCling))
        {
            if (wallCling && jump)
            {
                // if wallCling, speedX is always going to be 0... i think
                speedX -= (speedX + 3 * accelRate * maxSpeed);
                speedY += jumpHeight;
                canDash = true;

            }
            else if (jump && canJump)
            {
                airtime += 1;
                speedY += jumpHeight;

            }
        }

        if (left)
        {
            if (!(speedX - accelRate * maxSpeed < -maxSpeed))
            {
                speedX -= accelRate * maxSpeed;
            }
        }

        if (right)
        {
            if (!(speedX + accelRate * maxSpeed > maxSpeed))
            {
                speedX += accelRate * maxSpeed;
            }
        }

        // if neither left or right, make speed = 0;
        if (left == right)
        {
            if (speedX > 0)
            {
                speedX -= accelRate * maxSpeed;
                if (speedX < 0) // this should never happen with base implementation, but just in case
                {
                    speedX = 0;
                }
            }

            if (speedX < 0)
            {
                speedX += accelRate * maxSpeed;
                if (speedX > 0) // this should never happen with base implementation, but just in case
                {
                    speedX = 0;
                }
            }
        }

        if (wallCling) // if the player is touching the floor, reset airtime, and allow jumping
        {
            if (speedY < 0)
            {
                speedY = 2 * speedY / 3;
            }

        }
        
        /// Speed modifiers:

        if (inAir)
        {
            if (dashJump)
            {
                minAirSpeed = maxSpeed;
            } else
            {
                minAirSpeed = 0.75f * maxSpeed;
            }

            if (!(left && (speedX * airSpeedModifier > -minAirSpeed)) &&
                !(right && (speedX * airSpeedModifier < minAirSpeed)))
            {
                // This piece of code makes sure the character doesnt go too slow because of too much air time
                airSpeedModifier = airSpeedModifier * airSpeedDecay;
            } else
            {
                airSpeedModifier = airSpeedModifier / airSpeedDecay;
            }

            if (airSpeedModifier > 1) // some weird bug is making it > 1
            {
                airSpeedModifier = 1;
            }

            speedX = speedX * airSpeedModifier;
        }
        // 
        if (speedX < -maxSpeed && !inAir)
        {
            speedX += accelRate * 2 * maxSpeed;
        } else if (speedX > maxSpeed && !inAir) 
        {
            speedX -= accelRate * 2 * maxSpeed;
        }


        GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);

        // reset all states
        jump = false;
        right = false;
        left = false;
        canJump = false;
        dash = 0;

    }
}

 // TODO: 
    // decrease air speed.