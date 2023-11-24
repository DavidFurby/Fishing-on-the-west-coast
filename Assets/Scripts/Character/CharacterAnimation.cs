using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    protected CharacterManager manager;

    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void SetWalkAnimation(bool active)
    {
        if (animator != null)
        {
            animator.SetBool("walking", active);
        }
    }
}