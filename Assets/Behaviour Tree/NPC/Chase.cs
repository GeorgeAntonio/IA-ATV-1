using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Chase : Node 
{ 

    private Controller controller;
    public Chase(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {                     
        if (controller.npcTarget != null && !controller.IsStunned())
        {
            controller.moveDirection = (controller.npcTarget.position - controller.transform.position).normalized;            
            controller.rb.velocity = controller.moveDirection * controller.moveSpeed;
            state = NodeState.RUNNING;          
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
