using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnoughHealth : Node
{
    private Controller controller;
    public CheckEnoughHealth(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {        
        if (controller.health / (float)controller.maxHealth <= controller.fleeHealthThreshold)
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
