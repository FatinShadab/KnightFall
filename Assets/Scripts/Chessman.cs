using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to each individual chess piece on the board.
public class Chessman : MonoBehaviour
{
    // Reference to the main game controller and the move plate prefab
    public GameObject controller;
    public GameObject movePlate;

    // Logical board coordinates (0-7 for both x and y)
    private int xBoard = -1;
    private int yBoard = -1;

    // 'w' for white player, 'b' for black player
    private char player = 'w';

    // Sprites (images) used for each type of chess piece
    public Sprite bKing, bQueen, bKnight, bBishop, bRook, bPawn;
    public Sprite wKing, wQueen, wKnight, wBishop, wRook, wPawn;

    // This method initializes the chess piece after creation
    public void Activate()
    {
        // Find and assign the GameController script
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Set the piece's position in world space
        SetCoords();

        // Determine which player the piece belongs to (based on name prefix)
        player = this.name.StartsWith("b") ? 'b' : 'w';

        // Assign the correct sprite image to the piece based on its name
        switch (this.name)
        {
            case "bKing":   this.GetComponent<SpriteRenderer>().sprite = bKing; break;
            case "wKing":   this.GetComponent<SpriteRenderer>().sprite = wKing; break;
            case "bQueen":  this.GetComponent<SpriteRenderer>().sprite = bQueen; break;
            case "wQueen":  this.GetComponent<SpriteRenderer>().sprite = wQueen; break;
            case "bBishop": this.GetComponent<SpriteRenderer>().sprite = bBishop; break;
            case "wBishop": this.GetComponent<SpriteRenderer>().sprite = wBishop; break;
            case "bKnight": this.GetComponent<SpriteRenderer>().sprite = bKnight; break;
            case "wKnight": this.GetComponent<SpriteRenderer>().sprite = wKnight; break;
            case "bRook":   this.GetComponent<SpriteRenderer>().sprite = bRook; break;
            case "wRook":   this.GetComponent<SpriteRenderer>().sprite = wRook; break;
            case "bPawn":   this.GetComponent<SpriteRenderer>().sprite = bPawn; break;
            case "wPawn":   this.GetComponent<SpriteRenderer>().sprite = wPawn; break;
        }
    }

    // Converts board coordinates into Unity world space coordinates
    public void SetCoords()
    {
        float x = xBoard * 0.725f + -2.50f;
        float y = yBoard * 0.55f + -2.70f;

        this.transform.position = new Vector3(x, y, -1.0f);

        // Sets rendering order to simulate "depth" (higher Y = drawn behind)
        GetComponent<SpriteRenderer>().sortingOrder = 7 - yBoard;
    }

    // Getters and setters for board position
    public int GetXBoard() => xBoard;
    public int GetYBoard() => yBoard;
    public void SetXBoard(int x) => xBoard = x;
    public void SetYBoard(int y) => yBoard = y;

    // Destroys all move plates currently on the board
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject mp in movePlates)
        {
            Destroy(mp); // Destroy is asynchronous
        }
    }

    // Triggered when the player clicks on a piece
    void OnMouseUp()
    {
        // Only respond to click if game is not over and it's this player's turn
        if (!controller.GetComponent<Game>().IsGameOver() &&
            controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();     // Clear previous move plates
            InitiateMovePlates();    // Show valid moves for this piece
        }
    }

    // Based on the piece type, generate the valid moves
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

    // Handles linear movement like rook, bishop, queen
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        // Continue in a straight line until edge of board or blocked
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        // If next space has opponent's piece, allow attack move
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    // Handles knight's L-shaped moves
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

    // Handles the king's one-step moves in all directions
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    // Checks a single square and creates a move plate if it's valid
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject target = sc.GetPosition(x, y);

            if (target == null)
            {
                MovePlateSpawn(x, y); // Empty space
            }
            else if (target.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y); // Enemy piece
            }
        }
    }

    // Handles pawn movement and attacking
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y); // Move forward
            }

            // Attack diagonally if there's an enemy piece
            if (sc.PositionOnBoard(x + 1, y) &&
                sc.GetPosition(x + 1, y) != null &&
                sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) &&
                sc.GetPosition(x - 1, y) != null &&
                sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    // Spawns a regular move plate at board coordinates
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX * 0.725f + -2.50f;
        float y = matrixY * 0.55f + -3.00f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);       // Which piece this belongs to
        mpScript.SetCoords(matrixX, matrixY);    // Logical board coordinates
    }

    // Spawns an attack move plate (used for capturing enemy pieces)
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
