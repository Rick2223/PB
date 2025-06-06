using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;

    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject menu;

    public bool isMenuOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenuOpen) //set Escape to another key if running in the engine
        {
            OpenMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        uiCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        isMenuOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        saveMenu.SetActive(false);
        settingsMenu.SetActive(false);
        menu.SetActive(true);
        uiCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        isMenuOpen = false;
        if (InventorySystem.Instance.isOpen == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
