using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickeable : MonoBehaviour
{
    private Animator animator;
    private float deathDelay;

    public void Initialize(Animator animator, float delay)
    {
        this.animator = animator;
        this.deathDelay = delay;
    }

    private void OnMouseDown()
    {
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSeconds(deathDelay);
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);

        // Aquí incrementamos el contador de bichos destruidos en el SpawnRandom
        SpawnRandom spawnRandom = FindObjectOfType<SpawnRandom>();
        if (spawnRandom != null)
        {
            spawnRandom.bichosDestruidos++;  // Incrementar el contador de bichos destruidos
        }
    }
}
