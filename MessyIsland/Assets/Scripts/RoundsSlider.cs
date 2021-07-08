using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundsSlider : MonoBehaviour
{
    readonly int[] numberOfRounds = { 1, 3, 5 };
    int roundNumber;
    [SerializeField] TMPro.TextMeshProUGUI roundsText;
    Slider slider;

    public void updateRoundNumber()
    {
        if (slider.value == 1)
        {
            roundNumber = numberOfRounds[0];
        }
        else if (slider.value == 2)
        {
            roundNumber = numberOfRounds[1];
        }
        else
        {
            roundNumber = numberOfRounds[2];
        }
        roundsText.text = "Rounds: " + roundNumber;

        GameManager.instance.setNumberOfRounds(roundNumber);
    }

    void Start()
    {
        slider = GetComponent<Slider>();

    }


    void Update()
    {

    }


}
