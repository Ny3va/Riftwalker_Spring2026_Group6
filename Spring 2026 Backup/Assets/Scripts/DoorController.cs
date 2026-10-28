using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }
    public enum DoorDirection { Forward = 1, Backward = -1 }

    public DoorDirection direction = DoorDirection.Forward;
    public RotationAxis axis = RotationAxis.Y;

    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isLocked = false;

    private bool isOpen = false;
    private Quaternion targetRotation;
    private Quaternion closedRotation;

    void Awake()
    {
        closedRotation = transform.rotation;

        Vector3 rotationVector = Vector3.zero;
        float finalAngle = openAngle * (int)direction;

        switch (axis)
        {
            case RotationAxis.X: rotationVector = new Vector3(finalAngle, 0, 0); break;
            case RotationAxis.Y: rotationVector = new Vector3(0, finalAngle, 0); break;
            case RotationAxis.Z: rotationVector = new Vector3(0, 0, finalAngle); break;
        }

        targetRotation = closedRotation * Quaternion.Euler(rotationVector);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion target = isOpen ? targetRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * openSpeed);
    }

    public void Interact()
    {
        if (isLocked)
        {
            return;
        }
        isOpen = !isOpen;
    }

    public void Unlock()
    {
        isLocked = false;
    }
}
