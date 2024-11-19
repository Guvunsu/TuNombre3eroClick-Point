using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickeable : MonoBehaviour {
    public Animator animator;
    public float deathDelay;

    public void Initialize(Animator animator, float delay) {
        this.animator = animator;
        this.deathDelay = delay;
    }

    public void OnMouseDown() {
        print("funciono");
        StartCoroutine(PlayDeathAnimation());
    }

    public IEnumerator PlayDeathAnimation() {
        yield return new WaitForSeconds(deathDelay);
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);

        // Aquí incrementamos el contador de bichos destruidos en el SpawnRandom
        SpawnRandom spawnRandom = FindObjectOfType<SpawnRandom>();
        if (spawnRandom != null) {
            spawnRandom.bichosDestruidos++;  // Incrementar el contador de bichos destruidos
        }
    }
}
