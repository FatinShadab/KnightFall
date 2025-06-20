using UnityEngine;

/// <summary>
/// Controls the animation logic for the WhitePlayer character.
/// Triggers attack animations with randomized variations upon chess movement.
/// </summary>
public class WhitePlayer : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    /// <summary>
    /// Called when the object is first initialized.
    /// Retrieves the Animator component attached to this GameObject.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Plays a randomized attack animation on the WhitePlayer.
    /// Uses blend tree mode values to choose an animation variant.
    /// </summary>
    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            Debug.Log("Found animator !");
            float[] attackModes = { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            float randomIndex = attackModes[Random.Range(0, attackModes.Length)];
            Debug.Log(randomIndex);
            animator.SetFloat("Mode", randomIndex); // only if you’re switching between attack0 and attack1
            animator.SetBool("madeAmove", true);
            animator.SetTrigger("Attack Tree"); // This triggers the transition

            // Automatically reset after 1 second
            StartCoroutine(ResetMoveFlag());
        }

        /// <summary>
        /// Coroutine that resets the madeAmove flag after a short delay.
        /// Prevents the attack animation from being stuck in active state.
        /// </summary>
        System.Collections.IEnumerator ResetMoveFlag()
        {
            yield return new WaitForSeconds(0.25f); // Adjust based on your attack clip length
            animator.SetBool("madeAmove", false);
        }
    }
}
