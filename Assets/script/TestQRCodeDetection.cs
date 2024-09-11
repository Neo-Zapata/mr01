using UnityEngine;
using Microsoft.MixedReality.OpenXR;
using MixedReality.Toolkit.SpatialManipulation;
using TMPro;

public class TestQRCodeDetection : MonoBehaviour
{
    [SerializeField] private GameObject mainText;
    [SerializeField] private ARMarkerManager markerManager;
    [SerializeField] private GameObject cubodemo;
    private TextMeshProUGUI m_TextMeshPro;
    private BoundsControl boundsControl;
    private void Start()
    {
        if (markerManager == null)
        {
            Debug.LogError("ARMarkerManager no est� asignado.");
            return;
        }

        // Suscribirse al evento markersChanged
        markerManager.markersChanged += OnMarkersChanged;

        // Inicializar TextMeshPro
        m_TextMeshPro = (mainText != null) ? mainText.GetComponent<TextMeshProUGUI>() : null;
        if (m_TextMeshPro == null)
        {
            Debug.LogError("Componente TextMeshProUGUI no encontrado en el GameObject mainText.");
        }
    }

    /// <summary>
    /// Maneja el evento markersChanged y procesa los marcadores agregados, actualizados y eliminados.
    /// </summary>
    /// <param name="args">Argumentos del evento que contienen informaci�n sobre los marcadores agregados, actualizados y eliminados.</param>
    private void OnMarkersChanged(ARMarkersChangedEventArgs args)
    {
        Debug.Log("****on marker triggered");
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
    /// Maneja la l�gica para los marcadores reci�n agregados.
    /// </summary>
    /// <param name="addedMarker">El ARMarker reci�n agregado.</param>
    private void HandleAddedMarker(ARMarker addedMarker)
    {
        boundsControl = null;
        Debug.Log($"***�C�digo QR detectado! ID del marcador: {addedMarker.trackableId}");
        if (cubodemo != null)
        {
            // Instanciar el cubo en la posici�n del marcador QR
            GameObject cuboInstanciado = Instantiate(cubodemo, addedMarker.transform.position, addedMarker.transform.rotation);
            Debug.Log($"***�Cubo instanciado: {addedMarker.trackableId}");
            cubodemo.SetActive(true);
            // Obtener el componente BoundsControl del cubo instanciado
            boundsControl = cuboInstanciado.GetComponent<BoundsControl>();

            if (boundsControl != null)
            {
                Debug.Log("***BoundsControl encontrado en el cubo instanciado.");
                // Puedes activar o configurar el BoundsControl aqu�
                boundsControl.enabled = true;
            }
            else
            {
                Debug.LogError("***BoundsControl no encontrado en el cubo instanciado.");
            }


        }

        // Puedes acceder a m�s informaci�n sobre el marcador usando las propiedades de addedMarker
        // Por ejemplo, addedMarker.GetDecodedString() o addedMarker.GetQRCodeProperties()
        // L�gica adicional para manejar marcadores reci�n agregados
    }

    /// <summary>
    /// Maneja la l�gica para los marcadores actualizados.
    /// </summary>
    /// <param name="updatedMarker">El ARMarker actualizado.</param>
    private void HandleUpdatedMarker(ARMarker updatedMarker)
    {
        Debug.Log($"***�C�digo QR actualizado! ID del marcador: {updatedMarker}");
        // Puedes acceder a la informaci�n sobre el marcador usando las propiedades de updatedMarker
        // L�gica adicional para manejar marcadores actualizados
        boundsControl = null;
        // Obtiene la cadena decodificada del marcador agregado
        string qrCodeString = updatedMarker.GetDecodedString();

        // Establece la cadena del c�digo QR en el componente TextMeshPro
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = qrCodeString;
        }

        // Check if the detected QR code matches the target text
        if (qrCodeString == "cantstop")
        {
            // If the QR code matches, show the object
            if (cubodemo != null)
            {
                cubodemo.SetActive(true);
            }
        }
        else
        {
            // Optionally, hide the object if the QR code does not match
            if (cubodemo != null)
            {
                cubodemo.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Maneja la l�gica para los marcadores eliminados.
    /// </summary>
    /// <param name="removedMarkerId">El ID del marcador eliminado.</param>
    private void HandleRemovedMarker(ARMarker removedMarkerId)
    {
        Debug.Log($"***�C�digo QR eliminado! ID del marcador: {removedMarkerId}");

        // Limpia el texto de TextMeshPro cuando se elimina un marcador
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = string.Empty;
        }
    }
}