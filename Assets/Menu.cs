using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string MenuName;
    public bool IsOpen;

    public void OpenMenu()
    {
        IsOpen = true;
        gameObject.SetActive(true);
    }
    public void CloseMenu()
    {
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
