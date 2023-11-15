using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.Rendering;

public class Chase : Node 
{ 

    private Controller controller;
    public Chase(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {
        controller.patrolPoint = null;
        if (controller.CanAttack(controller.npcTarget.position))
        {
            return NodeState.SUCCESS;
        }
            if (controller.npcTarget != null && !controller.IsStunned())
        {
            controller.moveDirection = (controller.npcTarget.position - controller.transform.position).normalized;            
            controller.rb.velocity = controller.moveDirection * controller.moveSpeed;
           
        }
        /*else
        {
            state = NodeState.FAILURE;
        }*/
        state = NodeState.RUNNING;          
        return state;
    }
}
