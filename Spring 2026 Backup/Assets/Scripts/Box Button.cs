using UnityEngine;

public class BoxButton : MonoBehaviour
{
    public GameObject glassWall;

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            glassWall.SetActive(false);
        }
    }

    void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            glassWall.SetActive(true);
        }
    }
}
