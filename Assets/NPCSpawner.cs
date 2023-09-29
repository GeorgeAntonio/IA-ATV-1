using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // The NPC prefab to spawn
    public Transform[] spawnPoints; // An array of spawn points for the new NPCs

    private void Start()
    {
        // Subscribe to the event when an NPC is destroyed
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
            Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
