
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitArea : MonoBehaviour, IInteractible
{
    public SceneAsset toScene;
    public Animator animator;

    public void Interact()
    {
        Transition();
    }
    public void Transition()
    {
        if (toScene != null)
        {
            StartCoroutine(TransitWithFade(toScene.name));
        }
    }
    private IEnumerator TransitWithFade(string sceneName)
    {
        animator.Play("Fade");
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
