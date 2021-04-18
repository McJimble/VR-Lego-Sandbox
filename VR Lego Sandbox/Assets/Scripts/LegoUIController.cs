using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoUIController : Singleton<LegoUIController>
{
    [SerializeField] private Transform rayCastPoint;
    [SerializeField] private GameObject defaultCanvas;
    [Header("Alt canvas properties")]
    [SerializeField] private GameObject alternateCanvas;
    [SerializeField] private GameObject disallowedSelectionButtons;
    [SerializeField] private GameObject allowSelectionButtons;

    [Header("Debug")]
    [SerializeField] private LegoGroup selectedGroup;
    [SerializeField] private int selectedLegoX;
    [SerializeField] private int selectedLegoY;
    [SerializeField] private testspawn cloneSpawner;


    // Attempts to find a LegoGroup object to select
    public void CastRayToGroup()
    {
        LayerMask legoMask = LayerMask.GetMask("Parent Lego", "Default");
        RaycastHit hitInfo;
        Physics.Raycast(rayCastPoint.position, rayCastPoint.forward, out hitInfo, 10f, legoMask);

        LegoGroup tryForGroup;
        if (hitInfo.transform == null)
        {
            ToggleAltCanvas(false);
            return;
        }
        else if(!hitInfo.transform.CompareTag("LegoGroup") && !hitInfo.transform.CompareTag("WorldUI"))
        {
            // Have to do two separate ifs because it could otherwise throw an error
            // for the transform being null in the above check.
            ToggleAltCanvas(false);
            return;
        }
        if (hitInfo.transform.TryGetComponent<LegoGroup>(out tryForGroup))
        {
            ToggleAltCanvas(true);
            defaultCanvas.GetComponent<AudioCanvas>().ButtonClick();
            SetSelectedLego(tryForGroup);
            RefreshAlternateUI();
        }
    }

    public void SetSelectedLego(LegoGroup selectedGroup)
    {
        if(selectedGroup != null)
        {
            this.selectedGroup = selectedGroup;
            this.selectedGroup.ToggleLegoHighlights(true);
            selectedLegoX = selectedGroup.GroupSize.x;
            selectedLegoY = selectedGroup.GroupSize.y;
        }
        else
        {
            this.selectedGroup = null;
            selectedLegoX = 0;
            selectedLegoY = 0;
        }
    }

    public void RefreshAlternateUI()
    {
        if (selectedGroup == null)
            return;

        SetSelectedLego(selectedGroup);
        if(selectedGroup.AllowSelection)
        {
            allowSelectionButtons.SetActive(true);
            disallowedSelectionButtons.SetActive(false);
        }
        else
        {
            allowSelectionButtons.SetActive(false);
            disallowedSelectionButtons.SetActive(true);
        }
    }

    public void ToggleAltCanvas(bool active)
    {
        if(active)
        {
            alternateCanvas.SetActive(true);
            defaultCanvas.SetActive(false);
            ToggleSelectedHighlights(false);
        }
        else
        {
            alternateCanvas.SetActive(false);
            defaultCanvas.SetActive(true);
            ToggleSelectedHighlights(false);
            this.selectedGroup = null;
        }
    }

    // Changes color to the material on the given object's renderer
    public void ChangeSelectedGroupColor(Material materialForColor)
    {
        if (selectedGroup == null) return;
        selectedGroup.ChangeGroupColor(materialForColor.color);
    }

    public void DeleteSelectedLego()
    {
        Debug.Log("Deleting");
        if (selectedGroup == null) return;
        selectedGroup.DestroyLegoObject();
        // TODO: CREATE DESTROY FUNCTION ON LEGO ITSELF, SO PREVIOUSLY USED SNAP POINTS CAN BE RETURNED.
        selectedGroup = null;
    }

    public void ChangeSelectedGroupX(bool increase)
    {
        int newX = selectedLegoX + ((increase) ? 1 : -1);
        if (newX <= 0) return;
        ChangeLegoSize(newX, selectedLegoY);
        selectedLegoX = newX;
    }

    public void ChangeSelectedGroupY(bool increase)
    {
        int newY = selectedLegoY + ((increase) ? 1 : -1);
        if (newY <= 0) return;
        ChangeLegoSize(selectedLegoX, newY);
        selectedLegoY = newY;
    }

    public void ChangeLegoSize(int newX, int newY)
    {
        selectedGroup.RefreshLegoGroup(newX, newY);
        // Update displayed text once you add this.
    }

    public void SetSelectedToKinematic()
    {
        if (selectedGroup == null)
            return; 

        selectedGroup.SetKinematic(true);
        RefreshAlternateUI();
    }

    public void ToggleSelectedHighlights(bool active)
    {
        if(selectedGroup != null)
        {
            selectedGroup.ToggleLegoHighlights(active);
        }
    }

    public void CloneSelectedLego()
    {
        if (selectedGroup == null)
            return;

        cloneSpawner.CloneGroup(selectedGroup);
    }
}
