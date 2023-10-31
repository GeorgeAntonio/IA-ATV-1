using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class Flee : Node
{
    private Transform FindNPCToFleeFrom()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        Transform npcToFleeFrom = null;
        float highestHealthDifference = 0f;

        foreach (GameObject npc in npcs)
        {
            if (npc != NPC_BT.controller.gameObject)
            {
                float distance = Vector2.Distance(NPC_BT.controller.transform.position, npc.transform.position);
                NPC otherNPC = npc.GetComponent<NPC>();

                if (distance <= NPC_BT.controller.attackRange && otherNPC.health > NPC_BT.controller.health)
                {
                    float healthDifference = otherNPC.health - NPC_BT.controller.health;

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
            float distanceToPotion = Vector2.Distance(NPC_BT.controller.transform.position, potion.transform.position);

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
        // Health is 20% or less, flee towards health potions
        Transform nearestPotion = FindNearestHealthPotion();

        if (nearestPotion != null)
        {
            Vector2 fleeDirection = (nearestPotion.position - NPC_BT.controller.transform.position).normalized;
            state = NodeState.RUNNING;
            if (!NPC_BT.controller.IsStunned())
            {
                NPC_BT.controller.rb.velocity = fleeDirection * NPC_BT.controller.moveSpeed;
                state = NodeState.FAILURE;
            }
        }
        else {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
