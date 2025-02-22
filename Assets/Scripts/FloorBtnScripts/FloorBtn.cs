using UnityEngine;

public class FloorBtn : MonoBehaviour
{
    private bool isActivated = false;
    private SpriteRenderer spriteRenderer;
    
    public Sprite unpressedSprite;  // Default button sprite
    public Sprite pressedSprite;    // Pressed button sprite

    public delegate void ButtonActivated();
    public static event ButtonActivated OnButtonActivated;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unpressedSprite; // Set initial sprite
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            ActivateButton();
        }
    }

    private void ActivateButton()
    {
        isActivated = true;
        spriteRenderer.sprite = pressedSprite; // Change sprite to pressed version
        OnButtonActivated?.Invoke(); // Notify ButtonManager
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
