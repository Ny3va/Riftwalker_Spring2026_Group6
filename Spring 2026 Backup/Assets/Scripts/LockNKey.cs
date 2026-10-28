using UnityEngine;

public class LockNKey : MonoBehaviour
{
    public string keyTag = ("LevelKey");
    public DoorController door;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.TryGetComponent<Key>(out Key key))
        {
            if (key.keyType == keyTag)
            {
                OpenDoor(key.gameObject);
            }
        }
    }

    void OpenDoor(GameObject keyObj)
    {
        InteractionController interaction = FindFirstObjectByType<InteractionController>();

        if (interaction != null)
        {
            interaction.Drop();
        }

        keyObj.SetActive(false);

        if (keyObj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        if (door != null)
        {
            door.Unlock();
        }
    }
}
