using System.Collections.Generic;
using UnityEngine;

public class MusicNoteSlider : MonoBehaviour
{
    [SerializeField] List<GameObject> keys;
    [SerializeField] float speed;
    // Start is called before the first frame update

    public List<Transform> instantiatedKeys = new List<Transform>();
    void Start()
    {
        foreach (var key in keys)
        {
            var instantiatedKey = Instantiate(key, transform);

            instantiatedKeys.Add(instantiatedKey.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

    }
}
