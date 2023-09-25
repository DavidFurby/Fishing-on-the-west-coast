using System.Collections.Generic;
using UnityEngine;

public class ShopItemSpawner : MonoBehaviour
{
    private const string ItemsPath = "ScriptableObjects/Items";
    internal List<GameObject> shopItemPositions = new();
    internal Item emptySpot;

    public List<Item> ShopItems { get; private set; } = new List<Item>();

    private void Awake()
    {
        Item originalEmptySpot = Resources.Load<Item>(ItemsPath + "/Empty");
        emptySpot = originalEmptySpot.CloneItem();
        InitializeShopItems();
    }

    private void InitializeShopItems()
    {
        ShopItems = new List<Item>();
        string[] itemNames = { "/Baits/AdvanceBait", "/Hats/FancyHat", "/Rods/AdvanceRod", "/Baits/RareBait", "/Hats/PremiumHat", "/Rods/RareRod" };
        for (int i = 0; i < itemNames.Length; i++)
        {
            Item originalItem = Resources.Load<Item>(ItemsPath + itemNames[i]);
            ShopItems.Add(originalItem.CloneItem());
        }
    }

    public void InitializeShopItemPositions(Transform shopPositions)
    {
        if (shopPositions != null)
        {
            foreach (Transform childTransform in shopPositions)
            {
                if (childTransform != null)
                {
                    GameObject childPosition = childTransform.gameObject;
                    shopItemPositions.Add(childPosition);
                }
            }
        }
    }

    public void SpawnItems()
    {
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            if (i < ShopItems.Count && ShopItems[i] != null && !MainManager.Instance.Inventory.HasItem(ShopItems[i]))
            {
                SpawnShopItem(i);
            }
            else
            {
                SpawnEmptySpot(i);
            }
        }
    }

    private void SpawnObject(int index, Item item)
    {
        ShopItems[index] = item;
        GameObject newObject = Instantiate(item.model, shopItemPositions[index].transform.position, Quaternion.identity);
        newObject.transform.SetParent(shopItemPositions[index].transform, false);
        newObject.transform.position = shopItemPositions[index].transform.position;
    }

    private void SpawnEmptySpot(int index)
    {
        SpawnObject(index, emptySpot);
    }

    private void SpawnShopItem(int index)
    {
        SpawnObject(index, ShopItems[index]);
    }
}
