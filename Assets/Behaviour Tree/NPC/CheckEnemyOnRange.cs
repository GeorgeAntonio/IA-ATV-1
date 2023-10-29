using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyOnRange : Node
{

    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.CanChase())
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
