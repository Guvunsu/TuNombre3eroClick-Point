using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Aseg�rate de tener la referencia a TextMeshPro para mostrar texto en el Canvas

public class TimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    public float tiempoLimite = 60f;  // Tiempo l�mite en segundos (por ejemplo, 60 segundos)

    [Header("UI References")]
    public TextMeshProUGUI textoTimer;  // Referencia al TextMeshProUGUI para mostrar el tiempo

    private float tiempoRestante;  // Tiempo restante en segundos
    private bool timerActivo = true;  // Controla si el timer est� activo o no
    private bool isPaused = false;   // Controla si el timer est� en pausa

    private void Start()
    {
        // Inicializa el tiempo restante con el tiempo l�mite
        tiempoRestante = tiempoLimite;
    }

    private void Update()
    {
        // Si el timer est� activo y no est� pausado
        if (timerActivo && !isPaused)
        {
            // Decrementamos el tiempo usando unscaledDeltaTime para que no se vea afectado por la pausa
            tiempoRestante -= Time.unscaledDeltaTime;

            // Si el tiempo llega a 0, detenemos el timer
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                timerActivo = false;
                // Aqu� podr�as activar alguna l�gica cuando el timer llegue a 0, como finalizar el nivel
            }

            // Actualizamos el texto del timer en el UI
            ActualizarTextoTimer();
        }
    }

    // M�todo para actualizar el texto del Timer en el Canvas
    void ActualizarTextoTimer()
    {
        // Convertimos el tiempo restante a minutos:segundos y lo mostramos en el UI
        float minutos = Mathf.FloorToInt(tiempoRestante / 60);
        float segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // M�todo p�blico para detener el timer (por si lo necesitas desde otro script)
    public void DetenerTimer()
    {
        timerActivo = false;
    }

    // M�todo p�blico para reiniciar el timer (en caso de que se necesite reiniciar en alg�n momento)
    public void ReiniciarTimer()
    {
        tiempoRestante = tiempoLimite;
        timerActivo = true;
    }

    // M�todo p�blico para cambiar el tiempo l�mite si es necesario desde otro script
    public void CambiarTiempoLimite(float nuevoTiempo)
    {
        tiempoLimite = nuevoTiempo;
        ReiniciarTimer();  // Reinicia el timer con el nuevo tiempo l�mite
    }

    // M�todo para pausar el timer
    public void PausarTimer()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Detiene el tiempo del juego (efecto global de pausa)
    }

    // M�todo para reanudar el timer
    public void ReanudarTimer()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Restaura el tiempo del juego (efecto global de reanudaci�n)
    }
}
