using UnityEngine;

public class ShopInputManager : MonoBehaviour
{
    private Shop shop;

    private void Awake()
    {
        shop = FindObjectOfType<Shop>();
    }
    void OnEnable()
    {
        ExplorationController.NavigateShop += HandleShoppingInput;
    }
    void OnDestroy()
    {
        ExplorationController.NavigateShop -= HandleShoppingInput;
    }

    public void HandleShoppingInput()
    {
        if (shop.pauseShoppingControls) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ScrollBetweenItems(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ScrollBetweenItems(true);
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            CloseShop();
        }
    }


    private void ScrollBetweenItems(bool forward)
    {
        int newIndex = (shop.focusedShopItemIndex + (forward ? 1 : -1) + shop.itemSpawner.ShopItems.Count) % shop.itemSpawner.ShopItems.Count;
        shop.SetFocusedShopItemIndex(newIndex);
        shop.FocusItem();
        shop.UpdateShopDialog();
    }

    private void CloseShop()
    {
        shop.dialogManager.EndDialog();
        shop.playerController.SetState(new ExplorationIdle(shop.playerController));
        shop.cameraController.SetState(new PlayerCamera(shop.cameraController));
        shop.SetFocusedShopItemIndex(0);
    }
}
