using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlider : MonoBehaviour {
    public GameObject[] panels;
    private int currentPanelIndex = 0;

    void Start() {
        ShowPanel(currentPanelIndex);
    }

    public void NextPanel() {
        currentPanelIndex = (currentPanelIndex + 1) % panels.Length;
        ShowPanel(currentPanelIndex);
    }

    public void PreviousPanel() {
        currentPanelIndex = (currentPanelIndex - 1 + panels.Length) % panels.Length;
        ShowPanel(currentPanelIndex);
    }

    private void ShowPanel(int index) {
        for (int i = 0; i < panels.Length; i++) {
            panels[i].SetActive(i == index);
        }
    }
}
