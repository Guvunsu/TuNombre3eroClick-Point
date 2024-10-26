using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public Camera cameraToMove; // Cámara a mover
    public Transform[] panelPositions; // Array con las posiciones de los tres paneles

    public float transitionSpeed = 5.0f; // Velocidad de transición de la cámara
    public Button leftButton; // Botón para mover la cámara a la izquierda
    public Button rightButton; // Botón para mover la cámara a la derecha

    private int currentPanelIndex = 0; // Índice actual de la posición del panel
    private bool isMoving = false; // Control de estado de movimiento

    private void Start() {
        // Comprobar que hay una cámara y tres paneles configurados
        if (cameraToMove == null || panelPositions.Length != 3) {
            Debug.LogError("Asigna la cámara y asegúrate de tener 3 paneles en el array.");
            return;
        }

        // Coloca la cámara en el panel central (índice 0) al inicio
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
        // Mueve la cámara solo si se encuentra en transición
        if (isMoving) {
            Vector3 targetPosition = panelPositions[currentPanelIndex].position;

            // Movimiento suave hacia la posición objetivo
            cameraToMove.transform.position = Vector3.Lerp(
                cameraToMove.transform.position,
                targetPosition,
                transitionSpeed * Time.deltaTime
            );

            // Verificar si la cámara llegó al objetivo
            if (Vector3.Distance(cameraToMove.transform.position, targetPosition) < 0.01f) {
                cameraToMove.transform.position = targetPosition; // Asegura que esté en la posición exacta
                isMoving = false; // Detenemos la transición
                UpdateButtonInteractivity(); // Actualizamos la interactividad de los botones
            }
        }
    }

    // Movimiento a la izquierda, verifica que no esté en el extremo izquierdo
    public void MoveToLeftPanel() {
        if (currentPanelIndex > 0 && !isMoving) {
            currentPanelIndex--;
            isMoving = true;
            Debug.Log("Moviendo al panel de la izquierda, índice actual: " + currentPanelIndex);
        }
    }

    // Movimiento a la derecha, verifica que no esté en el extremo derecho
    public void MoveToRightPanel() {
        if (currentPanelIndex < panelPositions.Length - 1 && !isMoving) {
            currentPanelIndex++;
            isMoving = true;
            Debug.Log("Moviendo al panel de la derecha, índice actual: " + currentPanelIndex);
        }
    }

    // Actualiza la interactividad de los botones para evitar movimientos fuera de los límites
    private void UpdateButtonInteractivity() {
        if (leftButton != null)
            leftButton.interactable = currentPanelIndex > 0;

        if (rightButton != null)
            rightButton.interactable = currentPanelIndex < panelPositions.Length - 1;
    }
}

