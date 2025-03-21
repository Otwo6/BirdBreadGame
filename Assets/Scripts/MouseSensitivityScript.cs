using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityScript : MonoBehaviour
{
    Slider sensSlider;
    public RelayManager relayMan;
    void Start()
    {
        sensSlider = GetComponent<Slider>();
    }

    public void ChangeSens()
    {
        relayMan.playerSens = Mathf.Lerp(25.0f, 75.0f, sensSlider.value);
    }
}
