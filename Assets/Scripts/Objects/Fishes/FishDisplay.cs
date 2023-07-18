using UnityEngine;

public class FishDisplay : MonoBehaviour
{
    public Fish fish;

    public void DestroyFish()
    {
        Destroy(gameObject);
    }
}
