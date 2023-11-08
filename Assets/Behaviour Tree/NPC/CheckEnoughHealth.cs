using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnoughHealth : Node
{
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller != null)
        {
            float healthPercentage = (float)NPC_BT.controller.health / NPC_BT.controller.maxHealth;

            if (healthPercentage <= NPC_BT.controller.fleeHealthThreshold)
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
