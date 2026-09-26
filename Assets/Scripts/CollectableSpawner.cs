using UnityEngine;
//this will be attached to a random gameObject. It dies not need to be attached to the collectable
public class CollectableSpawner : MonoBehaviour
{

    //these will be used to determine where to spawn the collectable
    //I will get these values from the empty gameObjects we created by referencing their x and y values
    public GameObject lowestYSpawn;
    public GameObject largestYSpawn;

    //These are public variables that allow me to drag and drop our prefabs
    public GameObject tealCollectable;
    public GameObject purpleCollectable;
    public GameObject redCollectable;

    //random number to determine which of the collectables to spawn
    private int randomNum;

    //which collectable to spawn
    private GameObject collectableToSpawn;

    //we need a referenve to time so we can determine how often to spawn a collectable
    private float time;

    //we need a delay to specify how long to wait between spawns
    public float delay;


    // Update is called once per frame
    void Update()
    {
        //add to time. see how much time has passed since the last frame
        time += Time.deltaTime;

        if (time >= delay)
        {
            spawnObject();
            //reset our timer to 0
            time = 0f;
        }
    }

    private void spawnObject()
    {
        //get a random number to determine which object to spawn

        randomNum = Random.Range(0, 3);

        if (randomNum == 0)
        {
            //if the number is 0 spawn a teal collectable
            collectableToSpawn = Instantiate(tealCollectable);
        }
        else if (randomNum == 1)
        {
            //if the number is 1 spawn a purple collectable
            collectableToSpawn = Instantiate(purpleCollectable);
        }
        else if (randomNum == 2)
        {
            //if the number is 2 spawn a red collectable
            collectableToSpawn = Instantiate(redCollectable);
        }

        //telll the collectable where to spawn
        collectableToSpawn.transform.position = new Vector2 (lowestYSpawn.transform.position.x, Random.Range(lowestYSpawn.transform.position.y, largestYSpawn.transform.position.y));
    }
}
