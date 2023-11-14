using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyAttackRange : Node
{

    private Controller controller;
    public CheckEnemyAttackRange(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {       
        if (controller.CanAttack(controller.npcToChase.position))
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
