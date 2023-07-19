using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Singleton instance of the FishPool class
    public static ObjectPool Instance;

    // The size of the pool for each prefab
    [SerializeField] private int poolSize = 10;

    // Dictionary to store a queue of game objects for each prefab
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // Set the singleton instance and initialize the dictionary
        Instance = this;
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    }

    // Get a game object from the pool for the given prefab
    public GameObject GetFromPool(GameObject prefab)
    {
        // If the pool for this prefab doesn't exist, create it
        if (!poolDictionary.ContainsKey(prefab))
        {
            CreateNewPool(prefab);
        }

        // If the pool is empty, add a new game object to it
        if (poolDictionary[prefab].Count == 0)
        {
            AddToPool(prefab);
        }

        // Dequeue and return a game object from the pool
        return poolDictionary[prefab].Dequeue();
    }

    // Return a game object to the pool
    public void ReturnToPool(GameObject obj)
    {
        // Deactivate the game object and add it back to the pool
        obj.SetActive(false);
        poolDictionary[obj.GetComponent<IPoolable>().Prefab].Enqueue(obj);
    }

    // Create a new pool for the given prefab
    private void CreateNewPool(GameObject prefab)
    {
        // Add a new entry to the dictionary for this prefab
        poolDictionary.Add(prefab, new Queue<GameObject>());

        // Fill the pool with game objects
        for (int i = 0; i < poolSize; i++)
        {
            AddToPool(prefab);
        }
    }

    // Add a new game object to the pool for the given prefab
    private void AddToPool(GameObject prefab)
    {
        // Instantiate a new game object from the prefab
        GameObject obj = Instantiate(prefab);

        // Add a Poolable component to store a reference to the prefab
        obj.AddComponent<Poolable>().Prefab = prefab;

        // Deactivate the game object and add it to the pool
        obj.SetActive(false);
        poolDictionary[prefab].Enqueue(obj);
    }
}

// Interface for poolable objects
public interface IPoolable
{
    GameObject Prefab { get; set; }
}

// Class that implements the IPoolable interface
public class Poolable : MonoBehaviour, IPoolable
{
    public GameObject Prefab { get; set; }
}
