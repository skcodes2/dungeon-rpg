using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    public GameObject bossObject; // Assign the boss GameObject in the Inspector

    private static bool bossSpawned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (bossSpawned) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered boss activation");

            bossObject.SetActive(true); // Activate the boss object directly without using spawn point
            bossSpawned = true;

            DisableAllTriggers();
        }
    }

    void DisableAllTriggers()
    {
        GameObject[] triggers = GameObject.FindGameObjectsWithTag("BossTrigger");
        foreach (GameObject trigger in triggers)
        {
            trigger.SetActive(false);
        }
        Debug.Log("Disabled all boss triggers");
    }
}
