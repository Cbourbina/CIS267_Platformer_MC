using Unity.VisualScripting;
using UnityEngine;
//attached to collectable used to move collectable and handle interactions
public class CollectableController : MonoBehaviour
{

    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        setRandomSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        moveLeft();
    }

    private void moveLeft()
    {
        rb.linearVelocity = new Vector2(speed * -1, 0);
    }

    public void setRandomSpeed() 
    {
        float minSpeed = 3;
        float maxSpeed = 10;

        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OB"))
        {
            Destroy(this.gameObject);
        }
    }
}
