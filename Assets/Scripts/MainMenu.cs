using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles Main Menu button functionality for the KnightFall game.
/// Provides methods to start the game or quit the application.
/// Attach this script to a UI manager object in your Main Menu scene.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Loads the main gameplay scene.
    /// This method is called when the "Play" button is pressed.
    /// </summary>
    public void PlayGame()
    {
        // Load scene at index 1 (ensure the game scene is set at index 1 in Build Settings)
        SceneManager.LoadSceneAsync(1);
    }

    /// <summary>
    /// Quits the game application.
    /// This method is called when the "Exit" button is pressed.
    /// </summary>
    public void QuitGame()
    {
        // Exits the application (has no effect in the editor)
        Application.Quit();

        // Log message for debugging in the Unity Editor
        Debug.Log("Game Quit triggered (note: only works in built application)");
    }
}
