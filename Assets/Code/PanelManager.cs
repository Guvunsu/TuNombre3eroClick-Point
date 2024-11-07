using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Para el TextMeshProUGUI
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;

public class PanelManager : MonoBehaviour
{
    #region Variables generales

    private bool paused = false;  // Para saber si el juego est� en pausa

    #endregion Variables generales

    #region Funciones Start y Update

    void Start()
    {
        // Inicializamos la escena
        ActivatePanel(setIndex);

        // Inicializamos los paneles (asegur�ndonos que el panel de victoria est� desactivado)
        panelVictoria.SetActive(false);
        pausePanel.SetActive(false);  // Inicialmente el panel de pausa est� oculto

        // Inicializa el tiempo restante con el tiempo l�mite
        tiempoRestante = tiempoLimite;
    }

    void Update()
    {
        // Activar o desactivar el men� de pausa cuando el jugador presiona "P"
        if (input.GetKeyDown(Keycode.P))
        {
            if (paused)
            {
                ResumeGame();
            }
            else if (paused)
            {
                PauseGame();
                PausarTimer();
            }
            else if (!paused)
            {

                ReanudarTimer();
            }
        }

        //// Verificamos si el nivel ha sido completado para mostrar el panel de victoria
        //if (NivelCompletado())
        //{
        //    MostrarPanelVictoria();
        //}

        // Si el timer est� activo y no est� pausado
        if (timerActivo && !isPaused)
        {
            // Decrementamos el tiempo usando unscaledDeltaTime para que no se vea afectado por la pausa
            tiempoRestante -= Time.unscaledDeltaTime;

            // Si el tiempo llega a 0, detenemos el timer
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                timerActivo = false;
                // Aqu� podr�as activar alguna l�gica cuando el timer llegue a 0, como finalizar el nivel
            }

            // Actualizamos el texto del timer en el UI
            ActualizarTextoTimer();
        }
    }
    #endregion Funciones Start y Update

    #region Variables para los paneles del gameplay

    public GameObject[] panels;             // Tus paneles de gameplay
    public Transform[] panelTargets;        // Los puntos de enfoque para Cinemachine en cada panel
    public CinemachineVirtualCamera vCam;   // C�mara virtual de Cinemachine
    private int setIndex = 0;               // �ndice actual del panel activo
    public GameObject pausePanel;           // Panel de pausa que se activar� y desactivar�

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

        // La c�mara sigue al nuevo panel
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

    // Activa el men� de pausa
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

    // Desactiva el men� de pausa
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
        Time.timeScale = 1f;  // Asegura que el tiempo del juego est� en modo normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Carga la escena del men� principal
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
        // Si est�s en el editor de Unity, detiene el modo de juego
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
    // M�todo que se llama cuando el nivel ha sido completado
    //public bool NivelCompletado()
    //{
    //    // Verificamos si el jugador cumpli� ambas condiciones: tiempo y destrucci�n de bichos
    //    return ganarEstrellasScript.estrellas > 0 || spawnRandomScript.bichosDestruidos == 8;
    //}

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

        // Verificamos las estrellas por la destrucci�n de bichos (de SpawnRandom)
        if (spawnRandomScript.bichosDestruidos == 8)
        {
            totalEstrellas++;
        }

        // Actualizamos el texto de estrellas
        textoEstrellas.text = "�Has ganado " + totalEstrellas + " estrella(s)!";

        // Aqu� podr�as agregar m�s l�gica, como animaciones o sonidos de victoria
    }

    // M�todo para cerrar el panel de victoria y reiniciar el estado del juego
    public void CerrarPanelVictoria()
    {
        // Ocultamos el panel de victoria
        panelVictoria.SetActive(false);

        // Reseteamos los valores de las estrellas y bichos destruidos
        ganarEstrellasScript.ResetearEstrellas();
        spawnRandomScript.bichosDestruidos = 0;  // Reseteamos los bichos destruidos
    }
    #endregion Funciones Panel Victoria

    #region Variables Panel de tiempo TMP

    [Header("Timer Settings")]
    public float tiempoLimite;

    [Header("UI References")]
    public TextMeshProUGUI textoTimer;  // Referencia al TextMeshProUGUI para mostrar el tiempo

    private float tiempoRestante;  // Tiempo restante en segundos
    private bool timerActivo = true;  // Controla si el timer est� activo o no
    private bool isPaused = false;   // Controla si el timer est� en pausa

    #endregion Variables Panel de tiempo TMP


    #region Funciones panel de tiempo TMP

    void ActualizarTextoTimer()
    {
        float minutos = Mathf.FloorToInt(tiempoRestante / 60);
        float segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    public void DetenerTimer()
    {
        timerActivo = false;
    }
    public void ReiniciarTimer()
    {
        tiempoRestante = tiempoLimite;
        timerActivo = true;
    }
    public void CambiarTiempoLimite(float nuevoTiempo)
    {
        tiempoLimite = nuevoTiempo;
        ReiniciarTimer();  // Reinicia el timer con el nuevo tiempo l�mite
    }
    public void PausarTimer()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Detiene el tiempo del juego (efecto global de pausa)
    }
    public void ReanudarTimer()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Restaura el tiempo del juego (efecto global de reanudaci�n)
    }

    #endregion Funciones panel de tiempo TMP

}



