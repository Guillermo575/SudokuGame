using UnityEngine;
using UnityEngine.UI;
public class ColorSelector : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image colorPreview;
    public Color color { get { return new Color(redSlider.value, greenSlider.value, blueSlider.value); } }
    void Start()
    {
        redSlider.onValueChanged.AddListener(UpdateColor);
        greenSlider.onValueChanged.AddListener(UpdateColor);
        blueSlider.onValueChanged.AddListener(UpdateColor);
        UpdateColor(0);
    }
    void UpdateColor(float value)
    {
        colorPreview.color = color;
    }
}