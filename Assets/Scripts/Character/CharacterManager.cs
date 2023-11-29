using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterDialog))]
[RequireComponent(typeof(CharacterExpression))]
[RequireComponent(typeof(CharacterAnimation))]
[RequireComponent(typeof(CharacterHandlers))]
[RequireComponent(typeof(CharacterBlink))]
public class CharacterManager : CharacterStateMachine
{
    public Character character;
    internal CharacterMovement movement;
    internal CharacterDialog dialog;
    internal CharacterExpression expression;
    internal CharacterAnimation animations;
    internal CharacterBlink blink;
    private PlayerManager player;
    private CharacterHandlers handlers;
    private Quaternion defaultRotation;


    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        dialog = GetComponent<CharacterDialog>();
        expression = GetComponent<CharacterExpression>();
        animations = GetComponent<CharacterAnimation>();
        handlers = GetComponent<CharacterHandlers>();
        blink = GetComponent<CharacterBlink>();
        movement.Initialize(this);
        dialog.Initialize(this);
        expression.Initialize(this);
        animations.Initialize(this);
        handlers.Initialize(this);
        blink.Initialize(this);
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
