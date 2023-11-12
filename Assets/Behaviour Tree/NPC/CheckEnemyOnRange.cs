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
        if (controller.CanChase())
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
