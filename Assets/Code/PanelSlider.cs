using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelSlider : MonoBehaviour {
    //public GameObject[] panels;
    //private int currentPanelIndex = 0;

    //void Start() {
    //    ShowPanel(currentPanelIndex);
    //}

    //public void NextPanel() {
    //    currentPanelIndex = (currentPanelIndex + 1) % panels.Length;
    //    ShowPanel(currentPanelIndex);
    //}

    //public void PreviousPanel() {
    //    currentPanelIndex = (currentPanelIndex - 1 + panels.Length) % panels.Length;
    //    ShowPanel(currentPanelIndex);
    //}

    //private void ShowPanel(int index) {
    //    for (int i = 0; i < panels.Length; i++) {
    //        panels[i].SetActive(i == index);
    //    }
    //}

    public Transform[] panelPositions; // Array de posiciones de los paneles
    public float transitionSpeed = 2.0f; // Velocidad de transición de la cámara

    public Button leftButton;  // Botón para mover a la izquierda
    public Button rightButton; // Botón para mover a la derecha

    private int currentPanelIndex = 0; // Índice del panel actual
    private Camera mainCamera; // Referencia a la cámara principal

    private void Start() {
        // Asegúrate de que hay 3 paneles definidos
        if (panelPositions.Length != 3) {
            Debug.LogError("Debe haber exactamente 3 paneles configurados en el array panelPositions.");
            return;
        }

        mainCamera = Camera.main;
        // Iniciar la cámara en el primer panel
        mainCamera.transform.position = panelPositions[currentPanelIndex].position;

        // Asignar las funciones a los botones
        if (leftButton != null)
            leftButton.onClick.AddListener(MoveToLeftPanel);

        if (rightButton != null)
            rightButton.onClick.AddListener(MoveToRightPanel);
    }

    // Método para mover la cámara a la izquierda
    public void MoveToLeftPanel() {
        if (currentPanelIndex > 0) {
            currentPanelIndex--;
            StopAllCoroutines();
            StartCoroutine(MoveCamera(panelPositions[currentPanelIndex].position));
        }
    }

    // Método para mover la cámara a la derecha
    public void MoveToRightPanel() {
        if (currentPanelIndex < panelPositions.Length - 1) {
            currentPanelIndex++;
            StopAllCoroutines();
            StartCoroutine(MoveCamera(panelPositions[currentPanelIndex].position));
        }
    }

    // Corrutina para mover la cámara suavemente a la nueva posición
    private System.Collections.IEnumerator MoveCamera(Vector3 targetPosition) {
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f) {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        // Asegurar la posición final exacta
        mainCamera.transform.position = targetPosition;
    }
}



