using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;

/// <summary>
/// Stores all necessary information about a group of legos that are parented to
/// the same GameObject
/// </summary>
[RequireComponent(typeof(BoxCollider), typeof(AudioLego))]
public class LegoGroup : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField] private bool largerRefreshSize;
    [SerializeField] private Vector2Int testSize;
    public Color testColor;
    public bool lockRotation = false;

    [Header("Required Componenets")]
    // This is the base lego piece that will be duplicated and placed to
    // create the illusion of a whole lego that is changing in size.
    [SerializeField] private Lego baseLegoData;
    [SerializeField] private BoxCollider groupCollider;
    [SerializeField] private GameObject interactableInternalContainer;
    private InteractableFacade interactableFacade;
    private AudioLego audioFX;
    private Rigidbody mainRB;

    [Header("Data")]
    // 2D array of all legoPieces currently active on this group.
    private Lego[,] legoPieces;

    // Dictionary of connected legos (using their ID) and their fixed joint components.
    public Dictionary<int, FixedJoint> connectedJoints = new Dictionary<int, FixedJoint>();
    public List<LegoGroup> connectedGroups = new List<LegoGroup>();
    public List<Vector2Int> disabledLegoElements = new List<Vector2Int>();

    // Is group initialized with an existing base piece?
    private bool initialized = false;
    
    private static int currGroupID = 0;
    private int groupID;

    public int GroupID
    {
        get { return groupID; }
    }

    public Lego BaseLegoData
    {
        get { return baseLegoData; }
    }

    public AudioLego AudioFX
    {
        get { return audioFX; }
    }

    public Rigidbody MainRB
    {
        get { return mainRB; }
    }

    // Returns number of 1x1 legos in the group
    public Vector2Int GroupSize
    {
        get
        {
            return new Vector2Int(legoPieces.GetLength(0), legoPieces.GetLength(1));
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void ToggleInteractions(bool active)
    { 
        if(active)
        {
            interactableFacade.enabled = true;
            interactableInternalContainer.SetActive(true);
        }
        else
        {
            interactableFacade.enabled = false;
            interactableInternalContainer.SetActive(false);
        }
    }

    public void SetKinematic(bool active)
    {
        mainRB.isKinematic = active;
        if (active) ResetRotation();
    }  

    // Adds a new group as a neighbor, and creates a fixed joint for this lego.
    // Also adds the data to the newGroup, but WITHOUT adding a fixed joint component.
    public void AddConnectedLego(LegoGroup newGroup)
    {
        newGroup.connectedGroups.Add(this);
        this.connectedGroups.Add(newGroup);

        FixedJoint newJoint = gameObject.AddComponent<FixedJoint>();
        newJoint.connectedBody = newGroup.mainRB;
        newJoint.autoConfigureConnectedAnchor = true;

        newGroup.connectedJoints.Add(this.GroupID, newJoint);
        this.connectedJoints.Add(newGroup.GroupID, newJoint);

        newGroup.ToggleInteractions(false);
        this.ToggleInteractions(false);

        foreach (var lego in SnapManager.Instance.GetHoveredLegos())
        {
            disabledLegoElements.Add(lego.GroupElement);
            lego.SnapZoneTrigger.enabled = false;

            //For testing what is disabled:
            lego.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        audioFX = GetComponent<AudioLego>();
        interactableFacade = GetComponent<InteractableFacade>();

        if (interactableInternalContainer == null)
            interactableInternalContainer = transform.Find("Internal").gameObject;

        mainRB = GetComponent<Rigidbody>();
        if (mainRB.isKinematic)
            ToggleInteractions(false);

    }

    private void Start()
    {
        if (baseLegoData == null)
        {
            baseLegoData = GetComponentInChildren<Lego>();
        }

        // Seems stupid, but this makes Unity duplicate the current material so that
        // all clones of this base lego can be changed using ONE call of sharedMaterial
        // instead of looping through the entire group again.
        baseLegoData.RefreshLegoMaterial();
        testColor = baseLegoData.LegoRenderer.material.color;
        InitGroup();
        //StartCoroutine(TestRefresh());

        // Set Unique id for this group.
        groupID = ++currGroupID;
    }

    private void Update()
    {
        ChangeGroupColor(testColor);
        //if(Input.GetKeyDown(KeyCode.X))
        //{
        //    RefreshLegoGroup(testSize.x - 1, testSize.y - 1);
        //}
    }

    private void FixedUpdate()
    {
        if(lockRotation)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    public IEnumerator TestRefresh()
    {
        int change = 1 * ((largerRefreshSize) ? 2 : -1);
        yield return new WaitForSeconds(2.5f);
        RefreshLegoGroup(legoPieces.GetLength(0) + change, legoPieces.GetLength(1) + change);
    }

    // Initializes this group of legos for later.
    public void InitGroup()
    {
        this.groupCollider = GetComponent<BoxCollider>();

        // Start 2d array as one element with legoData.
        legoPieces = new Lego[1, 1] { { baseLegoData } };
        baseLegoData.transform.parent = this.transform;
        baseLegoData.transform.position = this.transform.TransformPoint(Vector3.zero);

        initialized = true;
        RefreshLegoGroup(testSize.x, testSize.y);
    }

    /// <summary>
    /// Places the LegoGroup gameObject at the center of all currently active
    /// legos in the group. Also refreshes group box collider and the Lego[,] array with new objects.
    /// </summary>
    /// <param name="newX"> Number of legos for length of group
    /// </param>
    /// <param name="y"> Number of legos for width of group
    /// </param>
    public void RefreshLegoGroup(int newX, int newY)
    {
        if (!initialized || newX <= 0 || newY <= 0) return;

        // Delete legos that are outside of new length and width.
        int oldX = legoPieces.GetLength(0);
        int oldY = legoPieces.GetLength(1);

        // Set the attached group of all legos to this,
        // which also makes this group a parent object of them.
        for (int i = 0; i < oldX; i++)
        {
            for (int j = 0; j < oldY; j++)
            {
                legoPieces[i, j].transform.parent = null;
            }
        }

        // Delete entire row if newX is less the oldX
        for (int i = newX; i < oldX; i++)
        {
            for (int j = 0; j < oldY; j++)
            {
                // If lagging from destroying, create object pooling system?
                Destroy(legoPieces[i, j].gameObject);
                legoPieces[i, j] = null;
                Debug.LogWarning("DELETING (" + i + ", " + j + ") LEGO ON " + gameObject.name);
            }
        }

        // Delete entire Column if newY is less than oldY
        for (int j = newY; j < oldY; j++)
        {
            for (int i = 0; i < oldX; i++)
            {
                // If lagging from destroying, create object pooling system?
                if(legoPieces[i, j] != null) Destroy(legoPieces[i, j].gameObject);
                Debug.LogWarning("DELETING (" + i + ", " + j + ") LEGO ON " + gameObject.name);
            }
        }

        // Refresh Lego Array
        Lego [,] newLegoPieces = new Lego[newX, newY];
        newLegoPieces[0, 0] = baseLegoData;

        baseLegoData.transform.parent = null;
        // Spawn and position new lego prefabs.
        for (int i = 0; i < newX; i++)
        {
            for (int j = 0; j < newY; j++)
            {
                // 0,0 prefab will remain the same.
                if ((i == 0 && j == 0)) continue;

                if (i < oldX && j < oldY)
                {
                    newLegoPieces[i, j] = legoPieces[i, j];
                }
                else
                {
                    // Set new instance of lego component
                    newLegoPieces[i, j] = Instantiate(baseLegoData.gameObject).GetComponent<Lego>();

                    // Set x and y value for piece and offset position accordingly.
                    newLegoPieces[i, j].SetGroupElement(i, j);
                    Vector3 setPos = baseLegoData.transform.TransformPoint(Vector3.zero);
                    setPos += (baseLegoData.transform.right * Lego.sqSize * i);
                    setPos += (baseLegoData.transform.forward * Lego.sqSize * j);

                    newLegoPieces[i, j].transform.position = setPos;
                }
            }
        }

        // New array created, overwrite legoPieces now.
        legoPieces = newLegoPieces;

        // Adjust legoGroup position to center of group.
        Vector3 newPos = legoPieces[0,0].transform.TransformPoint(Vector3.zero);
        float addMagnitudeX = Lego.sqSize * (newX / 2);
        addMagnitudeX += (newX % 2 == 0) ? -(Lego.sqSize / 2f) : 0f;
        newPos += baseLegoData.transform.right * addMagnitudeX;

        float addMagnitudeY = Lego.sqSize * (newY / 2);
        addMagnitudeY += (newY % 2 == 0) ? -(Lego.sqSize / 2f) : 0f;
        newPos += baseLegoData.transform.forward * addMagnitudeY;

        transform.position = newPos;

        // Set the attached group of all legos to this,
        // which also makes this group a parent object of them.
        for (int i = 0; i < newX; i++)
        {
            for (int j = 0; j < newY; j++)
            {
                legoPieces[i, j].transform.parent = this.transform;
            }
        }

        // Adjust boxCollider size to cover all new legos.
        Vector3 newSize = new Vector3(Lego.sqSize * newX, baseLegoData.height, Lego.sqSize * newY);
        groupCollider.size = newSize;
        //Debug.Log("New size of " + gameObject.name + ": " + newX + ", " + newY);
    }

    public void ChangeGroupColor(Color newColor)
    {
        baseLegoData.LegoRenderer.material.color = newColor;
    }

    // Changes the group's physics layer to one that interacts all the same as the default one,
    // but it doesn't collide with other lego groups. This is to avoid pushing legos when
    // attempting to place one on top of another.
    public void ToggleGroupCollision(bool active)
    {
        if(active)
        {
            gameObject.layer = LayerMask.NameToLayer("Parent Lego");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Ghost Parent Lego");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(SnapManager.Instance.GetAttacher() != null)
        {
            if (other.CompareTag("Lego") && SnapManager.Instance.GetAttacher().GroupID == this.GroupID)
            {
                Lego checkForLego;
                if (other.TryGetComponent<Lego>(out checkForLego))
                {
                    if (checkForLego.AttachedGroup.GroupID == this.GroupID)
                        return;

                    SnapManager.Instance.TrySetAttachedTo(checkForLego.AttachedGroup);
                    SnapManager.Instance.AddHoveredLego(checkForLego);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(SnapManager.Instance.GetAttacher() != null)
        {
            if (other.CompareTag("Lego") && SnapManager.Instance.GetAttacher().GroupID == this.GroupID)
            {
                Lego checkForLego;
                if (other.TryGetComponent<Lego>(out checkForLego))
                {
                    SnapManager.Instance.RemoveHoveredLego(checkForLego);
                }
            }
        }
    }
}
