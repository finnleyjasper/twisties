using UnityEngine;
using UnityEngine.UI;

public class WaterLevelUI : MonoBehaviour
{
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetWaterLevel(int water) { slider.value = water; }
}
