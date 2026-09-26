//===========================================================================
//Author: Cole Bourbina
//Date  : 09-09-2026
//Desc  : Handles all player interaction with World
//Attach: Player
//===========================================================================

using Unity.VisualScripting;
using UnityEngine;
//this is required for loading a scene
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // We need to access the rigidbody2d on the player
    private Rigidbody2D player_rb;
    //we need to have a variable to control the speed of the player
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    //how many jumps the player has performed
    private int numJumps;
    //max number of jumps the player can perform until they need to touch the ground again
    [SerializeField]
    private int maxNumJumps;

    //where on the pl;ayer the hat should be placed
    public GameObject doubleJumpHatLocation;

    public GameObject equippedSuctionCups;
    public GameObject suctionCupsLocation;
    private bool hasSuctionCups = false;
    private GameObject equippedItemSpawn;
    private bool isStuckLeft = false;
    private bool isStuckRight = false;

    void Start()
    {
        //we need to set the player rigibody variable
        //I can only get this component because the rigidbody2d is attached to the player
        //and this script is also attached to this player
        player_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayerLateral();
        jump();
        rotateSuctionCups(suctionCupsLocation);
    }

    private void movePlayerLateral()
    {
        //if A/D/<-/-> are pressed move the player accordingly
        //"Horizontal" is defined in the input section of the project settings
        // the line below will return:
        //0 - no button pressed
        //1 - right arrow pressed
        //-1 - left arrow pressed.
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        flipPlayerSprite(inputHorizontal);
        if (!isStuckLeft && !isStuckRight)
        {
            player_rb.linearVelocity = new Vector2(inputHorizontal * movementSpeed, player_rb.linearVelocityY);
        }
        else if (isStuckRight)
        {
            if (inputHorizontal == -1)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, movementSpeed);
            }
            else if (inputHorizontal == 1)
            {
                player_rb.linearVelocity = new Vector2(inputHorizontal * movementSpeed, player_rb.linearVelocityY);
            }
            else if (inputVertical == 1)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, movementSpeed);
            }
            else if (inputVertical == -1)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, movementSpeed * -1);
            }
            else
            {
                player_rb.linearVelocity = new Vector2(inputHorizontal * movementSpeed, 0f);
            }
        }
        else if (isStuckLeft)
        {
            if (inputHorizontal == 1)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, movementSpeed);
            }
            else if (inputHorizontal == -1)
            {
                player_rb.linearVelocity = new Vector2(inputHorizontal * movementSpeed, player_rb.linearVelocityY);
            }
            else if (inputVertical == 1)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, movementSpeed);
            }
            else if (inputVertical == -1)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocityX, movementSpeed * -1);
            }
            else
            {
                player_rb.linearVelocity = new Vector2(inputHorizontal * movementSpeed, 0f);
            }
        }
    }

    private void rotateSuctionCups(GameObject suctionCups)
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        if (inputHorizontal != 0)
        {
            suctionCups.transform.Rotate(0f, 0f, inputHorizontal * movementSpeed, Space.Self);
        }
        else if (inputVertical != 0)
        {
            suctionCups.transform.Rotate(0f, 0f, inputVertical * movementSpeed, Space.Self);
        }
    }

    private void flipPlayerSprite(float input)
    {
        //this function will help the player face the direction they are moving
        if (input > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (input < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && numJumps <= maxNumJumps)
        {
            if (isStuckLeft)
            {
                isStuckLeft = false;
                player_rb.gravityScale = 1f;
                player_rb.linearVelocity = new Vector2(movementSpeed * -1, jumpForce);
            }
            else if (isStuckRight)
            {
                isStuckRight = false;
                player_rb.gravityScale = 1f;
                player_rb.linearVelocity = new Vector2(movementSpeed * -1, jumpForce);
            }
            else
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, jumpForce);
                numJumps++;
            }
        }
    }


    //This is a prebuilt function that will detect collisions
    //in order to detect collisions both of the following must be true:
    //1. both objects need to have a collider
    //2. one of the objects needs to have a rigidbody
    //3 difference collisions: onenter, onexit, onstay

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OB"))
        {
            Debug.Log("Restart level");
            SceneManager.LoadScene("SampleScene");
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            //This gets the direction for a perpindicular line from the point of collision
            Vector2 hitNormal = collision.GetContact(0).normal;
            numJumps = 1;
            if (hasSuctionCups)
            {
                //This is comparing the direction of the line we got earlier to cardinal directions to determine the side of the collision
                if (Vector2.Dot(hitNormal, Vector2.left) > 0.5f)
                {
                    isStuckLeft = true;
                    player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, 0f);
                    player_rb.gravityScale = 0f;
                }
                else if (Vector2.Dot(hitNormal, Vector2.right) > 0.5f)
                {
                    isStuckRight = true;
                    player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, 0f);
                    player_rb.gravityScale = 0f;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isStuckLeft || isStuckRight)
        {
            isStuckLeft = false;
            isStuckRight= false;
            player_rb.gravityScale = 1f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DoubleJump"))
        {
            maxNumJumps = 2;
            GameObject hat = collision.gameObject;
            equipDoubleJumpHat(hat);
            //Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SuctionCup"))
        {
            hasSuctionCups = true;
            equippedItemSpawn = Instantiate(equippedSuctionCups);
            GameObject suctionCupsCollectable = collision.gameObject;
            equipSuctionCups(equippedItemSpawn);
            Destroy(collision.gameObject);
        }

    }

    private void equipDoubleJumpHat(GameObject hat)
    {
        hat.transform.position = doubleJumpHatLocation.transform.position;
        hat.gameObject.transform.SetParent(this.gameObject.transform);
    }

    private void equipSuctionCups(GameObject suctionCups)
    {
        suctionCups.transform.position = suctionCupsLocation.transform.position;
        suctionCups.gameObject.transform.SetParent(this.gameObject.transform);
        suctionCupsLocation = suctionCups;
    }
}
