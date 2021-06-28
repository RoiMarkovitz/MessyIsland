using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundsSlider : MonoBehaviour
{
    int roundNumber;
    [SerializeField] TMPro.TextMeshProUGUI roundsText;
    Slider slider;

    public void updateRoundNumber()
    {
        if (slider.value == 1)
        {
            roundNumber = 1;
        }
        else if (slider.value == 2)
        {
            roundNumber = 3;
        }
        else
        {
            roundNumber = 5;
        }
        roundsText.text = "Rounds: " + roundNumber;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        
    }

    public int getRoundsNumber()
    {
        return roundNumber;
    }
}
