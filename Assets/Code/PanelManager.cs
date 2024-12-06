using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Para el TextMeshProUGUI
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;

public class PanelManager : MonoBehaviour {
    #region Variables generales

    #region Variables paneles del gameplay

    public GameObject[] panels;             // Tus paneles de gameplay
    public Transform[] panelTargets;        // Los puntos de enfoque para Cinemachine en cada panel
    public CinemachineVirtualCamera vCam;   // Cámara virtual de Cinemachine
    private int setIndex = 0;               // Índice actual del panel activo
    public GameObject pausePanel;           // Panel de pausa que se activará y desactivará
    public int defaultPanelIndex = 0;       // esta variable la uso para despues de dejar el pausa panel y este en el indice 0

    #endregion Variables paneles del gameplay

    #region Variables pausa panel

    private bool paused = false;  // Para saber si el juego está en pausa
    public string menuSceneName = "Menu";
    public string tiendaSceneName = "Tienda";
    public string missionSelectSceneName = "MisionSelect";

    #endregion Variables pausa panel

    #region Variables Panel de Victoria

    SpawnRandom spawnRandomScript;

    public GameObject panelVictoria;
    public TextMeshProUGUI textoEstrellas;
    //private int totalEstrellas;


    public float tiempoMinimo;  // Tiempo mínimo para obtener una estrella (en segundos)
    public string resultado;  // Para mostrar el mensaje de ganaste o no la estrella

    public int estrellas;

    private int bichosDestruidos = 0;  // Contador de los enemigos destruidos
    private int totalBichos = 8;  // Número total de enemigos a destruir para ganar una estrella


    #endregion Variables Panel de Victoria

    #region Variables tiempo 

    [Header("Timer Settings")]
    public float tiempoLimite;

    [Header("UI References")]
    public TextMeshProUGUI textoTimer;  // Referencia al TextMeshProUGUI para mostrar el tiempo

    private float tiempoRestante;  // Tiempo restante en segundos
    private bool timerActivo = true;  // Controla si el timer está activo o no
    private bool isPaused = false;   // Controla si el timer está en pausa

    #endregion Variables tiempo

    #endregion Variables generales

    #region Funciones Start y Update

    void Start() {
        // Inicializamos la escena
        ActivatePanel(setIndex);

        // Inicializamos los paneles (asegurándonos que el panel de victoria está desactivado)
        panelVictoria.SetActive(false);
        pausePanel.SetActive(false);  // Inicialmente el panel de pausa está oculto

        // Inicializa el tiempo restante con el tiempo límite
        tiempoRestante = tiempoLimite;

        //Ganar EStrellas

        // Inicializamos el tiempo restante con el tiempo que empieza el nivel
        tiempoRestante = tiempoMinimo;
        estrellas = 0;  // Inicializamos las estrellas en 0
        resultado = "Tiempo mínimo no alcanzado";
        ResetearContadorBichos();
    }

    void Update() {
        // Activar o desactivar el menú de pausa cuando el jugador presiona "P"
        if (Input.GetKeyDown(KeyCode.P)) {
            if (paused) {
                ResumeGame();
                ReanudarTimer();
            } else {
                PauseGame();
                PausarTimer();
            }

        }

        ////// Verificamos si el nivel ha sido completado para mostrar el panel de victoria
        //if (NivelCompletado()) {
        //    MostrarPanelVictoria();
        //}

        // Si el timer está activo y no está pausado
        if (timerActivo && !isPaused) {
            // Decrementamos el tiempo usando unscaledDeltaTime para que no se vea afectado por la pausa
            tiempoRestante -= Time.unscaledDeltaTime;

            // Si el tiempo llega a 0, detenemos el timer
            if (tiempoRestante <= 0f) {
                tiempoRestante = 0f;
                timerActivo = false;
                //deberia de poner aqui el panel de loose 
            }

            // Actualizamos el texto del timer en el UI
            ActualizarTextoTimer();
        }


        // GanarEstrellas

        // Si el tiempo restante es mayor a 0, reducimos el tiempo
        if (tiempoRestante > 0) {
            tiempoRestante -= Time.deltaTime;
        } else {
            // Si pasamos el tiempo mínimo, asignamos una estrella
            if (tiempoRestante <= 0 && estrellas == 0) {
                // Aquí verificamos si se pasó el tiempo mínimo y se puede ganar la estrella
                if (tiempoRestante > -1)  // Si terminó dentro del tiempo
                {
                    estrellas = 1;
                    resultado = "¡Ganaste 1 estrella!";
                } else {
                    resultado = "Tiempo mínimo no alcanzado";
                }
            }
        }

        // Actualizamos el texto de las estrellas
        textoEstrellas.text = "Estrellas: " + estrellas;
    }
    #endregion Funciones Start y Update

    #region Funciones para los paneles del gameplay

    public void MovePanel(int direction) {
        int newIndex = (setIndex + direction + panels.Length) % panels.Length;
        ActivatePanel(newIndex);
    }

    private void ActivatePanel(int index) {
        foreach (GameObject panel in panels) {
            panel.SetActive(false);
        }
        if (index >= 0 && index < panels.Length) {
            panels[index].SetActive(true);
            setIndex = index;
        }
        //// Desactiva todos los paneles excepto el actual
        //for (int i = 0; i < panels.Length; i++)
        //{
        //    panels[i].SetActive(i == index);
        //}

        // La cámara sigue al nuevo panel
        vCam.Follow = panelTargets[setIndex];
        setIndex = index;
    }

