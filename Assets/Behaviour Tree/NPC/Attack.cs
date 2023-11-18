using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Cainos.PixelArtTopDown_Basic;

public class Attack : Node
{
    private Controller controller;
    private float timeSinceLastAttack;
    private bool isEnemyDestroyed;

    public Attack(Controller controller)
    {
        this.controller = controller;
        this.timeSinceLastAttack = 0f;
        this.isEnemyDestroyed = false;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());

        // Check if the enemy is destroyed
        if (isEnemyDestroyed)
        {
            // Enemy is destroyed, switch to patrol
            state = NodeState.FAILURE;
            isEnemyDestroyed = false; // Reset the flag
            return state;
        }

        if (controller.npcTarget != null)
        {
            // Check if enough time has passed since the last attack
            if (Time.time - timeSinceLastAttack >= controller.attackCooldown)
            {
                Vector2 knockbackDirection = (controller.transform.position - controller.npcTarget.position).normalized;
                controller.npcTarget.GetComponent<Rigidbody2D>().velocity = knockbackDirection * controller.knockbackForce;

                Controller targetNPCController = controller.npcTarget.GetComponent<Controller>();
                targetNPCController.health -= controller.attackDamage;
                controller.lastAttackTime = Time.time;
                targetNPCController.stunEndTime = Time.time + targetNPCController.stunDuration;

                if (targetNPCController.health <= 0)
                {
                    if (controller.npcTarget.gameObject == controller.mainCamera.GetComponent<CameraFollow>().target.gameObject)
                    {
                        controller.spawner.isTargetDead = true;
                    }

                    UnityEngine.Object.Destroy(controller.npcTarget.gameObject);

                    // Set the flag to indicate that the enemy is destroyed
                    isEnemyDestroyed = true;
                }

                // Update the time of the last attack
                timeSinceLastAttack = Time.time;

                state = NodeState.SUCCESS;
                controller.npcTarget = null;
            }
            else
            {
                // Not enough time has passed since the last attack
                state = NodeState.FAILURE;
            }
        }
        else
        {
            // No target to attack
            state = NodeState.FAILURE;
        }

        return state;
    }
}
