using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour {

    #region Variables 

    [SerializeField] GameObject[] spawnableObjects;      // Array de prefabs para elegir aleatoriamente
    [SerializeField] Transform[] spawnPoints;            // Array de puntos de aparici�n
    [Range(0f, 1f)]
    [SerializeField] float spawnProbability = 0.5f;      // Probabilidad de aparici�n (50%)
    [SerializeField] float idleDuration = 2.0f;          // Duraci�n en segundos en estado idle
    [SerializeField] float walkDuration = 3.0f;          // Duraci�n en segundos en estado de caminar
    [SerializeField] float retryInterval = 5.0f;         // Tiempo de espera para reintentar aparici�n si no sali� nada
    [SerializeField] float deathDelay = 2.0f;            // Tiempo de espera antes de iniciar la animaci�n de muerte despu�s de hacer clic

    GameObject spawnedObject;          // Referencia al objeto generado actualmente
    Animator animator;                 // Referencia al Animator del objeto generado

    #endregion Variables

    // accedo a mi funcion privada para spawnear objetos volviendolo este publica cuando le doy click a un objeto 
    #region Funciones Publicas
    public void OnButtonClick() {
        TrySpawnObject();
    }

    #endregion Funciones Publicas

    #region Funciones Privadas

    void TrySpawnObject() {
        // Verificar probabilidad de aparici�n
        if (Random.value < spawnProbability) {
            // Elegir un objeto aleatorio del array
            int randomObjectIndex = Random.Range(0, spawnableObjects.Length);
            GameObject objectToSpawn = spawnableObjects[randomObjectIndex];

            // Elegir un punto de aparici�n aleatorio
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomSpawnIndex];

            // Instanciar el objeto en el punto de aparici�n
            spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

            // Obtener el Animator del objeto generado
            animator = spawnedObject.GetComponent<Animator>();

            // Iniciar la secuencia de animaci�n y movimiento
            StartCoroutine(HandleObjectBehavior(spawnedObject));
        } else {
            Debug.Log("No se gener� un objeto esta vez. Reintentando en " + retryInterval + " segundos.");
            StartCoroutine(RetrySpawnAfterDelay());
        }
    }

    IEnumerator HandleObjectBehavior(GameObject obj) {
        // Estado idle al inicio
        yield return new WaitForSeconds(idleDuration);

        // Cambiar a la animaci�n de caminar
        if (animator != null) {
            animator.SetBool("isWalking", true); // Activar animaci�n de caminar
        }

        // Esperar en estado de caminar
        yield return new WaitForSeconds(walkDuration);

        // Volver a Idle despu�s de caminar
        if (animator != null) {
            animator.SetBool("isWalking", false);
        }

        // Configurar el objeto para que reaccione al clic para su animaci�n de muerte
        obj.AddComponent<Clickable>().Initialize(animator, deathDelay);
    }

    IEnumerator RetrySpawnAfterDelay() {
        // Esperar el intervalo de reintento
        yield return new WaitForSeconds(retryInterval);

        // Intentar nuevamente la aparici�n del objeto
        TrySpawnObject();
    }

    #endregion Funciones Privadas

    // Clase interna para manejar el clic y la animaci�n de muerte
    private class Clickable : MonoBehaviour {
        private Animator animator;
        private float deathDelay;

        public void Initialize(Animator animator, float delay) {
            this.animator = animator;
            deathDelay = delay;
        }

        private void OnMouseDown() {
            StartCoroutine(PlayDeathAnimation());
        }

        private IEnumerator PlayDeathAnimation() {
            // Esperar el tiempo de muerte
            yield return new WaitForSeconds(deathDelay);

            // Activar la animaci�n de muerte
            animator.SetTrigger("Death");

            // Esperar a que termine la animaci�n de muerte y luego destruir el objeto
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }
}


