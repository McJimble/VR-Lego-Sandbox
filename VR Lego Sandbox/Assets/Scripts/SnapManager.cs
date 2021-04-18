using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SnapManager : Singleton<SnapManager>
{
    [SerializeField] private LegoGroup attacherGroup;
    [SerializeField] private LegoGroup attachedToGroup;

    // The legos we are hovering over with currently held lego.
    [SerializeField] private List<Lego> hoveredLegos = new List<Lego>();

    private Renderer visualsRenderer;

    // The following Getters and setters for the two groups are below
    // For the sake of getting and setting them inside unity events.
    public LegoGroup GetAttacher()
    {
        return attacherGroup;
    }

    public void TrySetAttacher(GameObject newGroup)
    {
        LegoGroup checkForGroup = newGroup.GetComponent<LegoGroup>();
        if (checkForGroup != null)
        {
            attacherGroup = checkForGroup;
            attacherGroup.ToggleLegoHighlights(true);
        }
    }

    public void TrySetAttacher(LegoGroup newGroup)
    {
        if (attacherGroup != null)
            return;
        attacherGroup = newGroup;
        attacherGroup.ToggleLegoHighlights(true);
    }

    public LegoGroup GetAttachedTo()
    {
        return attachedToGroup;
    }

    public void TrySetAttachedTo(GameObject newGroup)
    {
        // Only allow one lego to attach to at a time.
        if (attachedToGroup != null)
            return;

        LegoGroup checkForGroup = newGroup.GetComponent<LegoGroup>();
        if (checkForGroup != null)
        {
            attachedToGroup = checkForGroup;
        }
    }

    public void TrySetAttachedTo(LegoGroup newGroup)
    {
        // Only allow one lego to attach to at a time.
        if (attachedToGroup != null)
            return;

        attachedToGroup = newGroup;
    }

    public void ClearAttacher()
    {
        if(attacherGroup != null)
        {
            attacherGroup.ToggleLegoHighlights(false);
        }
        attacherGroup = null;
        ClearAttachedTo();
        ClearHoveredLegos();
        transform.position = new Vector3(0, 2, 0);
    }

    public void ClearAttachedTo()
    {
        attachedToGroup = null;
        ClearHoveredLegos();
    }

    public void ClearHoveredLegos()
    {
        hoveredLegos.Clear();
    }

    public void AddHoveredLego(Lego newLego)
    {
        if (attachedToGroup == null || attacherGroup == null)
            return;

        if (newLego.AttachedGroup.GroupID == attachedToGroup.GroupID)
            hoveredLegos.Add(newLego);
        RefreshSnapHighlight();
    }

    public void RemoveHoveredLego(Lego removedLego)
    {
        if (hoveredLegos.Contains(removedLego))
            hoveredLegos.Remove(removedLego);

        if (hoveredLegos.Count == 0)
            ClearAttachedTo();
        RefreshSnapHighlight();
    }

    public List<Lego> GetHoveredLegos()
    {
        return hoveredLegos;
    }

    protected override void Awake()
    {
        base.Awake();
        visualsRenderer = GetComponent<Renderer>();
        visualsRenderer.enabled = true;
    }

    private void FixedUpdate()
    {
        
    }

    public void AttemptPlaceLego()
    {
        if(attacherGroup != null  && attachedToGroup != null)
        {
            attacherGroup.transform.position = this.transform.position;
            attacherGroup.transform.rotation = this.transform.rotation;

            attacherGroup.AddConnectedLego(attachedToGroup);
            LegoUIController.Instance.RefreshAlternateUI();
        }
        ClearAttacher();
    }

    // Sets renderer to active, but first adjusts the scale and location based on the size of
    // the Lego group it will mimic (ideally the lego currently held by player).
    private void RefreshSnapHighlight()
    {
        if (hoveredLegos.Count > 0 && attacherGroup != null && attachedToGroup != null)
        {
            Vector3 attacherRot = attacherGroup.transform.rotation.eulerAngles;
            Vector3 attachedToRot = attachedToGroup.transform.rotation.eulerAngles;

            if (attacherRot.y > 180f)
                attacherRot.y = -(360f - attacherRot.y);
            float yRot = attacherRot.y / 45f;
            float testRot = yRot;
            if (Mathf.Abs(yRot) <= 1f)
                yRot = 0f;
            else if (Mathf.Abs(yRot) >= 3f)
                yRot = 180f;
            else
                yRot = 90 * Mathf.Sign(yRot);

            Debug.Log("Calculated y-rotation: " + yRot + "FROM" + attacherRot.y);
            transform.rotation = Quaternion.Euler(attachedToRot.x, yRot, attachedToRot.z);
            transform.localScale = new Vector3(Lego.sqSize * attacherGroup.GroupSize.x, attacherGroup.BaseLegoData.height, Lego.sqSize * attacherGroup.GroupSize.y);
            transform.position = hoveredLegos[0].transform.position;  // <--- JUST FOR TESTING VISUALS, REMOVE LATER.

            float heightIncrease = (attachedToGroup.BaseLegoData.height + attacherGroup.BaseLegoData.height) / 2f;
            transform.position += new Vector3(Lego.sqSize/2f, heightIncrease, -Lego.sqSize);
        }
        else
        {
            transform.position = new Vector3(0, 2, 0);
        }
    }
}
