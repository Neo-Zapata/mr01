using UnityEngine;
using Microsoft.MixedReality.OpenXR;
using MixedReality.Toolkit.SpatialManipulation;
using TMPro;
 
public class TestQRCodeDetection : MonoBehaviour
{
    [SerializeField] private GameObject mainText;
    [SerializeField] private string targetQRCodeText;  // The text we are checking for in the detected QR code
    [SerializeField] private ARMarkerManager markerManager;
    [SerializeField] private GameObject cubodemo;
    private TextMeshProUGUI m_TextMeshPro;
    private BoundsControl boundsControl;
    private bool spaceFlag = true;
    private void Start()
    {
        if (markerManager == null)
        {
            Debug.LogError("***ARMarkerManager no está asignado.");
            return;
        }

        // Suscribirse al evento markersChanged
        markerManager.markersChanged += OnMarkersChanged;
        
        // Inicializar TextMeshPro
        m_TextMeshPro = (mainText != null) ? mainText.GetComponent<TextMeshProUGUI>() : null;
        if (m_TextMeshPro == null)
        {
            Debug.LogError("***Componente TextMeshProUGUI no encontrado en el GameObject mainText.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("***Space key pressed. Triggering update function.");

            if(spaceFlag){
                cubodemo.SetActive(true);
                spaceFlag = false;
            } else {
                cubodemo.SetActive(false);
                spaceFlag = true;
            }
        }
    }

    /// <summary>
    /// Maneja el evento markersChanged y procesa los marcadores agregados, actualizados y eliminados.
    /// </summary>
    /// <param name="args">Argumentos del evento que contienen información sobre los marcadores agregados, actualizados y eliminados.</param>
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
    /// Maneja la lógica para los marcadores recién agregados.
    /// </summary>
    /// <param name="addedMarker">El ARMarker recién agregado.</param>
    private void HandleAddedMarker(ARMarker addedMarker)
    {
        boundsControl = null;
        Debug.Log($"***¡Código QR detectado! ID del marcador: {addedMarker.trackableId}");
        
        string qrCodeString = addedMarker.GetDecodedString();

        if(qrCodeString == targetQRCodeText){
            cubodemo.SetActive(true);
        } else {
           cubodemo.SetActive(false);
        }
        
        if (cubodemo != null)
        {
            // Instanciar el cubo en la posición del marcador QR
            GameObject cuboInstanciado = Instantiate(cubodemo, addedMarker.transform.position, addedMarker.transform.rotation);
            Debug.Log($"***¡Cubo instanciado: {addedMarker.trackableId}");

            // Obtener el componente BoundsControl del cubo instanciado
            boundsControl = cuboInstanciado.GetComponent<BoundsControl>();

            if (boundsControl != null)
            {
                Debug.Log("***BoundsControl encontrado en el cubo instanciado.");
                // Puedes activar o configurar el BoundsControl aquí
                boundsControl.enabled = true;
            }
            else
            {
                Debug.LogError("***BoundsControl no encontrado en el cubo instanciado.");
            }
        }

        // Puedes acceder a más información sobre el marcador usando las propiedades de addedMarker
        // Por ejemplo, addedMarker.GetDecodedString() o addedMarker.GetQRCodeProperties()
        // Lógica adicional para manejar marcadores recién agregados
    }
    
    /// <summary>
    /// Maneja la lógica para los marcadores actualizados.
    /// </summary>
    /// <param name="updatedMarker">El ARMarker actualizado.</param>
    private void HandleUpdatedMarker(ARMarker updatedMarker)
    {
        Debug.Log($"***¡Código QR actualizado! ID del marcador: {updatedMarker}");
        // Puedes acceder a la información sobre el marcador usando las propiedades de updatedMarker
        // Lógica adicional para manejar marcadores actualizados
        boundsControl = null;
        // Obtiene la cadena decodificada del marcador agregado
        string qrCodeString = updatedMarker.GetDecodedString();

        if(qrCodeString == targetQRCodeText){
            cubodemo.SetActive(true);
        } else {
           cubodemo.SetActive(false);
        }
        
        // Establece la cadena del código QR en el componente TextMeshPro
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = qrCodeString;
        }
    }
    
    /// <summary>
    /// Maneja la lógica para los marcadores eliminados.
    /// </summary>
    /// <param name="removedMarkerId">El ID del marcador eliminado.</param>
    private void HandleRemovedMarker(ARMarker removedMarkerId)
    {
        Debug.Log($"***¡Código QR eliminado! ID del marcador: {removedMarkerId}");
        
        // Limpia el texto de TextMeshPro cuando se elimina un marcador
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = string.Empty;
        }

        // // Optionally, you can hide the object when a marker is removed
        // if (targetObject != null)
        // {
        //     targetObject.SetActive(false);  // Hide the object
        // }
    }
}
