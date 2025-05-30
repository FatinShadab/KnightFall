using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private char player = 'w';

    public Sprite bKing, bQueen, bKnight, bBishop, bRook, bPawn;
    public Sprite wKing, wQueen, wKnight, wBishop, wRook, wPawn;

    public void Activate() {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        if (this.name.StartsWith("b")) {
            player = 'b';
        }
        else {
            player = 'w';
        }

        switch (this.name) {
            case "bKing": this.GetComponent<SpriteRenderer>().sprite = bKing; break;
            case "wKing": this.GetComponent<SpriteRenderer>().sprite = wKing; break;
            case "bQueen": this.GetComponent<SpriteRenderer>().sprite = bQueen; break;
            case "wQueen": this.GetComponent<SpriteRenderer>().sprite = wQueen; break;
            case "bBishop": this.GetComponent<SpriteRenderer>().sprite = bBishop; break;
            case "wBishop": this.GetComponent<SpriteRenderer>().sprite = wBishop; break;
            case "bKnight": this.GetComponent<SpriteRenderer>().sprite = bKnight; break;
            case "wKnight": this.GetComponent<SpriteRenderer>().sprite = wKnight; break;
            case "bRook": this.GetComponent<SpriteRenderer>().sprite = bRook; break;
            case "wRook": this.GetComponent<SpriteRenderer>().sprite = wRook; break;
            case "bPawn": this.GetComponent<SpriteRenderer>().sprite = bPawn; break;
            case "wPawn": this.GetComponent<SpriteRenderer>().sprite = wPawn; break;
        }
    }

    public void SetCoords() {
        float x = xBoard;
        float y = yBoard;

        x *= 0.725f; // trial and error
        y *= 0.55f; // trial and error

        x += -2.50f; // trial and error
        y += -2.70f; // trial and error

        this.transform.position = new Vector3(x, y, -1.0f);

        // Apply Y-sorting (higher y => lower order so they appear behind)
        GetComponent<SpriteRenderer>().sortingOrder = 7 - yBoard;
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        this.xBoard = x;
    }

    public void SetYBoard(int y)
    {
        this.yBoard = y;
    }

    public void DestroyMovePlates()
    {
        //Destroy old MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); //Be careful with this function "Destroy" it is asynchronous
        }
    }

    void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            //Remove all moveplates relating to previously selected piece
            DestroyMovePlates();

            //Create new MovePlates
            InitiateMovePlates();
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "bQueen":
            case "wQueen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "bKnight":
            case "wKnight":
                LMovePlate();
                break;
            case "bBishop":
            case "wBishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "bKing":
            case "wKing":
                SurroundMovePlate();
                break;
            case "bRook":
            case "wRook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "bPawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "wPawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

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

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 0.725f;
        y *= 0.55f;

        //Add constants (pos 0,0)
        x += -2.50f;
        y += -3.00f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 0.725f;
        y *= 0.55f;

        //Add constants (pos 0,0)
        x += -2.50f;
        y += -3.00f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
