using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    const string ROUND_WON = "Round Won";
    const string ROUND_LOST = "Round Lost";
    const string GAME_WON = "Game Won";
    const string GAME_LOST = "Game Lost";

    const float ACTIVATED_WEAPONS_PERCENTAGE = 0.5f;

    readonly int[] numberOfRounds = { 1, 3, 5 };
    int chosenNumberOfRounds;
    int currentRound;
    int ninjaTeamRoundsWon;
    int swatTeamRoundsWon;

   
    [SerializeField] GameObject[] swatTeam;
    [SerializeField] GameObject[] ninjaTeam;

    // audio clips "Ninjas win the game", "Ninjas win the round"
    // , "Swat win the game", "Swat win the round"
   
    [SerializeField] GameObject grenades;
    [SerializeField] GameObject pistols;
    GameObject[] grenadesArray;
    GameObject[] pistolsArray;

    Player playerScript;

    [SerializeField] GameObject gameCanvas;
    Text healthText;
    Text roundCounterText;
    Text winsCountText;
    Text roundEndMessage;


    void Start()
    {
        grenadesArray = new GameObject[grenades.transform.childCount];
        for (int i = 0; i < grenadesArray.Length; i++)
            grenadesArray[i] = grenades.transform.GetChild(i).gameObject;

        pistolsArray = new GameObject[pistols.transform.childCount];    
        for (int i = 0; i < pistolsArray.Length; i++)
            pistolsArray[i] = pistols.transform.GetChild(i).gameObject;
        
        bool[] isActivatedGrenades = new bool[grenadesArray.Length];
        bool[] isActivatedPistols = new bool[pistolsArray.Length];

        setActivatedWeapons(isActivatedGrenades, grenadesArray);
        setActivatedWeapons(isActivatedPistols, pistolsArray);

        playerScript = swatTeam[0].GetComponent<Player>();

        healthText = gameCanvas.transform.GetChild(7).gameObject.GetComponent<Text>();
        roundCounterText = gameCanvas.transform.GetChild(8).gameObject.GetComponent<Text>();
        winsCountText = gameCanvas.transform.GetChild(9).gameObject.GetComponent<Text>();
        roundEndMessage = gameCanvas.transform.GetChild(10).gameObject.GetComponent<Text>();

        chosenNumberOfRounds = numberOfRounds[1];
        currentRound = 1;
        ninjaTeamRoundsWon = 0;
        swatTeamRoundsWon = 0;


    }

    void Update()
    {
        healthText.text = playerScript.getHealth().ToString();
        roundCounterText.text = "Round " + currentRound + " / " + chosenNumberOfRounds;
        winsCountText.text = "Rounds won: " + swatTeamRoundsWon;

        isRoundFinished();

    }

    void setActivatedWeapons(bool[] isActivatedWeaponsArray, GameObject[] weapons)
    {
        int numOfActivatedWeapon = (int)(isActivatedWeaponsArray.Length * ACTIVATED_WEAPONS_PERCENTAGE);

        for (int i = 0; i < numOfActivatedWeapon; i++)
        {
            isActivatedWeaponsArray[i] = true;
        }

        shuffleWeapons(isActivatedWeaponsArray);

        for (int i = 0; i < isActivatedWeaponsArray.Length; i++)
        {
            if (isActivatedWeaponsArray[i])
            {
                weapons[i].SetActive(true);
            }
        }

    }

    void shuffleWeapons(bool[] isActivatedWeaponsArray)
    {
        for (int i = 0; i < isActivatedWeaponsArray.Length; i++)
        {
            int random = Random.Range(0, isActivatedWeaponsArray.Length);
            bool temp = isActivatedWeaponsArray[random];
            isActivatedWeaponsArray[random] = isActivatedWeaponsArray[i];
            isActivatedWeaponsArray[i] = temp;
        }
    }

    void isRoundFinished()
    {
        // check if a team has players who are all dead
        // show message in middle screen accordingly
        isGameFinished();
    }

    void isGameFinished()
    {

    }
}
