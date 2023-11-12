using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Chase : Node 
{ 
    private Transform npcToChase;

    private Controller controller;
    public Chase(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {        
        controller.npcToChase = controller.FindClosestNPC();

        if (npcToChase != null)
        {
            Vector2 moveDirection = (controller.npcToChase.position - controller.transform.position).normalized;

            if (!controller.IsStunned())
            {
                controller.rb.velocity = controller.moveDirection * controller.moveSpeed;
            }

            if (controller.CanAttack(controller.npcToChase.position))
            {
                state = NodeState.SUCCESS;
            }
            else if (!controller.CanChase())
            {
                state = NodeState.FAILURE;
            }
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
