using UnityEngine;

public class FishingControlls : MonoBehaviour
{
    [SerializeField] CatchArea catchArea;
    private bool isFishing = false;
    [SerializeField] Bait baitScript;
    // Update is called once per frame
    void Update()
    {
        if (isFishing)
        {
            ReturnBait();

        }
        else
        {
            ThrowBait();

        }
    }

    private void ReturnBait()
    {
        if (catchArea.isInCatchArea)
        {
            Debug.Log("Get em");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (catchArea.isInCatchArea)
            {
                Debug.Log("Catch");
            }

            SetFishing();
        }

    }

    private void ThrowBait()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("up");
            SetFishing();
        }
    }

    private void SetFishing()
    {
        isFishing = !isFishing;
        Debug.Log(isFishing);
        baitScript.SetBait();
    }

}
