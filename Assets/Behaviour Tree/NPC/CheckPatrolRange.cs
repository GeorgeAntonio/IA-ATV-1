using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckPatrolRange : Node
{
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.patrolPoint != null)
        {
            Vector2 circleDirection = (NPC_BT.controller.patrolPoint.position - NPC_BT.controller.transform.position).normalized;
            Vector2 offset = new Vector2(-circleDirection.y, circleDirection.x) * NPC_BT.controller.patrolCircleRadius;
            Vector2 targetPosition = (Vector2)NPC_BT.controller.patrolPoint.position + offset;

            NPC_BT.controller.moveDirection = (targetPosition - (Vector2)NPC_BT.controller.transform.position).normalized;
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
