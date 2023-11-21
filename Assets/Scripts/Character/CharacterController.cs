using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterDialog))]
public class CharacterController : CharacterStateMachine
{
    [HideInInspector] public CharacterMovement movement;
    [HideInInspector] public CharacterDialog dialog;
    [SerializeField] private PlayerController player;
    private Quaternion defaultRotation;

    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        dialog = GetComponent<CharacterDialog>();

        movement.Initialize(this);
        dialog.Initialize(this);
    }
    void OnEnable()
    {
        DialogManager.OnEndDialog += () => SetState(new CharacterIdle(this));
    }
    void Start()
    {
        defaultRotation = transform.rotation;
    }

   void OnDestroy()
    {
        DialogManager.OnEndDialog -= () => SetState(new CharacterIdle(this));
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
}
