using UnityEngine;

public class BatController : MonoBehaviour
{
    //connected to the bat
    //it will use a simple path finding algorithm to find the player and move towards them

    private GameObject player;
    private Vector2 playerLocation;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerLocation = player.transform.position;
        //move the bat towards the player
        transform.position = Vector2.MoveTowards(transform.position, playerLocation, speed * Time.deltaTime);
    }
}
