using BehaviorTree;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_BT : BehaviorTree.Tree
{
    private Controller controller;
    protected override Node SetupTree()
    {
        controller = gameObject.GetComponent<Controller>();
        Node root = new BehaviorTree.Sequence(new List<Node>
        {
            new Selector(new List<Node>{
                new BehaviorTree.Sequence(new List<Node> { new CheckEnoughHealth(controller), new CheckEnemyOnRange(controller),  new Flee(controller) }),
                new BehaviorTree.Sequence(new List<Node> { new CheckEnemyOnRange(controller), new Chase(controller), new CheckEnemyAttackRange(controller), new Attack(controller) }),
                new BehaviorTree.Sequence(new List<Node>{ new CheckPatrolRange(controller), new Patrol(controller)}),
                new Idle(controller)
            })
        }); 
        return root;
    }
}