    #endregion Funciones para los paneles del gameplay

    #region Funciones para el panel de pausa

    // Activa el menú de pausa
    public void PauseGame() {
        paused = true;
        pausePanel.SetActive(true);  // Activar el panel de pausa
        Time.timeScale = 0f;         // Detiene el tiempo del juego

        // Desactiva los paneles de gameplay, pero NO los borra
        foreach (GameObject panel in panels) {
            panel.SetActive(false);
        }
        //while (paused) {

        //    pausePanel.SetActive(false);

        //    panels[panels.Length - 1].SetActive(false);
        //}

    }

    // Desactiva el menú de pausa
    public void ResumeGame() {
        paused = false;
        pausePanel.SetActive(false);  // Desactivar el panel de pausa
        Time.timeScale = 1f;          // Reanuda el tiempo del juego

        //// Reactiva los paneles de gameplay
        //foreach (GameObject panel in panels) {
        //    // agrego esto para ver si funciona que se quede en el panel donde se quedo en pausa y se reanuda alli
        //    //int currentIndex = (int)panel.transform.position.x;
        //    //if (setIndex == currentIndex) {
        //    panel.SetActive(true);

        //    //}

        // siempre regresa al panel numero 0

        for (int i = 0; i < panels.Length; i++) {

            panels[i].SetActive(i == defaultPanelIndex);
        }
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

    #region Funciones Panel Victoria
    //Método que se llama cuando el nivel ha sido completado
    //public bool NivelCompletado() {
    //    // si umplimos ambas condiciones de tiempo y destrucción de bichos se activa panel de victoria
    //    MostrarPanelVictoria();
    //    return estrellas > 0 || spawnRandomScript.bichosDestruidos == 8;
    //}

    // Mostrar el panel de victoria y las estrellas obtenidas

    public void MostrarPanelVictoria(bool esVictoria) {
        // Activamos el panel de victoria


        // Reseteamos las estrellas
        //totalEstrellas = 0;

        // Si el jugador gana
        if (esVictoria) {
            
            if (panelVictoria = null) {
                bichosDestruidos = totalBichos;
                panelVictoria.SetActive(true);

            }

            if (tiempoRestante > 0) {
                resultado = "¡Ganaste 1 estrella!";
            }


        } else {
            resultado = "Perdiste: tiempo agotado";
        }

        // Actualizamos el texto de estrellas
        textoEstrellas.text = "¡Has ganado " + estrellas + " estrella(s)!";
    }

    // Método para cerrar el panel de victoria y reiniciar el estado del juego
    public void CerrarPanelVictoria() {
        // Ocultamos el panel de victoria
        panelVictoria.SetActive(false);

        // Reseteamos los valores de las estrellas y bichos destruidos
        //ResetearEstrellas();
        ResetearContadorBichos();  // Reseteamos los bichos destruidos
    }

    //public void ResetearEstrellas() {
    //    // Reseteamos el valor de las estrellas y el tiempo cuando el jugador reinicia o vuelve a intentar el nivel
    //    estrellas = 0;
    //    tiempoRestante = tiempoMinimo;
    //    resultado = "Tiempo mínimo no alcanzado";
    //    textoEstrellas.text = "Estrellas: " + estrellas;
    //}

    public void BichoDestruido(string bichoID) {
        // Incrementamos el contador de bichos destruidos cuando el bicho se destruye
        // Cada enemigo debe tener un identificador único o tag, en este caso se asume que los enemigos 
        // tienen un string o ID único que pasa a través del parámetro
        if (bichosDestruidos < totalBichos) {
            bichosDestruidos++;
            Debug.Log($"Bicho destruido: {bichoID}. Total destruidos: {bichosDestruidos}");

            // Si el jugador ha destruido todos los bichos ganas 1 estrella
            if (bichosDestruidos == totalBichos) {
                estrellas++;
                Debug.Log("¡Ganaste 1 estrella por destruir a todos los bichos!");
            }
        }
    }
    public void ResetearContadorBichos() {
        // Reseteamos el contador a 0 cuando se reinicia el juego o se reinicia el nivel
        bichosDestruidos = 0;
    }

    #endregion Funciones Panel Victoria

    #region Funciones texto de tiempo 

    void ActualizarTextoTimer() {
        float minutos = Mathf.FloorToInt(tiempoRestante / 60);
        float segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    public void DetenerTimer() {
        timerActivo = false;
    }
    public void ReiniciarTimer() {
        tiempoRestante = tiempoLimite;
        timerActivo = true;
    }
    public void CambiarTiempoLimite(float nuevoTiempo) {
        tiempoLimite = nuevoTiempo;
        ReiniciarTimer();  // Reinicia el timer con el nuevo tiempo límite
    }
    public void PausarTimer() {
        isPaused = true;
        Time.timeScale = 0f;  // Detiene el tiempo del juego (efecto global de pausa)
    }
    public void ReanudarTimer() {
        isPaused = false;
        Time.timeScale = 1f;  // Restaura el tiempo del juego (efecto global de reanudación)
    }

    #endregion Funciones texto de tiempo


}



