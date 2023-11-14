using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Patrol : Node
{

    private Controller controller;
    public Patrol(Controller controller)
    {
        this.controller = controller;
    }

    public override NodeState Evaluate()
    {        
        Debug.Log(this.ToString());
        if (!controller.IsStunned())
        {
            controller.rb.velocity = controller.moveDirection * controller.moveSpeed;
        }

           
        state = NodeState.RUNNING; 
        return state;
    }

}
