using UnityEngine;

public class BlackPlayer : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

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

        System.Collections.IEnumerator ResetMoveFlag()
        {
            yield return new WaitForSeconds(0.25f); // Adjust based on your attack clip length
            animator.SetBool("madeAmove", false);
        }
    }
}
