using UnityEngine.UI;
using UnityEngine;

public class Volume : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider slider;


    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioListener.volume;
    }

    public void ValueChangeCheck()
    {
        AudioListener.volume = slider.value;
    }
}
