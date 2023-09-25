using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // PRIVATE
    private GameObject player;
    private PlayerController pController;
    private bool gameOn;
    private int[,] map;
    private int[] lastPlayerPosition;
    private List<GameObject> enemies;
    

    // PUBLIC

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        map = new int[3,3];
        lastPlayerPosition = null;
        player = GameObject.FindGameObjectWithTag("Player");
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        pController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        while (gameOn)
        {
            getPlayerPosition();
        }
        if(pController.isAlive() == false || isEnemiesAlive() == false)
        {
            gameOn = false;
        }
    }
    void changeMapPosition(int x, int y, int value)
    {
        map[x, y] = value;
    }
    void getPlayerPosition()
    {
        lastPlayerPosition = pController.getPosition();
    }

    bool isEnemiesAlive()
    {
        return enemies.Count > 0;
    }
}
