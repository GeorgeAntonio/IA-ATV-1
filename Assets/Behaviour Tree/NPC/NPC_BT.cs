using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BT : BehaviorTree.Tree
{
    public static Controller controller;    
    protected override Node SetupTree()
    {
        controller = gameObject.GetComponent<Controller>();
        Node root = new Sequence(new List<Node>
        {
            new Selector(new List<Node>{
                new Sequence(new List<Node> { new CheckEnoughHealth(), new CheckEnemyOnRange(),  new Flee()}),
                new Sequence(new List<Node> { new CheckEnemyOnRange(), new Chase(), new CheckEnemyAttackRange(), new Attack() }),
                new Sequence(new List<Node>{ new CheckPatrolRange(), new Patrol()}),
                new Idle()
            })
        }); 
        return root;
    }
}
