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

    //하나의 메뉴만 띄우고 전부 끄는 역활
    public void OpenMenu(string _menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].MenuName == _menuName)
            {
                menus[i].OpenMenu();
            }
            else if (menus[i].IsOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    //하나의 메뉴만 띄움
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
