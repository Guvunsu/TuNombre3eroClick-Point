using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {


    private GameObject menuPanel;

    public MenuManager(GameObject panel) {
        menuPanel = panel;
    }

    public void ToggleMenu() {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
}


