using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickableSpawner : MonoBehaviour {
    
    public Animator animator;  // Referencia al Animator
    public string animationTrigger = "Play"; // Nombre del Trigger en el Animator
    public float animationDuration; // Duraci�n antes de detener la animaci�n


    private bool isAnimating = false;

    void Start() {
        // Desactivamos la animaci�n inicialmente
        if (animator == null) {
            animator = GetComponent<Animator>();
        }

        if (animator != null) {
            animator.enabled = false;
        }
    }

    void OnMouseDown() {
        if (isAnimating) {

            return; // Evitar clics adicionales mientras ya est� animando
        }

        // Activar la animaci�n
        if (animator != null) {
            animator.enabled = true;
            animator.SetTrigger(animationTrigger);
        }

        isAnimating = true;
        StartCoroutine(HandleAnimation());
    }

    private IEnumerator HandleAnimation() {
        // Esperar la duraci�n configurada
        yield return new WaitForSeconds(animationDuration);

        // Detener la animaci�n
        if (animator != null) {
            animator.enabled = false;
        }

        isAnimating = false;

    }
}


