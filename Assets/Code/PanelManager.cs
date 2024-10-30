using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PanelManager : MonoBehaviour {
    public GameObject[] panels;             // Tus paneles
    public Transform[] panelTargets;        // Los puntos de enfoque para Cinemachine en cada panel
    public CinemachineVirtualCamera vCam;   // Cámara virtual de Cinemachine
    private int index = 0;           // Índice actual del panel activo

    void Start() {
        // Inicia enfocando en el primer panel
        ActivatePanel(index);
    }

    public void MovePanel(int direction) {
        int newIndex = (index + direction + panels.Length) % panels.Length;
        ActivatePanel(newIndex);
    }

    private void ActivatePanel(int Newindex) {
        // Desactiva todos los paneles excepto el actual
        for (int i = 0; i < panels.Length; i++) {
            panels[i].SetActive(i == Newindex);
        }

        // Cambia el objetivo de la cámara virtual al nuevo panel
        vCam.Follow = panelTargets[Newindex];

        index = Newindex;
    }
}




