using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a clickable move or attack plate on the chessboard.
/// Instantiated dynamically around a selected chess piece to show valid moves.
/// Handles movement logic, captures, and animation triggers.
/// </summary>
public class MovePlate : MonoBehaviour
{
    /// <summary>Reference to the Game controller to manage game state.</summary>
    public GameObject controller;

    /// <summary>Reference to the BlackPlayer object (for triggering animation).</summary>
    public GameObject blackPlayerObj;

    /// <summary>Reference to the WhitePlayer object (for triggering animation).</summary>
    public GameObject whitePlayerObj;

    /// <summary>The chess piece that owns this move plate.</summary>
    private GameObject reference = null;

    /// <summary>The X coordinate on the chessboard this plate represents.</summary>
    private int matrixX;

    /// <summary>The Y coordinate on the chessboard this plate represents.</summary>
    private int matrixY;

    /// <summary>True if this move plate represents an attack (i.e., captures an enemy piece).</summary>
    public bool attack = false;

    /// <summary>
    /// Called when the MovePlate is created.
    /// Finds player GameObjects and updates visual color if it's an attack plate.
    /// </summary>
    public void Start()
    {
        blackPlayerObj = GameObject.FindGameObjectWithTag("BlackPlayer");
        whitePlayerObj = GameObject.FindGameObjectWithTag("WhitePlayer");

        if (attack)
        {
            // Change the plate color to red for attack indicators
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Called when the player clicks on this MovePlate.
    /// Executes the move, triggers animations, captures enemies, updates board state.
    /// </summary>
    void OnMouseUp()
    {
        // Access Game controller
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Handle capturing enemy pieces
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            // Check for end-game by capturing a King
            if (cp.name == "wKing") controller.GetComponent<Game>().handleChessWinner("black");
            if (cp.name == "bKing") controller.GetComponent<Game>().handleChessWinner("white");

            Destroy(cp); // Remove captured piece
        }

        // Trigger attack animation for respective player
        if (reference.GetComponent<Chessman>().GetCurrentPlayer() == 'b')
        {
            if (blackPlayerObj != null)
            {
                BlackPlayer blackPlayer = blackPlayerObj.GetComponent<BlackPlayer>();
                if (blackPlayer != null)
                {
                    blackPlayer.PlayAttackAnimation();
                }
            }
        }
        else
        {
            if (whitePlayerObj != null)
            {
                WhitePlayer whitePlayer = whitePlayerObj.GetComponent<WhitePlayer>();
                if (whitePlayer != null)
                {
                    whitePlayer.PlayAttackAnimation();
                }
            }
        }

        // Update board state and piece coordinates
        Game game = controller.GetComponent<Game>();
        Chessman cm = reference.GetComponent<Chessman>();

        game.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());
        cm.SetXBoard(matrixX);
        cm.SetYBoard(matrixY);
        cm.SetCoords(); // Move GameObject in scene
        game.SetPosition(reference); // Update logical board

        game.NextTurn(); // Switch player turn

        // Remove all move plates from the board
        cm.DestroyMovePlates();
    }

    /// <summary>
    /// Sets the logical board coordinates for this move plate.
    /// </summary>
    /// <param name="x">X-coordinate on board</param>
    /// <param name="y">Y-coordinate on board</param>
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    /// <summary>
    /// Links this move plate to a specific chess piece.
    /// </summary>
    /// <param name="obj">The chess piece GameObject</param>
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    /// <summary>
    /// Gets the chess piece this move plate belongs to.
    /// </summary>
    /// <returns>The reference GameObject</returns>
    public GameObject GetReference()
    {
        return reference;
    }
}
