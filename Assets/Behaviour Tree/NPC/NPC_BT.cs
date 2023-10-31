using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BT : BehaviorTree.Tree
{
    public static Controller controller;
    public void Start()
    { 
        controller = Controller.Instance;
    }
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new Sequence(new List<Node>{ new CheckPatrolRange(), new Patrol()}),
            new CheckEnemyOnRange(),
            new Chase(),
            new Sequence(new List<Node> { new CheckEnemyAttackRange(), new CheckEnoughHealth(), new Attack()      }),
            new Flee()
        }) ;

        return root;
    }
}
