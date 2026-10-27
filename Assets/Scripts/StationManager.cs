using UnityEngine;
using TMPro; // Required for TextMeshPro UI

public class StationManager : MonoBehaviour
{
    [Header("Script References")]
    [Tooltip("Drag the GameObject with your SmoothCameraMouseLook script here")]
    public SmoothCameraMouseLook cameraScript;
    
    [Tooltip("Drag your UI Text element here to display the current station")]
    public TextMeshProUGUI stationNameText;

    [Header("Station Settings")]
    // Since Neurality has 2 stations, index 0 is Learning and index 1 is Result
    public string[] stationNames = { "Learning Station", "Result Station" };
    
    // Tracks the last index so we don't unnecessarily update the UI every frame
    private int currentStationIndex = -1;

    void Update()
    {
        // Ensure references exist
        if (cameraScript == null || stationNameText == null) return;

        // Check if the player pressed A or D to change the camera's target index
        if (cameraScript.currentIndex != currentStationIndex)
        {
            currentStationIndex = cameraScript.currentIndex;
            OnStationChanged(currentStationIndex);
        }
    }

    // This method fires exactly once every time the station switches
    private void OnStationChanged(int newIndex)
    {
        // 1. Update the UI Text
        if (newIndex >= 0 && newIndex < stationNames.Length)
        {
            stationNameText.text = stationNames[newIndex];
        }

        // 2. Handle Station-Specific Logic
        if (newIndex == 0)
        {
            // Player is looking at the Learning Station
            Debug.Log("Welcome to the Learning Station! Enable Keyword Cards.");
            
            // TODO: Enable the keyword drag-and-drop interactions here
            // TODO: Disable conveyor belt interactions
        }
        else if (newIndex == 1)
        {
            // Player is looking at the Result Station
            Debug.Log("Welcome to the Result Station! Enable Conveyor Belt.");
            
            // TODO: Enable conveyor belt scanning interactions here
            // TODO: Disable keyword interactions to prevent players from changing words mid-scan
        }
    }
}