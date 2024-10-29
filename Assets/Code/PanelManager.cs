using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelManager : MonoBehaviour {
    public GameObject[] panels;  // Arrastra tus paneles aqu� en el Inspector
    public Camera mainCamera;    // La c�mara principal que se mover� hacia el panel activo
    public float cameraSpeed = 5f;  // Velocidad de transici�n de la c�mara
    private int currentIndex = 0;   // �ndice actual del panel activo

    void Start() {
        // Asegurarse de que solo el panel inicial est� activo
        ActivatePanel(currentIndex);
    }

    // Llama a este m�todo con el �ndice de destino (1 a la derecha, -1 a la izquierda)
    public void MovePanel(int direction) {
        int newIndex = (currentIndex + direction + panels.Length) % panels.Length;
        ActivatePanel(newIndex);
    }

    private void ActivatePanel(int index) {
        for (int i = 0; i < panels.Length; i++) {
            panels[i].SetActive(i == index);  // Solo activa el panel seleccionado
        }

        currentIndex = index;
        StopAllCoroutines();  // Detiene cualquier transici�n en curso
        StartCoroutine(MoveCameraToPanel(panels[currentIndex].transform.position));
    }

    private System.Collections.IEnumerator MoveCameraToPanel(Vector3 targetPosition) {
        Vector3 startPosition = mainCamera.transform.position;

        // Establece Y y Z de targetPosition como la posici�n de inicio de la c�mara
        targetPosition.y = startPosition.y;
        targetPosition.z = startPosition.z;

        float elapsedTime = 0f;
        while (elapsedTime < 1f) {
            // Lerp solo en el eje X para mover lateralmente la c�mara
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * cameraSpeed;
            yield return null;
        }

        // Asegura que la c�mara termine exactamente en la posici�n X objetivo
        mainCamera.transform.position = targetPosition;
    }
}


