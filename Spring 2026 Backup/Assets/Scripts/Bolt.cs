using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bolt : MonoBehaviour
{
    public float speed = 30f;
    private Rigidbody rb;


    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        rb.useGravity = true;
        Destroy(gameObject,5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bolt hit a surface that can have a portal
        if (!collision.gameObject.CompareTag("PortalSurface"))
        {
            Destroy(gameObject);
            return;
        }

        ContactPoint contact = collision.GetContact(0);

        float groundDot = Vector3.Dot(contact.normal, Vector3.up);
        if (groundDot < 0.5f) return;

        LevelManager.Instance.SpawnPortal(contact.point);

        Destroy(gameObject);
    }
}
