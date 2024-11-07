using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Asegúrate de tener la referencia a TextMeshPro para mostrar texto en el Canvas

public class TimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    public float tiempoLimite = 60f;  // Tiempo límite en segundos (por ejemplo, 60 segundos)

    [Header("UI References")]
    public TextMeshProUGUI textoTimer;  // Referencia al TextMeshProUGUI para mostrar el tiempo

    private float tiempoRestante;  // Tiempo restante en segundos
    private bool timerActivo = true;  // Controla si el timer está activo o no
    private bool isPaused = false;   // Controla si el timer está en pausa

    private void Start()
    {
        // Inicializa el tiempo restante con el tiempo límite
        tiempoRestante = tiempoLimite;
    }

    private void Update()
    {
        // Si el timer está activo y no está pausado
        if (timerActivo && !isPaused)
        {
            // Decrementamos el tiempo usando unscaledDeltaTime para que no se vea afectado por la pausa
            tiempoRestante -= Time.unscaledDeltaTime;

            // Si el tiempo llega a 0, detenemos el timer
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                timerActivo = false;
                // Aquí podrías activar alguna lógica cuando el timer llegue a 0, como finalizar el nivel
            }

            // Actualizamos el texto del timer en el UI
            ActualizarTextoTimer();
        }
    }

    // Método para actualizar el texto del Timer en el Canvas
    void ActualizarTextoTimer()
    {
        // Convertimos el tiempo restante a minutos:segundos y lo mostramos en el UI
        float minutos = Mathf.FloorToInt(tiempoRestante / 60);
        float segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // Método público para detener el timer (por si lo necesitas desde otro script)
    public void DetenerTimer()
    {
        timerActivo = false;
    }

    // Método público para reiniciar el timer (en caso de que se necesite reiniciar en algún momento)
    public void ReiniciarTimer()
    {
        tiempoRestante = tiempoLimite;
        timerActivo = true;
    }

    // Método público para cambiar el tiempo límite si es necesario desde otro script
    public void CambiarTiempoLimite(float nuevoTiempo)
    {
        tiempoLimite = nuevoTiempo;
        ReiniciarTimer();  // Reinicia el timer con el nuevo tiempo límite
    }

    // Método para pausar el timer
    public void PausarTimer()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Detiene el tiempo del juego (efecto global de pausa)
    }

    // Método para reanudar el timer
    public void ReanudarTimer()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Restaura el tiempo del juego (efecto global de reanudación)
    }
}
