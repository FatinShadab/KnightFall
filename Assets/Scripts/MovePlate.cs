using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    // A reference to the GameController (needed to access and update game state)
    public GameObject controller;

    // The Chessman (piece) that this move plate belongs to (i.e., the one the player selected)
    GameObject reference = null;

    // The target X and Y position this move plate represents on the chessboard
    int matrixX;
    int matrixY;

    // If true, this move plate indicates an attack move (not just a normal move)
    public bool attack = false;

    // Called when this MovePlate object is created in the scene
    public void Start()
    {
        // If it's an attack plate, change its color to red for visual feedback
        if (attack)
        {
            // Note: This uses a SpriteRenderer, so it's for 2D games. Use material.color for 3D.
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    // Called when the player clicks on this MovePlate
    void OnMouseUp()
    {
        // Find and store reference to the GameController
        controller = GameObject.FindGameObjectWithTag("GameController");

        // If this move is an attack move, destroy the chess piece currently at this location
        if (attack)
        {
            // Get the chess piece at the target location
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            // If the captured piece is a King, the game ends and the opponent wins
            if (cp.name == "wKing") controller.GetComponent<Game>().handleChessWinner("black");
            if (cp.name == "bKing") controller.GetComponent<Game>().handleChessWinner("white");

            // Remove the enemy piece from the scene
            Destroy(cp);
        }

        // Clear the piece's previous position from the game board logic
        controller.GetComponent<Game>().SetPositionEmpty(
            reference.GetComponent<Chessman>().GetXBoard(),
            reference.GetComponent<Chessman>().GetYBoard()
        );

        // Update the piece's logical board coordinates to the new position
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);

        // Move the piece's GameObject in the scene to the new location
        reference.GetComponent<Chessman>().SetCoords();

        // Update the game board matrix to reflect the new position
        controller.GetComponent<Game>().SetPosition(reference);

        // Change the turn to the next player
        controller.GetComponent<Game>().NextTurn();

        // Remove all move plates from the board (including this one)
        reference.GetComponent<Chessman>().DestroyMovePlates();
    }

    // Set the board coordinates this move plate represents
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    // Set which piece this move plate is associated with
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    // Get the reference to the chess piece that owns this move plate
    public GameObject GetReference()
    {
        return reference;
    }
}
