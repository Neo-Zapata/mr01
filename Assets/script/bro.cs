using UnityEngine;
using Microsoft.MixedReality.OpenXR;

public class QRCodeObjectActivator : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;  // Object that will be shown if the QR code matches
    [SerializeField] private string targetQRCodeText;  // The text we are checking for in the detected QR code
    [SerializeField] private ARMarkerManager markerManager;  // ARMarkerManager for QR code detection

    private void Start()
    {
        // Ensure markerManager is assigned
        if (markerManager == null)
        {
            Debug.LogError("ARMarkerManager is not assigned.");
            return;
        }

        // Subscribe to the markersChanged event
        markerManager.markersChanged += OnMarkersChanged;
    }

    /// <summary>
    /// Handles the markersChanged event and processes added, updated, and removed markers.
    /// </summary>
    /// <param name="args">Event arguments containing information about added, updated, and removed markers.</param>
    private void OnMarkersChanged(ARMarkersChangedEventArgs args)
    {
        foreach (var addedMarker in args.added)
        {
            HandleAddedMarker(addedMarker);
        }

        foreach (var updatedMarker in args.updated)
        {
            HandleUpdatedMarker(updatedMarker);
        }

        foreach (var removedMarkerId in args.removed)
        {
            HandleRemovedMarker(removedMarkerId);
        }
    }

    /// <summary>
    /// Handles logic for newly added markers.
    /// </summary>
    /// <param name="addedMarker">The newly added ARMarker.</param>
    private void HandleAddedMarker(ARMarker addedMarker)
    {
        Debug.Log($"QR Code Detected! Marker ID: {addedMarker.trackableId}");

        // Get the detected QR code string
        string qrCodeString = addedMarker.GetDecodedString();

        // Check if the detected QR code matches the target text
        if (qrCodeString == targetQRCodeText)
        {
            // If the QR code matches, show the object (activate it)
            if (targetObject != null)
            {
                targetObject.SetActive(true);  // Show the object
            }
            else
            {
                Debug.LogWarning("Target object is not assigned.");
            }
        }
        else
        {
            // Optionally, hide the object if the QR code does not match
            if (targetObject != null)
            {
                targetObject.SetActive(false);  // Hide the object
            }
        }
    }

    /// <summary>
    /// Handles logic for updated markers.
    /// </summary>
    /// <param name="updatedMarker">The updated ARMarker.</param>
    private void HandleUpdatedMarker(ARMarker updatedMarker)
    {
        // Get the detected QR code string
        string qrCodeString = updatedMarker.GetDecodedString();

        // Check if the detected QR code matches the target text
        if (qrCodeString == targetQRCodeText)
        {
            // If the QR code matches, show the object
            if (targetObject != null)
            {
                targetObject.SetActive(true);
            }
        }
        else
        {
            // Optionally, hide the object if the QR code does not match
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Handles logic for removed markers.
    /// </summary>
    /// <param name="removedMarkerId">The ID of the removed marker.</param>
    private void HandleRemovedMarker(ARMarker removedMarkerId)
    {
        Debug.Log($"QR Code Removed! Marker ID: {removedMarkerId}");

        // Optionally, you can hide the object when a marker is removed
        if (targetObject != null)
        {
            targetObject.SetActive(false);  // Hide the object
        }
    }
}
