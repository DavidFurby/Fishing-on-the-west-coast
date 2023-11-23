using System.Collections;
using UnityEngine;

public class ShopInputManager : MonoBehaviour
{
    private Shop shop;
    private readonly float closeShopDelay = 1f;
    private readonly float scrollInputDelay = 0.5f;
    private bool canScroll = true;
    private bool canCloseShop = true;

    private Coroutine scrollInputDelayCoroutine;
    private Coroutine closeShopDelayCoroutine;
    private void Awake()
    {
        shop = FindObjectOfType<Shop>();
    }
    void OnEnable()
    {
        PlayerController.OnNavigateShop += HandleShoppingInput;
    }
    void OnDestroy()
    {
        PlayerController.OnNavigateShop -= HandleShoppingInput;
    }

    public void HandleShoppingInput()
    {
        if (shop.pauseShoppingControls) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && canScroll)
        {
            ScrollBetweenItems(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && canScroll)
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
        if (scrollInputDelayCoroutine != null)
        {
            StopCoroutine(scrollInputDelayCoroutine);
        }
        scrollInputDelayCoroutine = StartCoroutine(ScrollInputDelay());

        if (closeShopDelayCoroutine != null)
        {
            StopCoroutine(closeShopDelayCoroutine);
        }
        closeShopDelayCoroutine = StartCoroutine(CloseShopDelay());
    }
    private IEnumerator CloseShopDelay()
    {
        canCloseShop = false;
        yield return new WaitForSeconds(closeShopDelay);
        canCloseShop = true;
    }
    private IEnumerator ScrollInputDelay()
    {
        canScroll = false;
        yield return new WaitForSeconds(scrollInputDelay);
        canScroll = true;
    }
    private void CloseShop()
    {
        if (canCloseShop)
        {
            shop.dialogManager.EndDialog();
            PlayerController.Instance.SetState(new ExplorationIdle());
            shop.cameraController.SetState(new PlayerCamera());
            shop.SetFocusedShopItemIndex(0);
        }
    }
}
