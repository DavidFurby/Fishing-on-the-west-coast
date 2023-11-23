using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
[Serializable]
public class Character : ScriptableObject
{
    public new GameCharacters name;
    public string description;
}
public enum GameCharacters
{
    Player,
    Lotta,
    Lars,
    NeighborsWife,
    Fisherman,
    Dog,
    ShopKeeper,
}