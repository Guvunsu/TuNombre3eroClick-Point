//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CameraController : MonoBehaviour {

//    #region Variables
//    public Camera cameraToMove;
//    public Transform[] panelPositions;

//    public float transitionSpeed;
//    public Button leftButton;
//    public Button rightButton;

//    private int currentPanelIndex = 0; // �ndice actual de la posici�n del panel
//    private bool isMoving = false; // Control de estado de movimiento
//    #endregion Variables 

//    // me ayudan a la interaccion de mis botones de mis paneles,
//    // la dettecion de mis clicks,
//    // y a la posicion de mi camara 
//    #region Funciones Privadas

//    void Start() {

//        cameraToMove = GetComponent<Camera>();

//        // Comprobar que hay una c�mara y tres paneles configurados
//        if (cameraToMove == null || panelPositions.Length != 3) {
//            return;
//        }

//        // Coloca la c�mara en el panel central (�ndice 0) al inicio
//        currentPanelIndex = 0;
//        cameraToMove.transform.position = panelPositions[currentPanelIndex].position;

//        // Asignar las funciones de movimiento a los botones
//        if (leftButton != null)
//            leftButton.onClick.AddListener(MoveToLeftPanel);

//        if (rightButton != null)
//            rightButton.onClick.AddListener(MoveToRightPanel);
//        UpdateButtonInteractivity();
//    }

//    void Update() {
//        // Mueve la c�mara solo si se encuentra en transici�n
//        if (isMoving) {
//            Vector3 targetPosition = panelPositions[currentPanelIndex].position;

//            // Movimiento suave hacia la posici�n objetivo
//            cameraToMove.transform.position = Vector3.Lerp(cameraToMove.transform.position, targetPosition, transitionSpeed * Time.deltaTime);

//            // Verificar si la c�mara lleg� al objetivo
//            // Asegura que est� en la posici�n exacta
//            // Detenemos la transici�n
//            // Actualizamos la interactividad de los botones
//            if (Vector3.Distance(cameraToMove.transform.position, targetPosition) < 0.01f) {
//                cameraToMove.transform.position = targetPosition;
//                isMoving = false;
//                UpdateButtonInteractivity();
//            }
//        }
//    }

//    // Supuestamente me ayudara a la interaccion de mis botones y no sobrepase los limites de mis paneles 
//    void UpdateButtonInteractivity() {
//        if (leftButton != null)
//            leftButton.interactable = currentPanelIndex > 0;

//        if (rightButton != null)
//            rightButton.interactable = currentPanelIndex < panelPositions.Length - 1;
//    }

//    #endregion Funciones Privadas

//    // se encnuentra el movimiento de mis paneles
//    #region Funciones Publicas 

//    #region Movimiento De Paneles

//    public void MoveToLeftPanel() {
//        if (currentPanelIndex > 0 && !isMoving) {
//            currentPanelIndex--;
//            isMoving = true;
//            Debug.Log("Moviendo al panel de la izquierda, �ndice actual: " + currentPanelIndex);
//        }
//    }

//    public void MoveToRightPanel() {
//        if (currentPanelIndex < panelPositions.Length - 1 && !isMoving) {
//            currentPanelIndex++;
//            isMoving = true;
//            Debug.Log("Moviendo al panel de la derecha, �ndice actual: " + currentPanelIndex);
//        }
//    }


//    #endregion Movimiento De Paneles

//    #endregion Funciones Publicas

//}

