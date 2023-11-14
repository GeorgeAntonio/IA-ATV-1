using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Cainos.PixelArtTopDown_Basic;

public class Attack : Node
{
    private Controller controller;
    public Attack(Controller controller)
    {
        this.controller = controller;
    }
    public override NodeState Evaluate()
    {        
        Debug.Log(this.ToString());        

        if (controller.npcTarget != null)
        {                       
            Vector2 knockbackDirection = (controller.transform.position - controller.npcTarget.position).normalized;            
            controller.npcTarget.GetComponent<Rigidbody2D>().velocity = knockbackDirection * controller.knockbackForce;            
            Controller targetNPCController = controller.npcTarget.GetComponent<Controller>();
            targetNPCController.health -= controller.attackDamage;
            controller.lastAttackTime = Time.time;

            if (targetNPCController.health <= 0)
            {
                if (controller.npcTarget.gameObject == controller.mainCamera.GetComponent<CameraFollow>().target.gameObject) { controller.spawner.isTargetDead = true; }
                UnityEngine.Object.Destroy(controller.npcTarget.gameObject);
            }

            state = NodeState.RUNNING;          
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
