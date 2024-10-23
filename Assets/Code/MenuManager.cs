using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer.Internal.Converters;

public class MenuManager : MonoBehaviour {
    #region Variables

    public GameObject misionSelect, shopMenu, exitGO;
    public Button MainMenu, missionSelect, exit;

    #endregion Variables

    #region Funciones Publicas 
    // Métodos para cambiar entre escenas
    public void LoadMenuPrincipal() {
        SceneManager.LoadScene("Menu");
    }

    public void LoadSeleccionMisiones() {
        SceneManager.LoadScene("MisionSelect");
    }

    public void LoadTienda() {
        SceneManager.LoadScene("Tienda");
    }

    public void LoadGameplay() {
        SceneManager.LoadScene("Gameplay");
    }
    public void QuitGame() {

        // Si estás en el editor de Unity, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    #endregion Funciones Publicas

}



