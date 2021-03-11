using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;

/// <summary>
/// While grabbed by a tilia interactor, moves highest parent transform found on the object
/// the same way this object's transform moves each frame.
/// 
/// In The case of Legos, makes the entire lego parent object move when grabbing any
/// one of the legos in a group.
/// </summary>
[RequireComponent(typeof(InteractableFacade))]
public class MoveParentObjectWhileGrabbed : MonoBehaviour
{
    public Transform highestParent;
    public InteractableFacade interactableFacade;

    private Collider parentCollider;

    // Start is called before the first frame update
    void Start()
    {
        interactableFacade = GetComponent<InteractableFacade>();
        RefreshParent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (interactableFacade.IsGrabbed)
        {
            parentCollider.isTrigger = true;
            highestParent.position = transform.TransformPoint(Vector3.zero);
            highestParent.rotation = transform.rotation;
        }
        else
            parentCollider.isTrigger = false;
    }

    // Updates the highestParent transform that will follow the movement of this object.
    public void RefreshParent()
    {
        Transform tempParent = transform.parent;
        while(tempParent.parent != null)
        {
            tempParent = tempParent.parent;
        }

        highestParent = tempParent;
        parentCollider = highestParent.GetComponent<Collider>();
    }
}
