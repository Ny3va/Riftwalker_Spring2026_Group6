using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Transform holdPosition;
    public float interactRange = 5f;
    public float followSpeed = 50f;
    public float snapDistance = 0.5f;

    private GameObject heldObject;
    private Rigidbody heldRb;

    public bool HoldingObject => heldObject != null;

    public GameObject GetHeldObject() => heldObject;
    private void FixedUpdate()
    {
        if (HoldingObject)
        {
            MoveObject();
        }
    }
    public void Interact()
    {

        if (HoldingObject)
        {
            Drop();
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            DoorController door = hit.collider.GetComponentInParent<DoorController>();

            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                PickUp(hit.collider.gameObject);
            }
            else if (door != null)
            {
                door.Interact();
            }
        }
    }
    public void PickUp(GameObject obj)
    {
        if (obj.TryGetComponent<Rigidbody>(out heldRb))
        {
            heldObject = obj;

            heldRb.useGravity = false;
            heldRb.linearDamping = 10f;
            heldRb.angularDamping = 10f;
            heldRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        else
        {
            return;
        }
    }

    public void Drop()
    {
        if (heldObject != null && heldRb != null)
        {
            heldRb.useGravity = true;
            heldRb.linearDamping = 1f;
            heldRb.angularDamping = 0.05f;
            heldRb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            heldObject = null;
            heldRb = null;
        }
    }

    private void MoveObject()
    {
        if (heldObject == null || heldRb == null) return;

        Vector3 targetPosition = holdPosition.position;
        Vector3 currentPosition = heldRb.position;
        Vector3 direction = targetPosition - currentPosition;

        Vector3 targetVelocity = direction * followSpeed;

        heldRb.linearVelocity = Vector3.Lerp(heldRb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 20f);

        Quaternion targetRotation = holdPosition.rotation;
        heldRb.MoveRotation(Quaternion.RotateTowards(
            heldRb.rotation,
            targetRotation,
            followSpeed * 10f * Time.fixedDeltaTime));

    }
}
