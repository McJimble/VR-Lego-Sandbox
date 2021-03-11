using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : MonoBehaviour
{
    // All legos use a set width and length. Height may vary, but size must be
    // constant for groups of them to properly resize.
    public const float sqSize = 0.4f;

    // All unique lego prefabs have a varying height that should be defined
    // in the inspector, so as to avoid conflicts b/t models and localScale changes.
    public float height = 0.4f;

    // Lego group of this lego. Must be assigned in inspector before
    // runtime for preset lego prefabs to work!!
    [SerializeField] private LegoGroup attachedGroup;

    // Collider from interactable prefab's meshContainer (ideally)
    [SerializeField] private BoxCollider attachedCollider;

    [SerializeField] private GameObject snapPinObject;
    private Renderer snapPinRenderer;

    // Element in array of current lego group
    [SerializeField] private int groupX;
    [SerializeField] private int groupY;

    private Material legoMaterial;
    private Renderer legoRenderer;

    public Material LegoMaterial
    {
        get 
        {
            if (legoMaterial == null)
                legoMaterial = LegoRenderer.material;
            return legoMaterial; 
        }
    }

    public Renderer LegoRenderer
    {
        get
        {
            if (legoRenderer == null)
                legoRenderer = GetComponentInChildren<Renderer>();
            return legoRenderer;
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

    public BoxCollider AttachedCollider
    { 
        get 
        { 
            if (attachedCollider == null)
            {
                attachedCollider = GetComponentInChildren<BoxCollider>();
            }
            return attachedCollider; 
        }
        private set
        {
            attachedCollider = value;
        }
    }

    private void Awake()
    {
        //Debug.Log(transform.TransformPoint(Vector3.zero));
        AttachedCollider.isTrigger = true;
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
        LegoMaterial.color = LegoMaterial.color;
        snapPinRenderer.material = LegoMaterial;
    }

    private void OnDestroy()
    {
        Destroy(LegoMaterial);
    }
}
