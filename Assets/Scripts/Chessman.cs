using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages individual chess piece behavior:
/// • Stores position and type
/// • Assigns sprites
/// • Handles valid move and attack logic
/// </summary>
public class Chessman : MonoBehaviour
{
    public GameObject controller;  // Reference to the Game.cs script
    public GameObject movePlate;   // Move plate prefab for movement UI

    private int xBoard = -1, yBoard = -1; // Logical board coordinates
    private char player = 'w';            // 'w' for White, 'b' for Black

    // Sprite assets for all piece types
    public Sprite bKing, bQueen, bKnight, bBishop, bRook, bPawn;
    public Sprite wKing, wQueen, wKnight, wBishop, wRook, wPawn;

    /// <summary>
    /// Initializes the piece:
    /// • Sets position
    /// • Detects player color
    /// • Applies correct sprite
    /// </summary>
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        SetCoords();
        player = this.name.StartsWith("b") ? 'b' : 'w';

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (this.name)
        {
            case "bKing": sr.sprite = bKing; break;
            case "wKing": sr.sprite = wKing; break;
            case "bQueen": sr.sprite = bQueen; break;
            case "wQueen": sr.sprite = wQueen; break;
            case "bBishop": sr.sprite = bBishop; break;
            case "wBishop": sr.sprite = wBishop; break;
            case "bKnight": sr.sprite = bKnight; break;
            case "wKnight": sr.sprite = wKnight; break;
            case "bRook": sr.sprite = bRook; break;
            case "wRook": sr.sprite = wRook; break;
            case "bPawn": sr.sprite = bPawn; break;
            case "wPawn": sr.sprite = wPawn; break;
        }
    }

    public char GetCurrentPlayer() => player;

    /// <summary>
    /// Converts board position to Unity world position.
    /// </summary>
    public void SetCoords()
    {
        float x = xBoard * 0.725f + -2.50f;
        float y = yBoard * 0.55f + -2.70f;
        transform.position = new Vector3(x, y, -1.0f);
        GetComponent<SpriteRenderer>().sortingOrder = 7 - yBoard;
    }

    // Board coordinate accessors
    public int GetXBoard() => xBoard;
    public int GetYBoard() => yBoard;
    public void SetXBoard(int x) => xBoard = x;
    public void SetYBoard(int y) => yBoard = y;

    /// <summary>
    /// Removes all existing move plates.
    /// </summary>
    public void DestroyMovePlates()
    {
        foreach (GameObject mp in GameObject.FindGameObjectsWithTag("MovePlate"))
        {
            Destroy(mp);
        }
    }

    /// <summary>
    /// Handles click on the chess piece.
    /// </summary>
    void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() &&
            controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    /// <summary>
    /// Spawns valid move plates based on piece type.
    /// </summary>
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "bQueen":
            case "wQueen":
                LineMovePlate(1, 0); LineMovePlate(0, 1);
                LineMovePlate(1, 1); LineMovePlate(-1, 0);
                LineMovePlate(0, -1); LineMovePlate(-1, -1);
                LineMovePlate(-1, 1); LineMovePlate(1, -1);
                break;

            case "bKnight":
            case "wKnight":
                LMovePlate();
                break;

            case "bBishop":
            case "wBishop":
                LineMovePlate(1, 1); LineMovePlate(1, -1);
                LineMovePlate(-1, 1); LineMovePlate(-1, -1);
                break;

            case "bKing":
            case "wKing":
                SurroundMovePlate();
                break;

            case "bRook":
            case "wRook":
                LineMovePlate(1, 0); LineMovePlate(0, 1);
                LineMovePlate(-1, 0); LineMovePlate(0, -1);
                break;

            case "bPawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;

            case "wPawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    // Linear movement logic for queen, rook, bishop
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    // Knight movement
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    // King movement (1 square in all directions)
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    // General single-square movement
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject target = sc.GetPosition(x, y);

            if (target == null)
                MovePlateSpawn(x, y);
            else if (target.GetComponent<Chessman>().player != player)
                MovePlateAttackSpawn(x, y);
        }
    }

    // Pawn logic (forward and diagonal capture)
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
            MovePlateSpawn(x, y);

        if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null &&
            sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            MovePlateAttackSpawn(x + 1, y);

        if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null &&
            sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            MovePlateAttackSpawn(x - 1, y);
    }

    // Spawns a normal move plate at the given position
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX * 0.725f + -2.50f;
        float y = matrixY * 0.55f + -3.00f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    // Spawns a red attack move plate for capturing
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX * 0.725f + -2.50f;
        float y = matrixY * 0.55f + -3.00f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
