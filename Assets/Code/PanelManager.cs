using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour {

    #region Variables para los paneles del gameplay

    public GameObject[] panels;             // Tus paneles
    public Transform[] panelTargets;        // Los puntos de enfoque para Cinemachine en cada panel
    public CinemachineVirtualCamera vCam;   // Cámara virtual de Cinemachine
    private int setIndex = 0;           // Índice actual del panel activo
    public GameObject pausePanel;  // Panel de pausa que se activará y desactivará

    #endregion Variables para los paneles del gameplay

    #region Funciones para los paneles del gameplay

    void Start() {
        // Inicia enfocando en el primer panel
        ActivatePanel(setIndex);
    }
    public void MovePanel(int direction) {
        int newIndex = (setIndex + direction + panels.Length) % panels.Length;
        ActivatePanel(newIndex);
    }

    private void ActivatePanel(int index) {
        // Desactiva todos los paneles excepto el actual
        for (int i = 0; i < panels.Length; i++) {
            panels[i].SetActive(i == index);
        }

        // la camara esta en el nuevo panel siguiendolo
        vCam.Follow = panelTargets[setIndex];
        setIndex = index;
    }

    #endregion Funciones para los paneles del gameplay

    #region Variables para el pausa panel

    public string menuSceneName = "Menu";
    public string tiendaSceneName = "Tienda";
    public string missionSelectSceneName = "MisionSelect";

    private bool paused = false;

    #endregion Variables para el pausa panel

    #region Funciones para el panel de pausa
    void Update() {
        // Activar o desactivar el menú de pausa cuando el jugador presiona "Esc"
        if (Input.GetKeyDown(KeyCode.P)) {
            if (paused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Activa el menú de pausa
    public void PauseGame() {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;  // Detiene el tiempo del juego
        paused = true;
    }

    // Desactiva el menú de pausa
    public void ResumeGame() {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;  // Restaura el tiempo del juego
        paused = false;
    }

    // Reinicia el nivel actual
    public void RetryGame() {
        Time.timeScale = 1f;  // Asegura que el tiempo del juego esté en modo normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Carga la escena del menú principal
    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    // Carga la escena de la tienda
    public void GoToStore() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(tiendaSceneName);
    }

    // Carga la escena del selector de misiones
    public void GoToMissionSelector() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(missionSelectSceneName);
    }
    // Salir del juego 
    public void GoToExit() {

        // Si estás en el editor de Unity, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    #endregion Funciones para el panel de pausa

}




