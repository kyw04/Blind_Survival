using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menumanager : MonoBehaviour
{
    public static Menumanager Instace;


    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instace = this;    
    }

    public void OpenMenu(string _menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].MenuName == _menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].IsOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }
    public void OpenMenu(Menu _menu)
    {
        for(int i=0; i<menus.Length; i++)
        {
            if(menus[i].IsOpen)
            {
                CloseMenu(menus[i]);
            }
        }
        _menu.OpenMenu();

    }
    public void CloseMenu(Menu _menu)
    {

        _menu.CloseMenu();
    }


}
