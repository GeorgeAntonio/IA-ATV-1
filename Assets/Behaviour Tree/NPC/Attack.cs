using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Cainos.PixelArtTopDown_Basic;

public class Attack : Node
{
    private Controller controller;
    public Attack(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {        
        controller.npcToAttack = controller.FindClosestNPC();

        if (controller.npcToAttack != null)
        {
            if (controller.CanAttack(controller.npcToAttack.position))
            {
                if (controller.health / (float)controller.maxHealth <= controller.fleeHealthThreshold)
                {
                    state = NodeState.SUCCESS;
                }
                Vector2 knockbackDirection = (controller.transform.position - controller.npcToAttack.position).normalized;
                Rigidbody2D targetRB = controller.npcToAttack.GetComponent<Rigidbody2D>();

                if (targetRB != null)
                {
                    targetRB.velocity = knockbackDirection * controller.knockbackForce;
                }

                Controller targetNPC = controller.npcToAttack.GetComponent<Controller>();
                targetNPC.health -= controller.attackDamage;

                controller.lastAttackTime = Time.time;

                if (targetNPC.health <= 0)
                {
                    if (controller.npcToAttack.gameObject == controller.mainCamera.GetComponent<CameraFollow>().target.gameObject) { controller.spawner.isTargetDead = true; }
                    UnityEngine.Object.Destroy(controller.npcToAttack.gameObject);
                }

                state = NodeState.RUNNING;
            }
            else
            {
                state = NodeState.FAILURE;
            }
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
