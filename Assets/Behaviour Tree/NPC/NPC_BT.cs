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

        Node patrolSequence = new BehaviorTree.Sequence(new List<Node>
        {
            new CheckPatrolRange(controller),
            new Patrol(controller)
        });

        Node attackSequence = new BehaviorTree.Sequence(new List<Node>
        {
            new CheckEnemyOnRange(controller),
            new Chase(controller),
            new Attack(controller)
        });

        Node fleeSequence = new BehaviorTree.Sequence(new List<Node>
        {
            new CheckStun(controller),
            new BehaviorTree.Sequence(new List<Node>
            {
                new CheckEnoughHealth(controller),
                new CheckEnemyOnRange(controller),
                new Flee(controller)
            })
        });

        Node root = new Selector(new List<Node>
        {
            new CheckStun(controller),
            new BehaviorTree.Sequence(new List<Node>
            {
                new CheckEnoughHealth(controller),
                new CheckEnemyOnRange(controller),
                new Flee(controller)
            }),
            new BehaviorTree.Sequence(new List<Node>
            {
                new CheckEnemyOnRange(controller),
                new Chase(controller),
                new Attack(controller)
            }),
            patrolSequence
            //new Idle(controller)
        });

        return root;
    }
}
