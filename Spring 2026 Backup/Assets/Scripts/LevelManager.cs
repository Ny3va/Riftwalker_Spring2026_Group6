using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Transform worldA;
    public Transform worldB;
    public Vector3 worldOffset = new(0, 500, 0);

    public GameObject portalPrefab;
    public Transform player;

    public LayerMask worldAMask;
    public LayerMask worldBMask;

    public bool isInWorldA = true;
    private GameObject activeRift;
    private GameObject activeExit;

    public bool isTeleporting = false;

    public GameObject emptyIcon;
    public GameObject filledIcon;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnPortal(Vector3 position)
    {
       
        if (activeRift != null) Destroy(activeRift);
        if (activeExit != null) Destroy(activeExit);

        Vector3 dirToPlayer = player.position - position;
        dirToPlayer.y = 0;
        Quaternion facing = dirToPlayer != Vector3.zero ? Quaternion.LookRotation(dirToPlayer) : Quaternion.identity;

        Vector3 spawnPos = position + Vector3.up * 1f;

        activeRift = Instantiate(portalPrefab, spawnPos, facing);

        Quaternion exitFacing = facing * Quaternion.Euler(0, 180, 0);
        Vector3 otherWorldOffset = isInWorldA ? worldOffset : -worldOffset;
        activeExit = Instantiate(portalPrefab, spawnPos + otherWorldOffset, exitFacing);

        Portal entranceScript = activeRift.GetComponent<Portal>();
        Portal exitScript = activeExit.GetComponent<Portal>();

        entranceScript.SetDestination(activeExit.transform);
        exitScript.SetDestination(activeRift.transform);

    }

    public void SwitchWorlds()
    {
        isInWorldA = !isInWorldA;
        emptyIcon.SetActive(isInWorldA);
        filledIcon.SetActive(!isInWorldA);
    }

    public void StartTeleportCooldown()
    {
        isTeleporting = true;
        Invoke(nameof(ResetTeleport), 1f);
    }

    private void ResetTeleport()
    {
        isTeleporting = false;
    }
}
