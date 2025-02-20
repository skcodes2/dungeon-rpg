using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string sceneToLoad;  // Scene name to load
    public string spawnPointTag; // The tag of the spawn point in the next scene

    private void Start()
    {
        // If this is the first scene (SampleScene), clear PlayerPrefs to prevent wrong spawns
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Debug.Log("DoorTrigger: Resetting spawn point at game start.");
            PlayerPrefs.DeleteKey("LastSpawnPoint");
            PlayerPrefs.Save();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("DoorTrigger: Saving spawn point " + spawnPointTag + " before loading scene " + sceneToLoad);
            PlayerPrefs.SetString("LastSpawnPoint", spawnPointTag); // Save spawn point
            PlayerPrefs.Save(); // Ensure it's written immediately
            SceneManager.LoadScene(sceneToLoad); // Load the next scene
        }
    }
}
