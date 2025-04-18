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
            PlayerPrefs.SetString("LastSpawnPoint", spawnPointTag); // Save spawn point
            PlayerPrefs.Save(); // Ensure it's written immediately

            print("DoorTrigger: Loading scene " + sceneToLoad);
            

            // Load the next scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }

   
}
