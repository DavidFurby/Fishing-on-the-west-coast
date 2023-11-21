using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterDialog))]
[RequireComponent(typeof(CharacterExpression))]

public class CharacterController : CharacterStateMachine
{
    internal CharacterMovement movement;
    internal CharacterDialog dialog;
    internal CharacterExpression expression;
    [SerializeField] private PlayerController player;
    private Quaternion defaultRotation;

    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        dialog = GetComponent<CharacterDialog>();
        expression = GetComponent<CharacterExpression>();

        movement.Initialize(this);
        dialog.Initialize(this);
        expression.Initialize(this);
    }
    void OnEnable()
    {
        DialogManager.OnEndDialog += SetIdle;
    }
    void Start()
    {
        defaultRotation = transform.rotation;
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

        Vector3 direction = transform.position - player.transform.position;
        Quaternion playerRotation = Quaternion.LookRotation(direction);

        transform.rotation = playerRotation;
    }

    internal void SetDefaultRotation()
    {
        transform.rotation = defaultRotation;
    }

    private void SetIdle() {
        SetState(new CharacterIdle(this));
    }
}
