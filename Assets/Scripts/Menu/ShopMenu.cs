using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    public MenuManager menuManager;
    public void OpenShopMenu()
    {
        menuManager.OpenMenu("shopmenu", true);
    }

    public void CloseShopMenu()
    {
        menuManager.CloseMenu("shopmenu", true
        );
    }

    public void BuyItem()
    {
        
    }

}
