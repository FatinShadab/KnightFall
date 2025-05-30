using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // New Input System


public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] bPlayer = new GameObject[16];
    private GameObject[] wPlayer = new GameObject[16];

    private char currentPlayer = 'w';
    private bool gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // White pieces (bottom two rows: y = 0, y = 1)
        wPlayer = new GameObject[]
        {
            // Back rank
            Create("wRook",   0, 0), Create("wKnight", 1, 0), Create("wBishop", 2, 0), Create("wQueen", 3, 0),
            Create("wKing",   4, 0), Create("wBishop", 5, 0), Create("wKnight", 6, 0), Create("wRook",  7, 0),

            // Pawn rank
            Create("wPawn",   0, 1), Create("wPawn",   1, 1), Create("wPawn",   2, 1), Create("wPawn",  3, 1),
            Create("wPawn",   4, 1), Create("wPawn",   5, 1), Create("wPawn",   6, 1), Create("wPawn",  7, 1)
        };

        // Black pieces (top two rows: y = 6, y = 7)
        bPlayer = new GameObject[]
        {
            // Pawn rank
            Create("bPawn",   0, 6), Create("bPawn",   1, 6), Create("bPawn",   2, 6), Create("bPawn",  3, 6),
            Create("bPawn",   4, 6), Create("bPawn",   5, 6), Create("bPawn",   6, 6), Create("bPawn",  7, 6),

            // Back rank
            Create("bRook",   0, 7), Create("bKnight", 1, 7), Create("bBishop", 2, 7), Create("bQueen", 3, 7),
            Create("bKing",   4, 7), Create("bBishop", 5, 7), Create("bKnight", 6, 7), Create("bRook",  7, 7)
        };

        //Set all piece positions on the positions board
        for (int i = 0; i < bPlayer.Length; i++)
        {
            SetPosition(bPlayer[i]);
            SetPosition(wPlayer[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script

        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()

        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public char GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == 'w')
        {
            currentPlayer = 'b';
        }
        else
        {
            currentPlayer = 'w';
        }
    }

    public void handleChessWinner(string winningColor) {
        Debug.Log(winningColor);
        gameOver = true;
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
        }
    }
}
