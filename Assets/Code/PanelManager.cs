using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Para el TextMeshProUGUI
using Cinemachine;

public class PanelManager : MonoBehaviour
{
    #region Variables generales

    private bool paused = false;  // Para saber si el juego está en pausa

    #endregion Variables generales

    #region Funciones Start y Update

    void Start()
    {
        // Inicializamos la escena
        ActivatePanel(setIndex);

        // Inicializamos los paneles (asegurándonos que el panel de victoria está desactivado)
        panelVictoria.SetActive(false);
        pausePanel.SetActive(false);  // Inicialmente el panel de pausa está oculto
    }

    void Update()
    {
        // Activar o desactivar el menú de pausa cuando el jugador presiona "P"
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
                ResumeGame();
            else
                PauseGame();
        }

        // Verificamos si el nivel ha sido completado para mostrar el panel de victoria
        if (NivelCompletado())
        {
            MostrarPanelVictoria();
        }
    }
    #endregion Funciones Start y Update

    #region Variables para los paneles del gameplay

    public GameObject[] panels;             // Tus paneles de gameplay
    public Transform[] panelTargets;        // Los puntos de enfoque para Cinemachine en cada panel
    public CinemachineVirtualCamera vCam;   // Cámara virtual de Cinemachine
    private int setIndex = 0;               // Índice actual del panel activo
    public GameObject pausePanel;           // Panel de pausa que se activará y desactivará

    #endregion Variables para los paneles del gameplay

    #region Funciones para los paneles del gameplay

    public void MovePanel(int direction)
    {
        int newIndex = (setIndex + direction + panels.Length) % panels.Length;
        ActivatePanel(newIndex);
    }

    private void ActivatePanel(int index)
    {
        // Desactiva todos los paneles excepto el actual
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        // La cámara sigue al nuevo panel
        vCam.Follow = panelTargets[setIndex];
        setIndex = index;
    }

    #endregion Funciones para los paneles del gameplay

    #region Variables para el pausa panel

    public string menuSceneName = "Menu";
    public string tiendaSceneName = "Tienda";
    public string missionSelectSceneName = "MisionSelect";

    #endregion Variables para el pausa panel

    #region Funciones para el panel de pausa

    // Activa el menú de pausa
    public void PauseGame()
    {
        paused = true;
        pausePanel.SetActive(true);  // Activar el panel de pausa
        Time.timeScale = 0f;         // Detiene el tiempo del juego

        // Desactiva los paneles de gameplay, pero NO los borra
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // Desactiva el menú de pausa
    public void ResumeGame()
    {
        paused = false;
        pausePanel.SetActive(false);  // Desactivar el panel de pausa
        Time.timeScale = 1f;          // Reanuda el tiempo del juego

        // Reactiva los paneles de gameplay
        foreach (GameObject panel in panels)
        {
            panel.SetActive(true);
        }
    }

    // Reinicia el nivel actual
    public void RetryGame()
    {
        Time.timeScale = 1f;  // Asegura que el tiempo del juego esté en modo normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Carga la escena del menú principal
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    // Carga la escena de la tienda
    public void GoToStore()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(tiendaSceneName);
    }

    // Carga la escena del selector de misiones
    public void GoToMissionSelector()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(missionSelectSceneName);
    }

    // Salir del juego
    public void GoToExit()
    {
        // Si estás en el editor de Unity, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    #endregion Funciones para el panel de pausa

    #region Variables para el Panel de Victoria

    GanarEstrellas ganarEstrellasScript;
    SpawnRandom spawnRandomScript;

    public GameObject panelVictoria;
    public TextMeshProUGUI textoEstrellas;
    private int totalEstrellas;

    #endregion Variables para el Panel de Victoria

    #region Funciones Panel Victoria
    // Método que se llama cuando el nivel ha sido completado
    public bool NivelCompletado()
    {
        // Verificamos si el jugador cumplió ambas condiciones: tiempo y destrucción de bichos
        return ganarEstrellasScript.estrellas > 0 || spawnRandomScript.bichosDestruidos == 8;
    }

    // Mostrar el panel de victoria y las estrellas obtenidas
    public void MostrarPanelVictoria()
    {
        // Activamos el panel de victoria
        panelVictoria.SetActive(true);

        // Reseteamos las estrellas
        totalEstrellas = 0;

        // Verificamos las estrellas por tiempo (de GanarEstrellas)
        if (ganarEstrellasScript.estrellas > 0)
        {
            totalEstrellas++;
        }

        // Verificamos las estrellas por la destrucción de bichos (de SpawnRandom)
        if (spawnRandomScript.bichosDestruidos == 8)
        {
            totalEstrellas++;
        }

        // Actualizamos el texto de estrellas
        textoEstrellas.text = "¡Has ganado " + totalEstrellas + " estrella(s)!";

        // Aquí podrías agregar más lógica, como animaciones o sonidos de victoria
    }

    // Método para cerrar el panel de victoria y reiniciar el estado del juego
    public void CerrarPanelVictoria()
    {
        // Ocultamos el panel de victoria
        panelVictoria.SetActive(false);

        // Reseteamos los valores de las estrellas y bichos destruidos
        ganarEstrellasScript.ResetearEstrellas();
        spawnRandomScript.bichosDestruidos = 0;  // Reseteamos los bichos destruidos
    }
    #endregion Funciones Panel Victoria

}



