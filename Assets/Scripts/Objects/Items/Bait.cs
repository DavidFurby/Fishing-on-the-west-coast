using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Bait", menuName = "ScriptableObjects/Bait", order = 1)]
public class Bait : Item
{
    [SerializeField] public int level;

    public Bait() : base()
    {
        itemTag = ItemTag.Bait;
    }

    public void AddBaitToInstance()
    {
        MainManager.Instance.game.FoundBaits =
            MainManager.Instance.game.FoundBaits.Append(this).ToList();
    }

    public static Bait SetBait(BaitData baitData)
    {
        Bait bait = CreateInstance<Bait>();
        bait.id = baitData.id;
        bait.name = baitData.name;
        bait.description = baitData.description;
        bait.price = baitData.price;
        return bait;
    }
}
