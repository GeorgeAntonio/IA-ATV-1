using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyOnRange : Node
{
    private Controller controller;
    public CheckEnemyOnRange(Controller controller)
    {
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());
        Transform npcTarget = controller.FindClosestNPC();
        if (npcTarget != null && npcTarget.gameObject.GetComponent<Controller>().npcTarget == controller.gameObject)
        {
            controller.npcTarget = npcTarget;
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
