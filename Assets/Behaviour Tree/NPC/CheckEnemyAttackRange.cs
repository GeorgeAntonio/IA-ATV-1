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
        Debug.Log(this.ToString());
        Transform npcTarget = controller.FindClosestNPC();
        if (npcTarget != null && controller.CanAttack(npcTarget.position))
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
