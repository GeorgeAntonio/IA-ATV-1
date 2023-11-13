using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class Patrol : Node
{
<<<<<<< Updated upstream
    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());

        // Verificar se o NPC não está atordoado
        if (!NPC_BT.controller.IsStunned())
        {
            // Definir a velocidade do Rigidbody para a direção de movimentação
            NPC_BT.controller.rb.velocity = NPC_BT.controller.moveDirection * NPC_BT.controller.moveSpeed;
        }

        state = NodeState.RUNNING;

        return state;
    }
=======
    private Controller controller;
    private AStarPathfinding pathfinding;
    private List<Vector3Int> patrolPath;
    private int currentPatrolIndex;

    public Patrol(Controller controller, AStarPathfinding pathfinding)
    {
        this.controller = controller;
        this.pathfinding = pathfinding;
        this.patrolPath = new List<Vector3Int>();
        this.currentPatrolIndex = 0;
    }

    public override NodeState Evaluate()
    {
        if (!controller.IsStunned())
        {
            if (patrolPath.Count == 0 || currentPatrolIndex >= patrolPath.Count)
            {
                // Se não há caminho ou chegamos ao final, recalculamos o caminho
                CalculateNewPatrolPath();
            }

            MoveToNextPatrolPoint();
        }

        state = NodeState.RUNNING;
        return state;
    }

    private void CalculateNewPatrolPath()
    {
        Vector3Int startCell = controller.rb.position;
        Vector3Int randomPatrolPoint = GetRandomPatrolPoint();

        patrolPath = pathfinding.FindPath(startCell, randomPatrolPoint, controller.grid);
        currentPatrolIndex = 0;
    }

    private void MoveToNextPatrolPoint()
    {
        Vector3Int nextCell = patrolPath[currentPatrolIndex];
        Vector3 moveDirection = (controller.grid.GetCellCenterWorld(nextCell) - controller.rb.position).normalized;

        controller.rb.velocity = moveDirection * controller.moveSpeed;

        // Verifica se chegou ao próximo ponto de patrulha
        if (Vector3.Distance(controller.rb.position, controller.grid.GetCellCenterWorld(nextCell)) < 0.1f)
        {
            // Move imediatamente para o próximo ponto
            controller.rb.position = controller.grid.GetCellCenterWorld(nextCell);
            currentPatrolIndex++;
        }
    }

    private Vector3Int GetRandomPatrolPoint()
    {
        // Defina os limites da área de patrulha
        int minX = 0;
        int maxX = 10;
        int minY = 0;
        int maxY = 10;

        // Gere coordenadas x e y aleatórias dentro dos limites
        int randomX = Random.Range(minX, maxX);
        int randomY = Random.Range(minY, maxY);

        // Crie um Vector3Int com as coordenadas aleatórias
        Vector3Int randomPatrolPoint = new Vector3Int(randomX, randomY, 0);

        return randomPatrolPoint;
    }

>>>>>>> Stashed changes
}
