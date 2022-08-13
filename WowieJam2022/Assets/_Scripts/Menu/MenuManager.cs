using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public List<Canvas> menus;
    private Canvas currentMenu;
    private string CurrentMenuName { get { return currentMenu.name; } }

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            GetCurrentMenu();
        }
    }

    public Canvas GetMenuByName(string menuName)
    {
        foreach (Canvas canvas in menus)
        {
            if (canvas.name == menuName)
                return canvas;
        }
        Debug.LogError(transform.name + ": Failed to find Canvas under " + transform.parent.name + 
            " with name of " + menuName);
        return currentMenu;
    }

    public Canvas GetCurrentMenu()
    {
        foreach (Canvas canvas in menus)
        {
            if (canvas.gameObject.activeInHierarchy)
            {
                currentMenu = canvas;
                return currentMenu;
            }
        }
        Debug.LogWarning(transform.name + ": Failed to find any active Canvas under " + transform.parent.name + 
            ". Setting first inactive Canvas as current menu.");
        currentMenu = menus[0];
        return currentMenu;
    }

    public void LoadMenuByName(string menuName)
    {
        GetMenuByName(menuName).gameObject.SetActive(true);
    }

    public void UnloadMenuByName(string menuName)
    {
        GetMenuByName(menuName).gameObject.SetActive(false);
    }

    public void UnloadCurrentMenu()
    {
        UnloadMenuByName(CurrentMenuName);
    }

    
}
