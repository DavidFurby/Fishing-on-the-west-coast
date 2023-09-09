using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitArea : MonoBehaviour, IInteractive
{
    // Make sceneName a public field so its value can be set in the Unity editor
    [SerializeField] private string sceneName;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        Transition();
    }
    
    public void Transition()
    {
        // Check if sceneName is not null before starting the transition
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(TransitWithFade(sceneName));
        }
    }
    
    private IEnumerator TransitWithFade(string sceneName)
    {
        // Use SetTrigger instead of Play to start the fade animation
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.CompareTag("TransitionArea"))
        {
            Transition();
        }
    }
}
