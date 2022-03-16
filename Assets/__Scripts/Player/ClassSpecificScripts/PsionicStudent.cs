using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsionicStudent : PlayerGeneral
{
    // Handles the special movement the player can perform
    protected override void HandleMovementAbility()
    {
        if (inputManager.specialMoveInput) // Check if player is using the special move input
        {
            Vector3 dashDirection = Vector3.zero; // Track direction player will dash

            if (inputManager.horizontalInput == 0 && inputManager.verticalInput == 0) dashDirection = transform.forward; // If player isn't try to move then dash forwards
            else // If player is trying to move then dash in that direction
            {
                dashDirection = cameraTransform.right * inputManager.horizontalInput;
                dashDirection += cameraTransform.forward * inputManager.verticalInput;
            }

            inputManager.specialMoveInput = false; // Reset back to false

            playerAnimationManager.AplpyRootMotion(true); // Apply the root motion of the animation
            // Shrink collider
            this.GetComponent<CapsuleCollider>().height = 0.1f;
            this.GetComponent<CapsuleCollider>().radius = 0.1f;
            playerAnimationManager.PlayTargetAnimation("Dash Forwards", true); // Play the dash animation
            StartCoroutine("FinishDash"); // Start the coroutine to reset the variables after the animation finishes
        }
    }

    // Handles the special movement the player can perform
    protected override void HandleCombatAbility()
    {
        if (inputManager.specialAbilityInput) // Check if player is using the special ability input
        {
            inputManager.specialAbilityInput = false; // Reset back to false

            StartCoroutine("DelayAbility"); // Start the coroutine related to the abilities
        }
    }

    IEnumerator FinishDash()
    {
        yield return new WaitForSeconds(0.7f); // Wait duration of animation
        playerAnimationManager.AplpyRootMotion(false); // Set Root Motion back to false
        // Set collider back to normal
        this.GetComponent<CapsuleCollider>().height = 1.6f;
        this.GetComponent<CapsuleCollider>().radius = 0.28f;
        yield return null;
    }

    // Allows user to delay start of certain animations related to the ability
    IEnumerator DelayAbility()
    {
        // Check if player is locked onto a target
        if (cameraManager.currentLockOnTarget != null)
        {
            animator.CrossFade("PsionicPull", 0.2f); // Play fling animation
            yield return new WaitForSeconds(0.1f); // Wait for animation
            cameraManager.currentLockOnTarget.parent.GetComponent<EnemyGeneral>().FlingForwards(transform); // Fling enemy towards player
        }
        else
        {
            animator.CrossFade("PsionicPush", 0.2f); // Play fling animation
            yield return new WaitForSeconds(0.27f); // Wait for animation

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3, transform.forward); // Get all objects in a certain area around player

            foreach (RaycastHit hit in hits) // Loop through everything hit
            {
                if (hit.transform.GetComponent<EnemyGeneral>() != null) // Check if hit object is an enemy
                    hit.transform.GetComponent<EnemyGeneral>().FlingBackwards(transform); // Fling enemy backwards
            }
        }

        yield return null;
    }
}
