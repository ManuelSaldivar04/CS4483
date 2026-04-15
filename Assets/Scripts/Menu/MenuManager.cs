using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<MenuEntry> menuList = new List<MenuEntry>();
    public Dictionary<string, MenuData> menus = new Dictionary<string, MenuData>();
    public TimeManager timeManager;

    void Awake()
    {
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

    public void CheckOpen(string menuName)
    {
        if (menus.ContainsKey(menuName))
        {
            menus[menuName].isOpen = true;
        }
    }
}
