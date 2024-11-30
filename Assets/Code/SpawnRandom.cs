using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    //[SerializeField] GameObject prefab;
    GameObject spawnedObject;          // Referencia al objeto generado actualmente
    Animator animator;                 // Referencia al Animator del objeto generado
    public Clickeable clickeableScript;
    // Esta variable ser� usada por otro script para saber cu�ntos bichos han sido destruidos
    public int bichosDestruidos = 0;

    #endregion Variables

    #region Funciones P�blicas
    public void Start() {
        // Instantiate(prefab, transform.position, Quaternion.identity);
    }
    public void OnButtonClick() {
        TrySpawnObject();
    }
    public void TrySpawnObject() {
        if (Random.value < spawnProbability) {
            int randomObjectIndex = Random.Range(0, spawnableObjects.Length);
            GameObject objectToSpawn = spawnableObjects[randomObjectIndex];

            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomSpawnIndex];

            spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
            animator = spawnedObject.GetComponent<Animator>();

            StartCoroutine(HandleObjectBehavior(spawnedObject));
        } else {
            StartCoroutine(RetrySpawnAfterDelay());
        }
    }

    #endregion Funciones P�blicas

    #region Funciones Privadas

    IEnumerator HandleObjectBehavior(GameObject obj) {
        yield return new WaitForSeconds(idleDuration);
        if (animator != null) {
            animator.SetBool("isWalking", true);
        }

        yield return new WaitForSeconds(walkDuration);
        if (animator != null) {
            animator.SetBool("isWalking", false);
        }
        obj.AddComponent<Clickeable>().Initialize(animator, deathDelay);

    }

    IEnumerator RetrySpawnAfterDelay() {
        yield return new WaitForSeconds(retryInterval);
        TrySpawnObject();
    }
    #endregion Funciones Privadas


}