using UnityEngine;
using UnityEngine.UI;
public class HUDSliderZoom : MonoBehaviour
{
    public Slider zoomSlider;
    public CamaraController camaraController;
    private bool isStarted = false;
    void Update()
    {
        UpdateSlider();
    }
    public void UpdateSlider()
    {
        if (camaraController != null && zoomSlider != null)
        {
            zoomSlider.minValue = 1f;
            zoomSlider.maxValue = camaraController.GetMaxOrthographicSize();
            zoomSlider.value = camaraController.GetZoomActual();
        }
        else
        {
            Debug.LogError("Asignar referencias en el inspector");
        }
    }
    public void OnSliderValueChanged()
    {
        if (camaraController != null)
        {
            camaraController.CambiarZoom(zoomSlider.value);
        }
    }
}