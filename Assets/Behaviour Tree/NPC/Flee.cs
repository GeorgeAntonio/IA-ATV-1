using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class Flee : Node
{
    private Controller controller;
    public Flee(Controller controller)
    {
        this.controller = controller;
    }
    private Transform FindNPCToFleeFrom()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        Transform npcToFleeFrom = null;
        float highestHealthDifference = 0f;

        foreach (GameObject npc in npcs)
        {
            if (npc != controller.gameObject)
            {
                float distance = Vector2.Distance(controller.transform.position, npc.transform.position);
                NPC otherNPC = npc.GetComponent<NPC>();

                if (distance <= controller.attackRange && otherNPC.health > controller.health)
                {
                    float healthDifference = otherNPC.health - controller.health;

                    if (healthDifference > highestHealthDifference)
                    {
                        highestHealthDifference = healthDifference;
                        npcToFleeFrom = npc.transform;
                    }
                }
            }
        }

        return npcToFleeFrom;
    }
    private Transform FindNearestHealthPotion()
    {
        GameObject[] healthPotions = GameObject.FindGameObjectsWithTag("HealthPotion");

        Transform nearestPotion = null;
        float nearestPotionDistance = float.MaxValue;

        foreach (GameObject potion in healthPotions)
        {
            float distanceToPotion = Vector2.Distance(controller.transform.position, potion.transform.position);

            if (distanceToPotion < nearestPotionDistance)
            {
                nearestPotion = potion.transform;
                nearestPotionDistance = distanceToPotion;
            }
        }

        return nearestPotion;
    }


    public override NodeState Evaluate()
    {
        Vector2 fleeDirection;
        // Health is 20% or less, flee towards health potions
        Transform nearestPotion = FindNearestHealthPotion();
        Debug.Log(this.ToString());
        if (!controller.IsStunned())
        {            
            if (nearestPotion != null)
            {
                fleeDirection = (nearestPotion.position - controller.transform.position).normalized;                
            }
            else
            {
                fleeDirection = controller.npcTarget.gameObject.transform.position * -1;
            }
            controller.rb.velocity = fleeDirection * controller.moveSpeed;
            state = NodeState.RUNNING;
        }
        return state;
    }
}
