using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Idle : Node
{    
    public override NodeState Evaluate()
    {
        if (NPC_BT.controller.randomWanderTarget == Vector2.zero || Vector2.Distance(NPC_BT.controller.transform.position, NPC_BT.controller.randomWanderTarget) < 1f)
        {
            // Gerar um novo alvo de movimentação aleatória
            NPC_BT.controller.randomWanderTarget = (Vector2)NPC_BT.controller.transform.position + Random.insideUnitCircle * 5f;
        }

        Vector2 moveDirection = (NPC_BT.controller.randomWanderTarget - (Vector2)NPC_BT.controller.transform.position).normalized;

        // Verificar se o NPC não está atordoado
        if (!NPC_BT.controller.IsStunned())
        {
            NPC_BT.controller.rb.velocity = moveDirection * NPC_BT.controller.moveSpeed;
            state = NodeState.RUNNING;
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }
}
