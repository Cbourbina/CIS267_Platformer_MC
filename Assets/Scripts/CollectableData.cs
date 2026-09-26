using UnityEngine;
//attached to a collectable used to remove object when needed
//and get the value of the given collectable.
public class CollectableData : MonoBehaviour
{
    [SerializeField]
    private int collectableValue;

    public void destroyCollectable()
    {
        Destroy(this.gameObject);
    }

    public int getCollectable()
    {
        return collectableValue;
    }
}
