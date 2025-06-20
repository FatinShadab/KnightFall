using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Used for the new Unity Input System

/// <summary>
/// Main Game manager for the KnightFall chess system.
/// Handles piece spawning, turn logic, board setup, and game restart.
/// </summary>
public class Game : MonoBehaviour
{
    // Prefab for the chess piece
    public GameObject chesspiece;

    // Reference to the Game Over UI panel
    public GameObject gameOverUI;

    // 2D array representing the 8x8 chessboard grid
    private GameObject[,] positions = new GameObject[8, 8];

    // Arrays to hold all black and white pieces
    private GameObject[] bPlayer = new GameObject[16];
    private GameObject[] wPlayer = new GameObject[16];

    // Track which player's turn it is ('w' for white, 'b' for black)
    private char currentPlayer = 'w';

    // Boolean flag to indicate if the game has ended
    private bool gameOver = false;

    /// <summary>
    /// Called when the scene starts.
    /// Initializes the chessboard with pieces in starting positions.
    /// </summary>
    void Start()
    {
        // Initialize white player pieces (bottom of the board)
        wPlayer = new GameObject[]
        {
            Create("wRook", 0, 0), Create("wKnight", 1, 0), Create("wBishop", 2, 0), Create("wQueen", 3, 0),
            Create("wKing", 4, 0), Create("wBishop", 5, 0), Create("wKnight", 6, 0), Create("wRook", 7, 0),
            Create("wPawn", 0, 1), Create("wPawn", 1, 1), Create("wPawn", 2, 1), Create("wPawn", 3, 1),
            Create("wPawn", 4, 1), Create("wPawn", 5, 1), Create("wPawn", 6, 1), Create("wPawn", 7, 1)
        };

        // Initialize black player pieces (top of the board)
        bPlayer = new GameObject[]
        {
            Create("bPawn", 0, 6), Create("bPawn", 1, 6), Create("bPawn", 2, 6), Create("bPawn", 3, 6),
            Create("bPawn", 4, 6), Create("bPawn", 5, 6), Create("bPawn", 6, 6), Create("bPawn", 7, 6),
            Create("bRook", 0, 7), Create("bKnight", 1, 7), Create("bBishop", 2, 7), Create("bQueen", 3, 7),
            Create("bKing", 4, 7), Create("bBishop", 5, 7), Create("bKnight", 6, 7), Create("bRook", 7, 7)
        };

        // Place all pieces on the positions grid
        for (int i = 0; i < bPlayer.Length; i++)
        {
            SetPosition(bPlayer[i]);
            SetPosition(wPlayer[i]);
        }
    }

    /// <summary>
    /// Instantiates a new chess piece based on name and position.
    /// </summary>
    /// <param name="name">The name of the chess piece (e.g., "wPawn").</param>
    /// <param name="x">X coordinate on the board.</param>
    /// <param name="y">Y coordinate on the board.</param>
    /// <returns>Instantiated GameObject of the chess piece.</returns>
    GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    /// <summary>
    /// Places a piece into the board array at its current (x, y) position.
    /// </summary>
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    /// <summary>
    /// Empties the board cell at (x, y).
    /// </summary>
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    /// <summary>
    /// Gets the GameObject (chess piece) at (x, y), or null if empty.
    /// </summary>
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    /// <summary>
    /// Checks whether the given (x, y) is within board bounds.
    /// </summary>
    public bool PositionOnBoard(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < 8 && y < 8);
    }

    /// <summary>
    /// Returns the current player character: 'w' or 'b'.
    /// </summary>
    public char GetCurrentPlayer()
    {
        return currentPlayer;
    }

    /// <summary>
    /// Returns true if the game is over.
    /// </summary>
    public bool IsGameOver()
    {
        return gameOver;
    }

    /// <summary>
    /// Switches the turn to the next player.
    /// </summary>
    public void NextTurn()
    {
        currentPlayer = (currentPlayer == 'w') ? 'b' : 'w';
    }

    /// <summary>
    /// Called when a player wins the chess phase.
    /// Triggers the duel sequence in KnightFall (or Game Over UI).
    /// </summary>
    /// <param name="winningColor">Color of the winning player ("white" or "black").</param>
    public void handleChessWinner(string winningColor)
    {
        Debug.Log(winningColor + " wins the chess match!");
        gameOver = true;

        // Optional: Add logic to store winner for duel transition
    }

    /// <summary>
    /// Called once per frame.
    /// If the game is over and player clicks, shows Game Over panel.
    /// </summary>
    public void Update()
    {
        if (gameOver && Mouse.current.leftButton.wasPressedThisFrame)
        {
            gameOverUI.SetActive(true);
        }
    }

    /// <summary>
    /// Bound to the "Rematch" button of Game Over Panel.
    /// Reloads the current gameplay scene.
    /// </summary>
    public void OnRematchButtonPressed()
    {
        gameOverUI.SetActive(false);
        gameOver = false;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Bound to the "Menu" button of Game Over Panel.
    /// Returns to the main menu scene.
    /// </summary>
    public void OnMenuButtonPressed()
    {
        gameOverUI.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
