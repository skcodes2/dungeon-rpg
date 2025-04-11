using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Cinemachine; // Correct namespace for Cinemachine 3.x
using System.Collections; // Required for IEnumerator / Coroutines

public class DoorTrigger : MonoBehaviour
{
    public string sceneToLoad;  // Scene name to load
    public string spawnPointTag; // The tag of the spawn point in the next scene

    [SerializeField]
    private List<DialogueTrigger> dialogueTriggers;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("DoorTrigger: Saving spawn point " + spawnPointTag + " before loading scene " + sceneToLoad);
            PlayerPrefs.SetString("LastSpawnPoint", spawnPointTag); // Save spawn point
            PlayerPrefs.Save(); // Ensure it's written immediately

            // Subscribe to the sceneLoaded event to move the player after the scene loads
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Load the next scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe from the event to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if(sceneToLoad != "Room1")return;
        // Get the last saved spawn point tag
        

        if (!string.IsNullOrEmpty(spawnPointTag))
        {
            // Find the spawn point by its tag
            GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);

            if (spawnPoint != null)
            {
                // Find the player object and move it to the spawn point
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawnPoint.transform.position;
                    CinemachineCamera cineCam = FindObjectOfType<CinemachineCamera>();
                    if (cineCam != null && player != null)
                    {
                        cineCam.Follow = player.transform;

                        Debug.Log("🎥 CinemachineCamera is now following the player.");
                    }
                    Debug.Log($"DoorTrigger: Moved player to spawn point '{spawnPointTag}' in scene '{scene.name}'.");
                }
                else
                {
                    Debug.LogError("DoorTrigger: Player object not found in the scene!");
                }
            }
            else
            {
                Debug.LogError($"DoorTrigger: Spawn point with tag '{spawnPointTag}' not found in scene '{scene.name}'.");
            }
        }
        else
        {
            Debug.LogWarning("DoorTrigger: No spawn point tag found in PlayerPrefs.");
        }
    }
}
