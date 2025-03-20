using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class SecretStairs : MonoBehaviour
{
    private bool playerIsNearby = false;
    public string treasureRoomScene = "TreasureRoom"; // Set this to your Treasure Room scene name

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {  
            playerIsNearby = true; // Player is near the stairs
            Debug.Log("Press 'E' to enter the Treasure Room.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false; // Player moved away from stairs
        }
    }

    private void Update()
    {
        if (playerIsNearby && Input.GetKeyDown(KeyCode.E))
        {
            EnterTreasureRoom();
        }
    }

    private void EnterTreasureRoom()
    {
        Debug.Log("Player entered the Treasure Room!");
        SceneManager.LoadScene(treasureRoomScene); // Load the Treasure Room scene
    }
}
