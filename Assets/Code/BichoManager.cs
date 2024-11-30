using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BichoManager : MonoBehaviour {
    public GameObject[] m_bichos; // Array de bichos
    public Animator[] animators; // Array de Animators asociados a los bichos
    public float deathDelay = 1f; // Tiempo de retraso antes de desactivar el objeto
    private bool[] isDead; // Array para rastrear el estado de cada bicho

    [SerializeField] private string bichoTag = "Bicho"; // Tag para los bichos
    [SerializeField] private Canvas canvas; // Canvas específico donde están los bichos

    void Start() {
        // Asegúrate de que el array de estados coincida con el número de bichos
        isDead = new bool[m_bichos.Length];
    }

    void Update() {
        DetectarBichoClick(); // Detectar clics en los bichos
    }

    // Detecta si un bicho ha sido clickeado usando un Raycast
    private void DetectarBichoClick() {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                GameObject bicho = hit.collider.gameObject;

                // Verifica si el objeto tiene el tag correcto y pertenece al Canvas
                if (bicho.CompareTag(bichoTag) && bicho.transform.IsChildOf(canvas.transform)) {
                    int index = ObtenerIndiceBicho(bicho);

                    if (index >= 0 && !isDead[index]) {
                        Debug.Log($"Bicho {index} clickeado: {bicho.name}");
                        StartCoroutine(PlayDeathAnimation(index));
                    }
                }
            }
        }
    }

    // Obtiene el índice del bicho en el array
    private int ObtenerIndiceBicho(GameObject bicho) {
        for (int i = 0; i < m_bichos.Length; i++) {
            if (m_bichos[i] == bicho) {
                return i;
            }
        }
        return -1; // No encontrado
    }

    // Coroutine para jugar la animación de muerte y luego desactivar el bicho
    private IEnumerator PlayDeathAnimation(int index) {
        isDead[index] = true; // Marca el bicho como "muerto"

        // Ejecuta la animación de muerte
        if (animators[index] != null) {
            animators[index].SetTrigger("Death");
        }

        // Espera antes de desactivar el bicho
        yield return new WaitForSeconds(deathDelay);

        // Desactiva el bicho
        m_bichos[index].SetActive(false);

        // Lógica adicional (ejemplo: actualizar un contador global)
        SpawnRandom spawnRandom = FindObjectOfType<SpawnRandom>();
        if (spawnRandom != null) {
            spawnRandom.bichosDestruidos++;
        }

        Debug.Log($"Bicho {index} desactivado.");
    }
}
