using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Flee : Node
{
    private Controller controller;

    public Flee(Controller controller)
    {
        this.controller = controller;
    }
<<<<<<< HEAD
    private UnityEngine.Transform FindNPCToFleeFrom()
=======

    private Transform FindNPCToFleeFrom()
>>>>>>> 1229c85d42bebef9f4a5c425c0edaba9e22b62f5
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        UnityEngine.Transform npcToFleeFrom = null;
        float highestHealthDifference = 0f;

        foreach (GameObject npc in npcs)
        {
            if (npc != controller.gameObject)
            {
                float distance = Vector2.Distance(controller.transform.position, npc.transform.position);
                NPC otherNPC = npc.GetComponent<NPC>();

                if (otherNPC != null)  // Check if otherNPC is not null
                {
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
        }

        return npcToFleeFrom;
    }
<<<<<<< HEAD
    private UnityEngine.Transform FindNearestHealthPotion()
=======


    private Transform FindNearestHealthPotion()
>>>>>>> 1229c85d42bebef9f4a5c425c0edaba9e22b62f5
    {
        GameObject[] healthPotions = GameObject.FindGameObjectsWithTag("HealthPotion");

        UnityEngine.Transform nearestPotion = null;
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
        controller.patrolPoint = null;
        Vector2 fleeDirection;

        // Health is 20% or less, flee towards health potions
<<<<<<< HEAD
        UnityEngine.Transform nearestPotion = FindNearestHealthPotion();
=======
        Transform nearestPotion = FindNearestHealthPotion();
        Transform npcToFleeFrom = FindNPCToFleeFrom();

>>>>>>> 1229c85d42bebef9f4a5c425c0edaba9e22b62f5
        Debug.Log(this.ToString());

        if (!controller.IsStunned())
        {
<<<<<<< HEAD
            if (controller.npcTarget != null)
            {
                if (nearestPotion != null)
                {
                    fleeDirection = (nearestPotion.position - controller.transform.position).normalized;
                    if (controller.isHealing && nearestPotion == controller.selectedPotion)
                    {
                        controller.Heal(controller.healthPotionValue, nearestPotion);
                    }
=======
            if (nearestPotion != null)
            {
                fleeDirection = (nearestPotion.position - controller.transform.position).normalized;

                if (controller.isHealing && nearestPotion == controller.selectedPotion)
                {
                    controller.Heal(controller.healthPotionValue, nearestPotion);
>>>>>>> 1229c85d42bebef9f4a5c425c0edaba9e22b62f5
                }
                else
                {
                    fleeDirection = (controller.transform.position - controller.npcTarget.transform.position).normalized;
                }
                controller.rb.velocity = fleeDirection * controller.moveSpeed;
                state = NodeState.RUNNING;
            }
            else if (npcToFleeFrom != null)
            {
                // Flee from the NPC
                fleeDirection = (controller.transform.position - npcToFleeFrom.position).normalized;
            }
            else
            {
<<<<<<< HEAD
                controller.npcTarget = null;
                state = NodeState.SUCCESS;
            }
=======
                // No NPC to flee from, maintain current direction (or set a default direction)
                fleeDirection = Vector2.zero;
            }

            controller.rb.velocity = fleeDirection * controller.moveSpeed;
            state = NodeState.RUNNING;
>>>>>>> 1229c85d42bebef9f4a5c425c0edaba9e22b62f5
        }

        return state;
    }
}
