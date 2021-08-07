using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to control the sliders and detect changes on values
public class SliderController : MonoBehaviour
{

    public delegate void Panel_GameUiValueChangeEvent(float val);

    public Panel_GameUiValueChangeEvent
        onCoherenceValueChange,
        onSeperationValueChange,
        onAlignmentValueChange;
    public UnityEngine.UI.Slider
         slider_Coherence,
         slider_Seperation,
         slider_Alignment;
    void Start()
    {
        slider_Coherence.onValueChanged.AddListener(HandleCoherenceValueChanged);
        slider_Seperation.onValueChanged.AddListener(HandleSeperationValueChanged);
        slider_Alignment.onValueChanged.AddListener(HandleAlignmentValueChanged);
    }

    private void HandleCoherenceValueChanged(float val)
    {
        onCoherenceValueChange?.Invoke(val);
    }


    private void HandleSeperationValueChanged(float val)
    {
        onSeperationValueChange?.Invoke(val);
    }

    private void HandleAlignmentValueChanged(float val)
    {
        onAlignmentValueChange?.Invoke(val);
    }
}
