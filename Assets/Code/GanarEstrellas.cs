using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GanarEstrellas : MonoBehaviour
{
    public float tiempoMinimo = 180f;  // Tiempo m�nimo para obtener una estrella (en segundos)
    public TextMeshProUGUI textoEstrellas;  // Texto que muestra las estrellas
    public string resultado;  // Para mostrar el mensaje de ganaste o no la estrella

    private float tiempoRestante;
    public int estrellas;

    private int bichosDestruidos = 0;  // Contador de los enemigos destruidos
    private int totalBichos = 8;  // N�mero total de enemigos a destruir para ganar una estrella

    private void Start()
    {
        // Inicializamos el tiempo restante con el tiempo que empieza el nivel
        tiempoRestante = tiempoMinimo;
        estrellas = 0;  // Inicializamos las estrellas en 0
        resultado = "Tiempo m�nimo no alcanzado";
        ResetearContadorBichos();

    }

    private void Update()
    {
        // Si el tiempo restante es mayor a 0, reducimos el tiempo
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
        }
        else
        {
            // Si pasamos el tiempo m�nimo, asignamos una estrella
            if (tiempoRestante <= 0 && estrellas == 0)
            {
                // Aqu� verificamos si se pas� el tiempo m�nimo y se puede ganar la estrella
                if (tiempoRestante > -1)  // Si termin� dentro del tiempo
                {
                    estrellas = 1;
                    resultado = "�Ganaste 1 estrella!";
                }
                else
                {
                    resultado = "Tiempo m�nimo no alcanzado";
                }
            }
        }

        // Actualizamos el texto de las estrellas
        textoEstrellas.text = "Estrellas: " + estrellas;
    }

    public void ResetearEstrellas()
    {
        // Reseteamos el valor de las estrellas y el tiempo cuando el jugador reinicia o vuelve a intentar el nivel
        estrellas = 0;
        tiempoRestante = tiempoMinimo;
        resultado = "Tiempo m�nimo no alcanzado";
        textoEstrellas.text = "Estrellas: " + estrellas;
    }
    public void BichoDestruido(string bichoID)
    {
        // Incrementamos el contador de bichos destruidos cuando el bicho se destruye
        // Cada enemigo debe tener un identificador �nico o tag, en este caso se asume que los enemigos 
        // tienen un string o ID �nico que pasa a trav�s del par�metro
        if (bichosDestruidos < totalBichos)
        {
            bichosDestruidos++;
            Debug.Log($"Bicho destruido: {bichoID}. Total destruidos: {bichosDestruidos}");

            // Si el jugador ha destruido todos los bichos
            if (bichosDestruidos == totalBichos)
            {
                Debug.Log("�Ganaste 1 estrella por destruir a todos los bichos!");
                // Aqu� puedes otorgar una estrella, por ejemplo, sumando una estrella al jugador
            }
        }
    }

    public void ResetearContadorBichos()
    {
        // Reseteamos el contador a 0 cuando se reinicia el juego o se reinicia el nivel
        bichosDestruidos = 0;
    }
}

