using UnityEngine;

public class PlayerButton : MonoBehaviour
{
    public GameObject doorWall1;
    public GameObject doorWall2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorWall1.SetActive(false);
            doorWall2.SetActive(false);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorWall1.SetActive(true);
            doorWall2.SetActive(true);
        }
    }*/
}
