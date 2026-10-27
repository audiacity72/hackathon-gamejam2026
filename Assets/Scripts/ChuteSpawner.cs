using UnityEngine;
using UnityEngine.InputSystem; 

public class ChuteSpawner : MonoBehaviour
{
    [Header("Chute Setup")]
    [Tooltip("The exact point where the duplicate object teleports to.")]
    public Transform spawnPoint;

    [Tooltip("Drag your 3D object prefabs here.")]
    public GameObject[] objectPrefabs;

    [Header("Physics Settings")]
    [Tooltip("A slight downward push to help the object out of the chute.")]
    public float downwardEjectionForce = 2f;

    void Update()
    {
        // Drop a new duplicated item when the player presses 'E'
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            SpawnRandomObject();
        }
    }

    public void SpawnRandomObject()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0)
        {
            Debug.LogWarning("ChuteSpawner has no objects to spawn! Add some to the array.");
            return;
        }

        // 1. Pick a random prefab from the list
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        GameObject selectedPrefab = objectPrefabs[randomIndex];

        // 2. DUPLICATE the object first
        GameObject spawnedClone = Instantiate(selectedPrefab);
        spawnedClone.name = selectedPrefab.name; // Clean up the name

        // 3. TELEPORT the object to the spawn point
        Vector3 targetPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        spawnedClone.transform.position = targetPosition;
        spawnedClone.transform.rotation = Quaternion.identity;

        // 4. Ensure it has a Rigidbody, reset its momentum, and turn USE GRAVITY ON
        Rigidbody rb = spawnedClone.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawnedClone.AddComponent<Rigidbody>();
        }
        
        rb.isKinematic = false; // Ensure it can be moved by physics
        rb.useGravity = true;   // <--- Turns gravity ON for the duplicate
        rb.velocity = Vector3.zero; // Clears any weird momentum during teleport

        // 5. Apply a quick downward impulse force to shoot it out of the chute
        rb.AddForce(Vector3.down * downwardEjectionForce, ForceMode.Impulse);
    }
}