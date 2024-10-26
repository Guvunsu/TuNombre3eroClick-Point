using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public Camera cameraToMove; // C�mara a mover
    public Transform[] panelPositions; // Array con las posiciones de los tres paneles

    public float transitionSpeed = 5.0f; // Velocidad de transici�n de la c�mara
    public Button leftButton; // Bot�n para mover la c�mara a la izquierda
    public Button rightButton; // Bot�n para mover la c�mara a la derecha

    private int currentPanelIndex = 0; // �ndice actual de la posici�n del panel
    private bool isMoving = false; // Control de estado de movimiento

    private void Start() {
        // Comprobar que hay una c�mara y tres paneles configurados
        if (cameraToMove == null || panelPositions.Length != 3) {
            Debug.LogError("Asigna la c�mara y aseg�rate de tener 3 paneles en el array.");
            return;
        }

        // Coloca la c�mara en el panel central (�ndice 0) al inicio
        currentPanelIndex = 0;
        cameraToMove.transform.position = panelPositions[currentPanelIndex].position;

        // Asignar las funciones de movimiento a los botones
        if (leftButton != null)
            leftButton.onClick.AddListener(MoveToLeftPanel);

        if (rightButton != null)
            rightButton.onClick.AddListener(MoveToRightPanel);

        UpdateButtonInteractivity();
    }

    private void Update() {
        // Mueve la c�mara solo si se encuentra en transici�n
        if (isMoving) {
            Vector3 targetPosition = panelPositions[currentPanelIndex].position;

            // Movimiento suave hacia la posici�n objetivo
            cameraToMove.transform.position = Vector3.Lerp(
                cameraToMove.transform.position,
                targetPosition,
                transitionSpeed * Time.deltaTime
            );

            // Verificar si la c�mara lleg� al objetivo
            if (Vector3.Distance(cameraToMove.transform.position, targetPosition) < 0.01f) {
                cameraToMove.transform.position = targetPosition; // Asegura que est� en la posici�n exacta
                isMoving = false; // Detenemos la transici�n
                UpdateButtonInteractivity(); // Actualizamos la interactividad de los botones
            }
        }
    }

    // Movimiento a la izquierda, verifica que no est� en el extremo izquierdo
    public void MoveToLeftPanel() {
        if (currentPanelIndex > 0 && !isMoving) {
            currentPanelIndex--;
            isMoving = true;
            Debug.Log("Moviendo al panel de la izquierda, �ndice actual: " + currentPanelIndex);
        }
    }

    // Movimiento a la derecha, verifica que no est� en el extremo derecho
    public void MoveToRightPanel() {
        if (currentPanelIndex < panelPositions.Length - 1 && !isMoving) {
            currentPanelIndex++;
            isMoving = true;
            Debug.Log("Moviendo al panel de la derecha, �ndice actual: " + currentPanelIndex);
        }
    }

    // Actualiza la interactividad de los botones para evitar movimientos fuera de los l�mites
    private void UpdateButtonInteractivity() {
        if (leftButton != null)
            leftButton.interactable = currentPanelIndex > 0;

        if (rightButton != null)
            rightButton.interactable = currentPanelIndex < panelPositions.Length - 1;
    }
}

