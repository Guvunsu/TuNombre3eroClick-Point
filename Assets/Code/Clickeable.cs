using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clickeable : MonoBehaviour {
    public Animator animator;
    public float deathDelay;
    private bool isDead = false;  // Variable para verificar si el objeto ya está muerto

    [SerializeField] private GameObject[] m_bichos;
    private CanvasGroup canvasGroup; // CanvasGroup para controlar la interactividad
    private Button[] buttons; // Lista de botones que deben seguir siendo interactivos

    private PanelManager panelManager;

    // Método para inicializar el animador, el delay y el CanvasGroup
    public void Initialize(Animator animator, float delay) {
        this.animator = animator;
        this.deathDelay = delay;

        // Obtenemos el CanvasGroup del objeto
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();  // Si no tiene CanvasGroup, lo añadimos
        }
    }

    // Detecta clic sobre el objeto
    public void OnMouseDown() {

        // Solo ejecuta si el objeto no está muerto
        if (!isDead) {
            print("¡Funcionó! El objeto fue clickeado inicio la animacion de muerte");
            StartCoroutine(PlayDeathAnimation());
        }
    }

    // Coroutine para jugar la animación de muerte y luego desactivar el objeto
    public IEnumerator PlayDeathAnimation() {
        isDead = true;  //para no repetir la acción

        // Activamos la animación de muerte
        print("se actriva mi Trigger de muerte");
        animator.SetTrigger("Death");

        // Esperamos un tiempo antes de activar la animación de muerte
        print("Esperamos un tiempo antes de activar la animación de muerte");
        yield return new WaitForSeconds(deathDelay);

        //dejamos la animación de muerte que termine
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);  //para desactivar a los enemigos 
        // tratar de destruirlos para que funcione o de abajo 
        //Destroy(gameObject);

        SpawnRandom spawnRandom = FindObjectOfType<SpawnRandom>();

        if (spawnRandom != null) {
            // Incrementar el contador de bichos destruidos
            spawnRandom.bichosDestruidos++;

            // Verificamos si el jugador ha alcanzado el objetivo
            if (spawnRandom.bichosDestruidos >= 8) {
                // Llamar a la función que activa el panel de victoria
                FindObjectOfType<PanelManager>();
                if (panelManager != null) {
                    panelManager.MostrarPanelVictoria(panelManager);
                }
            }
        }

    }
}


