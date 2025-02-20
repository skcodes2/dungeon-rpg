using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        // Check if the game is starting fresh (not transitioning from another scene)
        if (!PlayerPrefs.HasKey("LastSpawnPoint"))
        {
            Debug.Log("Spawn Manager: First game start detected. Keeping default player position.");
            return; 
        }

        // Retrieve the last saved spawn point
        string lastSpawnPoint = PlayerPrefs.GetString("LastSpawnPoint", "");

        // If the last spawn point is empty, just keep the player at their default position
        if (string.IsNullOrEmpty(lastSpawnPoint))
        {
            Debug.Log("Spawn Manager: No previous scene transition detected. Keeping default position.");
            return; 
        }

        // Find the spawn point in the scene
        GameObject spawnPoint = GameObject.FindWithTag(lastSpawnPoint);

        if (spawnPoint != null)
        {
            Debug.Log("Spawn Manager: Moving player to " + spawnPoint.transform.position);
            transform.position = spawnPoint.transform.position; // Move player
        }
        else
        {
            Debug.LogWarning("Spawn Manager: No spawn point found with tag " + lastSpawnPoint);
        }
    }
}
