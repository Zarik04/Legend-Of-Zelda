using UnityEngine;

public class DoorTriggerInteraction : MonoBehaviour
{
    public GameObject door; // Reference to the door object
    public Vector3 hingeOffset = new Vector3(0.1f, 0, 0); // Adjust this for the hinge position (left edge)
    public float openAngle = -90f; // Angle to open the door
    public float openSpeed = 2f; // Speed of the door opening
    public KeyCode interactKey = KeyCode.E; // Interaction key
    public AudioClip doorOpenSFX; // Sound effect for opening
    public AudioClip doorCloseSFX; // Sound effect for closing

    private bool isOpen = false;
    private bool inRange = false;
    private Quaternion initialRotation;
    private Quaternion openRotation;
    private Transform playerTransform;
    private AudioSource audioSource;

    void Start()
    {
        // Set up the initial rotation and audio source
        initialRotation = door.transform.rotation;
        audioSource = door.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
            UpdateOpenRotation();
            PlaySoundEffect();
        }

        Quaternion targetRotation = isOpen ? openRotation : initialRotation;
        RotateDoorAroundHinge(targetRotation);
    }

    private void UpdateOpenRotation()
    {
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = playerTransform.position - door.transform.position;
            float dotProduct = Vector3.Dot(door.transform.forward, directionToPlayer.normalized);
            float direction = dotProduct > 0 ? 1 : -1;
            openRotation = Quaternion.Euler(0, direction * openAngle, 0) * initialRotation;
        }
    }

    private void RotateDoorAroundHinge(Quaternion targetRotation)
    {
        Vector3 hingePoint = door.transform.position + door.transform.TransformDirection(hingeOffset);
        door.transform.position = hingePoint;
        door.transform.rotation = Quaternion.Slerp(door.transform.rotation, targetRotation, Time.deltaTime * openSpeed);
        door.transform.position -= door.transform.TransformDirection(hingeOffset);
    }

    private void PlaySoundEffect()
    {
        if (audioSource != null)
        {
            audioSource.clip = isOpen ? doorOpenSFX : doorCloseSFX;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            playerTransform = null;
        }
    }
}