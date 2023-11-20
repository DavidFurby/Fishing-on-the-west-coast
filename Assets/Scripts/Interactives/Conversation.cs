using System;
using UnityEngine;

public class Conversation : MonoBehaviour, IInteractive
{
    [SerializeField] private GameCharacters character;

    private enum GameCharacters
    {
        Player,
        Lotta,
        Lars,
        NeighborsWife,
        Fisherman,
        Dog,
        ShopKeeper,
    }
    public static event Action<string> StartConversation;
    [SerializeField] private PlayerController player;

    public void Interact()
    {
        StartConversation.Invoke(character.ToString());
        RotateTowardsPlayer(player.transform.position);
        PlayerController.Instance.SetState(new Conversing());
    }

    public void RotateTowardsPlayer(Vector3 playerPosition)
    {
        Vector3 direction = transform.position - playerPosition;
        Quaternion playerRotation = Quaternion.LookRotation(direction);
        transform.rotation = playerRotation;
    }
}
