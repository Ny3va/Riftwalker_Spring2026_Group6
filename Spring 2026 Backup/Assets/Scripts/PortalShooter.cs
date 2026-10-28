using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalShooter : MonoBehaviour
{
    public GameObject boltPrefab;
    public Transform firePoint;
    public Player playerScript;

    private InputAction shootAction;

    public Transform hourHand;
    public Transform minuteHand;
    public float spinTime = 0.5f;
    public float totalRotation = 720f;

    public AudioSource tickSound;

    [SerializeField]
    private InputActionAsset inputAsset;

    private void Awake()
    {
        shootAction = inputAsset.FindAction("Player/Attack");
        //altFireAction = inputAsset.FindAction("Player/AltFire");
    }
    // Update is called once per frame
    private void OnEnable()
    {
        shootAction.Enable();
        //altFireAction.Enable();
    }
    private void OnDisable()
    {
        shootAction.Disable();
        //altFireAction.Disable();
    }

    void Update()
    {
        if (playerScript!=null && playerScript.isPaused) {
            return;
        }
        if (shootAction.triggered)
        {
            Shoot(boltPrefab);
        }
            //if (altFireAction.triggered)
            //{
            //    Shoot(boltPrefab2);
        //}
    }

    void Shoot(GameObject projectile)
    {
        Instantiate(projectile, firePoint.position, firePoint.rotation);

        if (tickSound != null)
        {
            tickSound.PlayOneShot(tickSound.clip);
        }
        StopAllCoroutines();
        StartCoroutine(SpinClock());
    }

    IEnumerator SpinClock()
    {
        float elapsedTime = 0f;

        Quaternion startMinute = minuteHand.localRotation;
        Quaternion startHour = hourHand.localRotation;

        while (elapsedTime < spinTime)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / spinTime;

            float currentRotation = percent * totalRotation;

            minuteHand.localRotation = startMinute * Quaternion.Euler(0, currentRotation * 12f, 0);
            hourHand.localRotation = startHour * Quaternion.Euler(0, currentRotation, 0);
            yield return null;
        }
    }
}
