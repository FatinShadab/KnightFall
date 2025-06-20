using UnityEngine;

/// <summary>
/// Controls animation logic for the Black player character in KnightFall.
/// Responsible for triggering attack animations when the character moves.
/// </summary>
public class BlackPlayer : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    /// <summary>
    /// Initializes the animator component.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Triggers a randomized attack animation.
    /// This is typically called when a black chess piece makes a move.
    /// </summary>
    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            Debug.Log("Found animator!");

            // Define different animation mode weights to randomize attack
            float[] attackModes = { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            float randomIndex = attackModes[Random.Range(0, attackModes.Length)];
            Debug.Log($"Attack Mode: {randomIndex}");

            // Set animator parameters
            animator.SetFloat("Mode", randomIndex);    // Blend tree parameter
            animator.SetBool("madeAmove", true);       // Flag used in Animator transitions
            animator.SetTrigger("Attack Tree");        // Trigger transition to attack state

            // Reset the flag after a short delay
            StartCoroutine(ResetMoveFlag());
        }
    }

    /// <summary>
    /// Coroutine to reset "madeAmove" flag after animation completes.
    /// Prevents animation from getting stuck in the move state.
    /// </summary>
    private System.Collections.IEnumerator ResetMoveFlag()
    {
        yield return new WaitForSeconds(0.25f); // Adjust based on animation length
        animator.SetBool("madeAmove", false);
    }
}
