using UnityEngine;

public class UnflipUI : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private Vector3 initialLocalScale;

    void Start()
    {
        // Store the original offset and scale
        initialLocalPosition = transform.localPosition;
        initialLocalScale = transform.localScale;
    }

    void LateUpdate()
    {
        float flip = Mathf.Sign(transform.parent.localScale.x);

        // Keep position fixed relative to parent
        transform.localPosition = new Vector3(
            initialLocalPosition.x * flip,
            initialLocalPosition.y,
            initialLocalPosition.z
        );

        // Undo the horizontal flip for the UI element
        transform.localScale = new Vector3(
            initialLocalScale.x * flip,
            initialLocalScale.y,
            initialLocalScale.z
        );
    }
}
