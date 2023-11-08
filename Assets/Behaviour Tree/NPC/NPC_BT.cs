using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NPC_BT : BehaviorTree.Tree
{
    public static Controller controller;    

    public NPC_BT(GameObject gameObject) : base(gameObject) // Chame o construtor da classe base com o objeto gameObject
    {
    }

    protected override Node SetupTree()
    {
        controller = gameObject.GetComponent<Controller>();
        Node root = new Sequence(new List<Node>
        {
            new Selector(new List<Node>{
                new Sequence(new List<Node> { new CheckEnoughHealth(), new CheckEnemyOnRange(), new Flee()}),
                new Sequence(new List<Node> { new CheckEnemyOnRange(), new Chase(), new CheckEnemyAttackRange(), new Attack() }),
                new Sequence(new List<Node>{ new CheckPatrolRange(), new Patrol()}),
                new Idle()
            })
        }); 

        return root;
    }
}
