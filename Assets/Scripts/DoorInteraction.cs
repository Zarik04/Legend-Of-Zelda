using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject door; // Reference to the door GameObject
    public float openAngle = 90f; // Angle to open the door
    public float openSpeed = 2f; // Speed at which the door opens
    public KeyCode interactKey = KeyCode.E; // Key to interact with the door

    private bool isOpen = false;
    private bool inRange = false;
    private Quaternion initialRotation;
    private Quaternion openRotation;

    void Start()
    {
        initialRotation = door.transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
        }

        if (isOpen)
        {
            door.transform.rotation = Quaternion.Slerp(door.transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            door.transform.rotation = Quaternion.Slerp(door.transform.rotation, initialRotation, Time.deltaTime * openSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            Debug.Log("Player near!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log("Player left!");
        }
    }
}
