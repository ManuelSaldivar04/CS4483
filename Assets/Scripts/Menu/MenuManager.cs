using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; private set; }
    [SerializeField] private List<MenuEntry> menuList = new List<MenuEntry>();
    public Dictionary<string, MenuData> menus = new Dictionary<string, MenuData>();
    public TimeManager timeManager;
    public ShopMenu shopMenu;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var entry in menuList)
        {
            menus[entry.key] = entry.value;
        }
    }

    void Start()
    {
        foreach (MenuData menuData in menus.Values)
        {
            menuData.menu.SetActive(false);
        }    
    }

    public void OpenMenu(string menuName, bool stopTime)
    {
        if (menus.TryGetValue(menuName, out MenuData menuData))
        {
            CloseAllMenus();
            menuData.menu.SetActive(true);
            menuData.isOpen = true;

            if (stopTime)
            {
                timeManager.StopTime();
            }
        }
        else
        {
            Debug.LogWarning("Menu not found: " + menuName);
        }
    }
    public void OpenMenu(string menuName, bool stopTime, NPC shopNPC)
    {
        if (menus.TryGetValue(menuName, out MenuData menuData))
        {
            CloseAllMenus();
            menuData.menu.SetActive(true);
            menuData.isOpen = true;

            if(menuName == "shopmenu")
            {
                OpenShopMenu(shopNPC);
            }

            if (stopTime)
            {
                timeManager.StopTime();
            }
        }
        else
        {
            Debug.LogWarning("Menu not found: " + menuName);
        }
    }

    public void OpenMenu(string menuName, bool stopTime, TutorialNPC shopNPC)
    {
        if (menus.TryGetValue(menuName, out MenuData menuData))
        {
            CloseAllMenus();
            menuData.menu.SetActive(true);
            menuData.isOpen = true;

            if(menuName == "shopmenu")
            {
                OpenShopMenu(shopNPC);
            }

            if (stopTime)
            {
                timeManager.StopTime();
            }
        }
        else
        {
            Debug.LogWarning("Menu not found: " + menuName);
        }
    }

    public void OpenShopMenu(NPC shopNPC)
    {
        shopMenu.Initalize(shopNPC);
    }

    public void OpenShopMenu(TutorialNPC shopNPC)
    {
        shopMenu.Initalize(shopNPC);
    }

    public void CloseMenu(string menuName, bool resumeTime)
    {
        if (menus.ContainsKey(menuName))
        {
            menus[menuName].menu.SetActive(false);
            menus[menuName].isOpen = false;
            if (resumeTime)
            {
                timeManager.ResumeTime();
            }
        }
    }

    public void CloseAllMenus()
    {
        foreach (var menuData in menus.Values)
        {
            menuData.menu.SetActive(false);
            menuData.isOpen = false;
        }
        timeManager.ResumeTime();
    }

    public bool CheckOpen(string menuName)
    {
        if (menus.ContainsKey(menuName))
        {
            return menus[menuName].isOpen;
        }
        return false;
    }
}
