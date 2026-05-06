using UnityEngine;
using UnityEngine.UI;

public class EXPbar : MonoBehaviour
{
    public Slider expSlider;

    public void setMaxExp(int exp)
    {
        expSlider.maxValue = exp;
    }
    public void setExp(int exp)
    {
        expSlider.value = exp;        
    }
}
