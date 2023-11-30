using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterDialog))]
[RequireComponent(typeof(CharacterHandlers))]
[RequireComponent(typeof(CharacterAnimationController))]
public class CharacterManager : CharacterStateMachine
{
    public Character character;
    internal CharacterMovement movement;
    internal CharacterDialog dialog;
    private PlayerManager player;
    internal CharacterAnimationController animationController;
    private CharacterHandlers handlers;
    private Quaternion defaultRotation;
    private 


    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        dialog = GetComponent<CharacterDialog>();
        handlers = GetComponent<CharacterHandlers>();
        animationController = GetComponent<CharacterAnimationController>();
        movement.Initialize(this);
        dialog.Initialize(this);
        handlers.Initialize(this);
        animationController.Initialize(this);
    }
    void OnEnable()
    {
        DialogManager.OnEndDialog += SetIdle;
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();
        defaultRotation = transform.rotation;
        SetIdle();
    }

    void OnDestroy()
    {
        DialogManager.OnEndDialog -= SetIdle;
    }

    internal void RotateTowardsPlayer()
    {
        if (player == null)
        {
            Debug.LogError("PlayerController is not assigned");
            return;
        }

        Vector3 direction = player.transform.position - transform.position;
        Quaternion playerRotation = Quaternion.LookRotation(direction);

        transform.rotation = playerRotation;
    }

    internal void SetDefaultRotation()
    {
        transform.rotation = defaultRotation;
    }

    private void SetIdle()
    {
        SetState(new CharacterIdle(this));
    }
}
