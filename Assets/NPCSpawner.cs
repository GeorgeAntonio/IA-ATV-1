using Cainos.PixelArtTopDown_Basic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    private GameObject mainCamera, dollNPC;
    public GameObject npcPrefab; // The NPC prefab to spawn
    public Transform[] spawnPoints; // An array of spawn points for the new NPCs
    public bool isTargetDead;

    private void Start()
    {
        // Subscribe to the event when an NPC is destroyed
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        NPC.OnNPCDestroyed += SpawnNewNPC;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        NPC.OnNPCDestroyed -= SpawnNewNPC;        
    }

    private void SpawnNewNPC(Vector3 position)
    {
        // Choose a random spawn point from the array
        if (spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPosition = spawnPoints[randomIndex].position;

            // Spawn a new NPC at the chosen position
            dollNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            if (mainCamera.GetComponent<CameraFollow>().target.gameObject) { mainCamera.GetComponent<CameraFollow>().target = dollNPC.transform; }
            isTargetDead = false;
        }
    }
}
