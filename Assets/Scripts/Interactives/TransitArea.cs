
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitArea : MonoBehaviour, IInteractive
{
    private readonly SceneAsset toScene;
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