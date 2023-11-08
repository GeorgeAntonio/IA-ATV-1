using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Cainos.PixelArtTopDown_Basic;

public class Attack : Node
{
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.npcToAttack != null)
        {
            if (NPC_BT.controller.CanAttack(NPC_BT.controller.npcToAttack.position))
            {
                if (NPC_BT.controller.health / (float)NPC_BT.controller.maxHealth <= NPC_BT.controller.fleeHealthThreshold)
                {
                    state = NodeState.SUCCESS;
                }
                Vector2 knockbackDirection = (NPC_BT.controller.transform.position - NPC_BT.controller.npcToAttack.position).normalized;
                Rigidbody2D targetRB = NPC_BT.controller.npcToAttack.GetComponent<Rigidbody2D>();

                if (targetRB != null)
                {
                    targetRB.velocity = knockbackDirection * NPC_BT.controller.knockbackForce;
                }

                Controller targetNPC = NPC_BT.controller.npcToAttack.GetComponent<Controller>();
                targetNPC.health -= NPC_BT.controller.attackDamage;

                NPC_BT.controller.lastAttackTime = Time.time;

                if (targetNPC.health <= 0)
                {
                    if (NPC_BT.controller.npcToAttack.gameObject == NPC_BT.controller.mainCamera.GetComponent<CameraFollow>().target.gameObject) { NPC_BT.controller.spawner.isTargetDead = true; }
                    UnityEngine.Object.Destroy(NPC_BT.controller.npcToAttack.gameObject);
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
