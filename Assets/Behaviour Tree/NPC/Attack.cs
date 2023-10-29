using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Attack : Node
{
    public override NodeState Evaluate()
    {
        Transform npcToAttack = NPC_BT.controller.FindClosestNPC();

        if (npcToAttack != null)
        {
            if (NPC_BT.controller.CanAttack(npcToAttack.position))
            {
                if (NPC_BT.controller.health / (float)NPC_BT.controller.maxHealth <= NPC_BT.controller.fleeHealthThreshold)
                {
                    state = NodeState.SUCCESS;
                }
                Vector2 knockbackDirection = (NPC_BT.controller.transform.position - npcToAttack.position).normalized;
                Rigidbody2D targetRB = npcToAttack.GetComponent<Rigidbody2D>();

                if (targetRB != null)
                {
                    targetRB.velocity = knockbackDirection * NPC_BT.controller.knockbackForce;
                }

                NPC targetNPC = npcToAttack.GetComponent<NPC>();
                targetNPC.health -= NPC_BT.controller.attackDamage;

                NPC_BT.controller.lastAttackTime = Time.time;

                if (targetNPC.health <= 0)
                {
                    if (npcToAttack.gameObject == NPC_BT.controller.mainCamera.GetComponent<CameraFollow>().target.gameObject) { NPC_BT.controller.spawner.isTargetDead = true; }
                    Destroy(npcToAttack.gameObject);
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
