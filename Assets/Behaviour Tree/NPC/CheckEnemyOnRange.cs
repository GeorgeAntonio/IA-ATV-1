using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyOnRange : Node
{
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller != null && NPC_BT.controller.npcToChase != null)
        {
            float distanceToChase = Vector2.Distance(NPC_BT.controller.transform.position, NPC_BT.controller.npcToChase.position);

            if (distanceToChase <= NPC_BT.controller.chaseRange)
            {
                state = NodeState.SUCCESS;
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
