using UnityEngine;

public class TreasureChestInteraction : MonoBehaviour
{
    public GameObject topPart; // Reference to the top part of the chest
    public float openAngle = 270f; // Positive angle to open the chest upwards
    public float openSpeed = 2f; // Speed of the chest opening
    public KeyCode interactKey = KeyCode.E; // Interaction key
    public AudioClip chestOpenSFX; // Sound effect for opening
    public AudioClip chestCloseSFX; // Sound effect for closing

    private bool isOpen = false;
    private bool inRange = false;
    private Quaternion initialRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;

    void Start()
    {
        // Set up the initial rotation and audio source
        initialRotation = topPart.transform.localRotation;
        openRotation = Quaternion.Euler(-openAngle, 0, 0) * initialRotation; // Open upwards
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
            PlaySoundEffect();
        }

        Quaternion targetRotation = isOpen ? openRotation : initialRotation;
        RotateTopPart(targetRotation);
    }

    private void RotateTopPart(Quaternion targetRotation)
    {
        topPart.transform.localRotation = Quaternion.Slerp(topPart.transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
    }

    private void PlaySoundEffect()
    {
        if (audioSource != null)
        {
            audioSource.clip = isOpen ? chestOpenSFX : chestCloseSFX;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}