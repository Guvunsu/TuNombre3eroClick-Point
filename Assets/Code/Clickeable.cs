using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//public class Clickeable : MonoBehaviour {
//    public Animator animator;
//    public float deathDelay;

//    public void Initialize(Animator animator, float delay) {
//        this.animator = animator;
//        this.deathDelay = delay;
//    }

//    public void OnMouseDown() {
//        print("funciono");
//        StartCoroutine(PlayDeathAnimation());
//    }

//    public IEnumerator PlayDeathAnimation() {
//        yield return new WaitForSeconds(deathDelay);
//        animator.SetTrigger("Death");
//        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
//        Destroy(gameObject);

//        // Aquí incrementamos el contador de bichos destruidos en el SpawnRandom
//        SpawnRandom spawnRandom = FindObjectOfType<SpawnRandom>();
//        if (spawnRandom != null) {
//            spawnRandom.bichosDestruidos++;  // Incrementar el contador de bichos destruidos
//        }
//    }
//}


public class Clickeable : MonoBehaviour {
    public Animator animator;
    public float deathDelay;
    private bool isDead = false;  // Variable para verificar si el objeto ya está muerto

    [SerializeField] private GameObject[] m_bichos;
    private CanvasGroup canvasGroup; // CanvasGroup para controlar la interactividad
    private Button[] buttons; // Lista de botones que deben seguir siendo interactivos

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

    // Detecta cuando el jugador hace clic sobre el objeto
    public void OnMouseDown() {

        // Solo ejecuta si el objeto no está muerto
        if (!isDead) {
            print("¡Funcionó! El objeto fue clickeado inicio la animacion de muerte");
            StartCoroutine(PlayDeathAnimation());
        }
    }

    // Coroutine para jugar la animación de muerte y luego desactivar el objeto
    public IEnumerator PlayDeathAnimation() {
        isDead = true;  // Marcamos que el objeto está "muerto" para no repetir la acción

        // Desactivar la interacción con el objeto (evitar que interfiera con los botones)}
        // desactive esto ya que mis bichos no ocuapn botones
        //if (canvasGroup != null) {
        //    canvasGroup.blocksRaycasts = false; // Desactiva la interactividad con el objeto
        //}

        // Activamos la animación de muerte
        print("se actriva mi Trigger de muerte");
        animator.SetTrigger("Death");

        // Esperamos el tiempo del retraso antes de activar la animación de muerte
        print("me espero tantos segundos para mi delay de muerte");
        yield return new WaitForSeconds(deathDelay);

        // Esperamos que la animación de muerte termine
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(2).length);

        // Desactivamos el objeto
        gameObject.SetActive(false);

        // Aquí incrementamos el contador de bichos destruidos en el SpawnRandom
        SpawnRandom spawnRandom = FindObjectOfType<SpawnRandom>();
        if (spawnRandom != null) {
            spawnRandom.bichosDestruidos++;  // Incrementar el contador de bichos destruidos
        }

    }
}


