using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : MonoBehaviour
{
    // All legos use a set width and length. Height may vary, but size must be
    // constant for groups of them to properly resize.
    public const float sqSize = 0.4f;
    public const float snapBoxHeight = 0.2f;

    // All unique lego prefabs have a varying height that should be defined
    // in the inspector, so as to avoid conflicts b/t models and localScale changes.
    public float height = 0.4f;

    // Lego group of this lego. Must be assigned in inspector before
    // runtime for preset lego prefabs to work!!
    [SerializeField] private LegoGroup attachedGroup;

    // Collider from interactable prefab's meshContainer (ideally)
    // Acts as a snapZoneTrigger area for detected legos to attach to the main group.
    [SerializeField] private BoxCollider snapZoneTrigger;

    [SerializeField] private GameObject mainBrickObject;
    [SerializeField] private GameObject snapPinObject;

    // Element in array of current lego group
    [SerializeField] private int groupX;
    [SerializeField] private int groupY;

    private Renderer snapPinRenderer;
    private Renderer mainLegoRenderer;

    public Vector3 DefaultSnapZoneSize
    {
        get
        {
            return new Vector3(sqSize, snapBoxHeight, sqSize);
        }
    }

    public Vector3 DefaultSnapZoneOffset
    {
        get
        {
            return new Vector3(0, (snapBoxHeight + height) * 0.5f, 0);
        }
    }

    public Renderer LegoRenderer
    {
        get
        {
            if (mainLegoRenderer == null)
                mainLegoRenderer = mainBrickObject.GetComponent<Renderer>(); ;
            return mainLegoRenderer;
        }
    }

    // If the elements of width and height in group are both
    // zero, this is the base lego of that group.
    public bool IsBaseOfGroup
    {
        get { return (groupX == 0 && groupY == 0); }
    }

    public LegoGroup AttachedGroup
    {
        get
        {
            return attachedGroup;
        }
        set
        {
            attachedGroup = value;
        }
    }

    public BoxCollider SnapZoneTrigger
    { 
        get 
        {
            if (snapZoneTrigger == null)
            {
                Debug.LogWarning("Please assign an attached collider in the inspector!");
                snapZoneTrigger = GetComponent<BoxCollider>();
            }
            return snapZoneTrigger; 
        }
        private set
        {
            snapZoneTrigger = value;
        }
    }

    public Vector2Int GroupElement
    {
        get { return new Vector2Int(groupX, groupY); }
    }

    private void Awake()
    {
        // Initialize SnapZone Trigger if collider for one exists
        if (SnapZoneTrigger == null)
            Destroy(this.gameObject);
        SnapZoneTrigger.isTrigger = true;
        SnapZoneTrigger.size = DefaultSnapZoneSize;
        SnapZoneTrigger.center = DefaultSnapZoneOffset;

        mainLegoRenderer = mainBrickObject.GetComponent<Renderer>();
        snapPinRenderer = snapPinObject.GetComponent<Renderer>();
    }

    public void SetGroupElement(int x, int y)
    {
        this.groupX = x;
        this.groupY = y;
    }

    // Duplicates this object's material and assigns that copy to the
    // cylindrical snap point object as well. This allows us to change the
    // material of any copy of this lego by changing LegoRenderer.sharedMaterial.
    public void RefreshLegoMaterial()
    {
        LegoRenderer.material = new Material(mainLegoRenderer.material);
        snapPinRenderer.material = mainLegoRenderer.material;
    }

    private void OnDestroy()
    {
        Destroy(mainLegoRenderer.material);
    }
}
