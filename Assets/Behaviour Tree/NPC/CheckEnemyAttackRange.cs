using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyAttackRange : Node
{
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.CanAttack(NPC_BT.controller.npcToChase.position))
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
