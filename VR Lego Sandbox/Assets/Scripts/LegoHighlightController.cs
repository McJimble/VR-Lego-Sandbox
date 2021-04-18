using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoHighlightController : MonoBehaviour
{
    [SerializeField] private GameObject hasChangingMaterial;
    [SerializeField] private Renderer highlightRenderer;

    private Color currentColor;

    private void Awake()
    {
        highlightRenderer = hasChangingMaterial.GetComponent<Renderer>();
        if (highlightRenderer == null)
            Destroy(this.gameObject);

    }

    // Change sharedMaterial from hasChangingMaterial's renderer every
    // fixedTimestep, interpolating between white and black.
    private void FixedUpdate()
    {
        currentColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        highlightRenderer.sharedMaterial.SetColor("_OutlineColor", currentColor);
    }
}
