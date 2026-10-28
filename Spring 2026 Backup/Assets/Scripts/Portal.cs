using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destination;

    public AudioClip teleportSound;
    public float volume = 1f;
    public float objectTeleportDelay = 1f;

    private AudioSource audioSource;
    private readonly HashSet<GameObject> onCooldown = new();

    private bool IsOnCooldown(GameObject obj) => onCooldown.Contains(obj);

    private void SetCooldown(GameObject obj)
    {
        StartCoroutine(CooldownRoutine(obj));
    }

    private System.Collections.IEnumerator CooldownRoutine(GameObject obj)
    {
        onCooldown.Add(obj);
        yield return new WaitForSeconds(objectTeleportDelay);
        onCooldown.Remove(obj);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0;
        audioSource.playOnAwake = false;

    }
    public void SetDestination(Transform destination)
        { this.destination = destination; }

    void Teleport(GameObject player)
    {
        if (destination == null || LevelManager.Instance.isTeleporting) return;

        PlaySound();
        LevelManager.Instance.StartTeleportCooldown();

        Vector3 offset = LevelManager.Instance.isInWorldA
            ? LevelManager.Instance.worldOffset
            : -LevelManager.Instance.worldOffset;

        var interactionScript = player.GetComponent<InteractionController>();
        if (interactionScript != null && interactionScript.HoldingObject)
        {
           
            GameObject held = interactionScript.GetHeldObject(); 
            held.transform.position += offset;
            SetCooldown(held); 
        }

        if (player.TryGetComponent<CharacterController>(out var cc)) cc.enabled = false;
        player.transform.position += offset;
        if (cc != null) cc.enabled = true;

        LevelManager.Instance.SwitchWorlds();

    }
    void TeleportObject(GameObject obj)
    {
        if (IsOnCooldown(obj)) return;

        PlaySound();
        SetCooldown(obj);

        Vector3 offset = LevelManager.Instance.isInWorldA
            ? LevelManager.Instance.worldOffset
            : -LevelManager.Instance.worldOffset;


        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.WakeUp();
            rb.position += offset;
            
        }
        else
        {
            obj.transform.position += offset;
        }
    }
    private void PlaySound()
    {
        if (teleportSound != null)
        {
            audioSource.PlayOneShot(teleportSound, volume);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        bool portalInWorldA = transform.position.y < (LevelManager.Instance.worldOffset.y / 2f);

        if (portalInWorldA != LevelManager.Instance.isInWorldA) return;

        if (other.CompareTag("Player"))
        {
            Teleport(other.gameObject);
        }
        else if (other.CompareTag("Interactable"))
        {
            var interaction = LevelManager.Instance.player.GetComponent<InteractionController>();
            bool isHeld = interaction != null &&
                          interaction.HoldingObject &&
                          interaction.GetHeldObject() == other.gameObject;

            if (!isHeld)
            {
                TeleportObject(other.gameObject);
            }
        }
    }
}
