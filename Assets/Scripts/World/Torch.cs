using UnityEngine;
using UnityEngine.Rendering.Universal;
public class Torch : MonoBehaviour
{
    private bool isLit = false;

    public Animator animator;
    public Light2D torchLight;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isLit = true;
            animator.SetTrigger("Burn");
            torchLight.enabled = true;

        }
    }

    void Start()
    {
        torchLight.enabled = false;
    }


}