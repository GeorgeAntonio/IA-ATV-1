using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Chase : Node 
{ 
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.npcToChase != null)
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
            else
            {
                state = NodeState.RUNNING;
            }
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
