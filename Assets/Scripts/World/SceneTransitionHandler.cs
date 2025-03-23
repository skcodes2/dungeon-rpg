using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine; // Correct namespace for Cinemachine 3.x
using System.Collections; // Required for IEnumerator / Coroutines

public class SceneTransitionHandler : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start the coroutine to delay the player/camera setup
        StartCoroutine(SetupAfterSceneLoad());
    }

    private IEnumerator SetupAfterSceneLoad()
    {
        // Wait one frame so the new scene fully loads its objects
        yield return null;

        // Find player and spawn point in the new scene
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.Find("SpawnPoint");

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
            Debug.Log("✅ Player moved to spawn point.");
        }
        else
        {
            Debug.LogWarning("⚠️ Player or SpawnPoint not found!");
        }

        // Wait one more frame in case the camera loads later
        yield return null;

        // Find the Cinemachine camera
        CinemachineCamera cineCam = FindObjectOfType<CinemachineCamera>();
        if (cineCam != null && player != null)
        {
            cineCam.Follow = player.transform;

            Debug.Log("🎥 CinemachineCamera is now following the player.");
        }
        else
        {
            Debug.LogWarning("⚠️ CinemachineCamera or player not found.");
        }
    }
}
