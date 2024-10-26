using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour {

    #region Variables 

    public GameObject[] spawnableObjects;      // Array de prefabs para elegir aleatoriamente
    public Transform[] spawnPoints;            // Array de puntos de aparici�n
    [Range(0f, 1f)]
    [SerializeField] float spawnProbability = 0.5f;      // Probabilidad de aparici�n (50%)
    [SerializeField] float idleDuration = 3.0f;          // Duraci�n en segundos en estado idle
    [SerializeField] float walkDuration = 10.0f;          // Duraci�n en segundos en estado de caminar
    [SerializeField] float retryInterval = 15.0f;         // Tiempo de espera para reintentar aparici�n si no sali� nada

    [SerializeField] GameObject spawnedObject;          // Referencia al objeto generado actualmente

    #endregion Variables

    // Funcion que spawnea un objeto dentro de otro objeto que tiene como hijo un boton
    #region Funciones Publicas

    public void OnButtonClick() {
        TrySpawnObject();
    }

    #endregion Funciones Publicas 

    // spawneo objetos a base de una probabilidad aleatoria y su lugar 
    //luego una maquina de estado finito donde triggeara la animaciones correspodientes y su debido tiempo de duracion y su destruccion
    //se podra hacer de nuevo pasando su debido tiempo minimo ya establecido en las variables 
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

            // Iniciar la secuencia de animaci�n y movimiento
            StartCoroutine(HandleObjectBehavior(spawnedObject));
        } else {
            Debug.Log("No se gener� un objeto esta vez. Reintentando en " + retryInterval + " segundos.");
            StartCoroutine(RetrySpawnAfterDelay());
        }
    }
    IEnumerator HandleObjectBehavior(GameObject obj) {
        // Obtener el Animator y activar la animaci�n de idle
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null) {
            animator.SetTrigger("Idle"); // Trigger para la animaci�n de idle
        }

        // Esperar en estado idle
        yield return new WaitForSeconds(idleDuration);

        // Cambiar a la animaci�n de caminar
        if (animator != null) {
            animator.SetTrigger("Walk"); // Trigger para la animaci�n de caminar
        }

        // Esperar en estado de caminar
        yield return new WaitForSeconds(walkDuration);

        // Destruir el objeto despu�s de caminar
        Destroy(obj);
    }
    IEnumerator RetrySpawnAfterDelay() {
        // Esperar el intervalo de reintento
        yield return new WaitForSeconds(retryInterval);

        // Intentar nuevamente la aparici�n del objeto
        TrySpawnObject();
    }

    #endregion Funciones Privadas
}

