using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all necessary information about a group of legos that are parented to
/// the same GameObject
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class LegoGroup : MonoBehaviour
{
    // This is the base lego piece that will be duplicated and placed to
    // create the illusion of a whole lego that is changing in size.
    [SerializeField] private Lego baseLegoData;
    [SerializeField] private BoxCollider groupCollider;

    // 2D array of all legoPieces currently active on this group.
    private Lego[,] legoPieces;

    // Is group initialized with an existing base piece?
    private bool initialized = false;

    [SerializeField] private Vector2Int testSize;
    public Color testColor;

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
    }

    private void Update()
    {
        ChangeGroupColor(testColor);
        //if(Input.GetKeyDown(KeyCode.X))
        //{
        //    RefreshLegoGroup(testSize.x - 1, testSize.y - 1);
        //}
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

        for (int i = newX; i < oldX; i++)
        {
            for (int j = newY; j < oldY; j++)
            {
                // If lagging from destroying, create object pooling system?
                Destroy(legoPieces[i, j].gameObject);
            }
        }

        // Refresh Lego Array
        legoPieces = new Lego[newX, newY];
        legoPieces[0, 0] = baseLegoData;

        baseLegoData.transform.parent = null;
        // Spawn and position new lego prefabs.
        for (int i = 0; i < newX; i++)
        {
            for (int j = 0; j < newY; j++)
            {
                // 0,0 prefab will remain the same.
                if ((i == 0 && j == 0)) continue;

                // Set new instance of lego component
                legoPieces[i, j] = Instantiate(baseLegoData.gameObject).GetComponent<Lego>();

                // Set x and y value for piece and offset position accordingly.
                legoPieces[i, j].SetGroupElement(newX, newY);
                Vector3 setPos = baseLegoData.transform.TransformPoint(Vector3.zero);
                setPos += (baseLegoData.transform.right * Lego.sqSize * i);
                setPos += (baseLegoData.transform.forward * Lego.sqSize * j);

                legoPieces[i, j].transform.position = setPos;

            }
        }

        // Adjust legoGroup position to center of group.
        Vector3 newPos = transform.TransformPoint(Vector3.zero);
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
    }

    public void ChangeGroupColor(Color newColor)
    {
        baseLegoData.LegoRenderer.material.color = newColor;
    }
}
