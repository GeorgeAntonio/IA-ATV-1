using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnoughHealth : Node
{
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.health / (float)NPC_BT.controller.maxHealth <= NPC_BT.controller.fleeHealthThreshold)
        {
            state = NodeState.SUCCESS;            
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
