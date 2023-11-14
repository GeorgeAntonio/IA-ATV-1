using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckPatrolRange : Node
{
    private Controller controller;
    public CheckPatrolRange(Controller controller)
    {
        this.controller = controller;
    } 

    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());
        if (controller.patrolPoint != null)
        {
            Vector2 circleDirection = (controller.patrolPoint.position - controller.transform.position).normalized;
            Vector2 offset = new Vector2(-circleDirection.y, circleDirection.x) * controller.patrolCircleRadius;
            Vector2 targetPosition = (Vector2)controller.patrolPoint.position + offset;

            controller.moveDirection = (targetPosition - (Vector2)controller.transform.position).normalized;
            state = NodeState.RUNNING;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
