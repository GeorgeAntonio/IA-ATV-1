using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Patrol : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());
        if (!NPC_BT.controller.IsStunned())
        {
            NPC_BT.controller.rb.velocity = NPC_BT.controller.moveDirection * NPC_BT.controller.moveSpeed;
        }

           
        state = NodeState.RUNNING; 
        return state;
    }

}
