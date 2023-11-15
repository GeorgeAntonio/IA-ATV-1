using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStun : Node { 
    private Controller controller;
    public CheckStun(Controller controller)
    {
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {
        if(Time.time < controller.stunEndTime)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
