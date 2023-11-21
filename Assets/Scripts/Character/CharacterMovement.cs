using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    internal CharacterController controller;

    public void Initialize(CharacterController controller)
    {
        this.controller = controller;
    }
}