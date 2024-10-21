using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageClickManager : MonoBehaviour {


    public Button imageButton;
    public Animator animator;
    public GameObject spawnObject;
    public float idleTime = 3f;
    public float moveSpeed = 2f;
    private bool isSpawned = false;

    void Start() {
        imageButton.onClick.AddListener(OnImageClick);
        spawnObject.SetActive(false);
    }

    void OnImageClick() {
        animator.SetTrigger("Activate");
        float randomChance = Random.value; // Valor aleatorio entre 0 y 1

        if (randomChance > 0.5f) // 50% de probabilidad
        {
            spawnObject.SetActive(true);
            isSpawned = true;
            Invoke("MoveAndDisappear", idleTime);
        }
    }

    void MoveAndDisappear() {
        if (isSpawned) {
            StartCoroutine(MoveAndDestroy(spawnObject));
        }
    }

    IEnumerator MoveAndDestroy(GameObject obj) {
        float timer = 0f;
        Vector3 startPosition = obj.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 3f; // Mover hacia arriba

        while (timer < 1f) {
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, timer);
            timer += Time.deltaTime * moveSpeed;
            yield return null;
        }

        Destroy(obj);
    }
}


