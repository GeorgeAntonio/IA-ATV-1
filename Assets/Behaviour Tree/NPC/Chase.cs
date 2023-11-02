using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Chase : Node 
{ 
    private Transform npcToChase;
    public override NodeState Evaluate()
    {
        NPC_BT.controller.npcToChase = NPC_BT.controller.FindClosestNPC();

        if (npcToChase != null)
        {
            Vector2 moveDirection = (NPC_BT.controller.npcToChase.position - NPC_BT.controller.transform.position).normalized;

            if (!NPC_BT.controller.IsStunned())
            {
                NPC_BT.controller.rb.velocity = moveDirection * NPC_BT.controller.moveSpeed;
            }

            if (NPC_BT.controller.CanAttack(NPC_BT.controller.npcToChase.position))
            {
                state = NodeState.SUCCESS;
            }
            else if (!NPC_BT.controller.CanChase())
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
