using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Idle : Node
{

    private Controller controller;
    public Idle(Controller controller)
    {
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());
        if (controller.randomWanderTarget == Vector2.zero || Vector2.Distance(controller.transform.position,controller.randomWanderTarget) < 1f)
        {
            controller.randomWanderTarget = (Vector2)controller.transform.position + Random.insideUnitCircle * 5f;
        }

        Vector2 moveDirection = (controller.randomWanderTarget - (Vector2)controller.transform.position).normalized;

        if (!controller.IsStunned())
        {
            controller.rb.velocity = moveDirection * controller.moveSpeed;
            state = NodeState.RUNNING;
        }
        
        return state;
    }
}
