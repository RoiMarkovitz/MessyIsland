using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundManager : MonoBehaviour
{
    const string NINJAS_WON = "Ninjas Won The Round";
    const string SWAT_WON = "Swat Won The Round";

    bool isTeamWonTheRound;

    [SerializeField] GameObject gameManager;
    GameManager gameManagerScript;

    [SerializeField] GameObject gameCanvas;

    [SerializeField] GameObject[] swatTeam;
    [SerializeField] GameObject[] ninjaTeam;

  
    // audio clips "Ninjas win the game", "Ninjas win the round"
    // , "Swat win the game", "Swat win the round"

    Player playerScript;

    Text healthText;
    Text roundEndMessage;


    void Start()
    {
        isTeamWonTheRound = false;
        gameManagerScript = gameManager.GetComponent<GameManager>();

        healthText = gameCanvas.transform.GetChild(7).gameObject.GetComponent<Text>();
        roundEndMessage = gameCanvas.transform.GetChild(10).gameObject.GetComponent<Text>();

        playerScript = swatTeam[0].GetComponent<Player>();
        playerScript.setNickname(gameManagerScript.getNickname());
       // Debug.Log(playerScript.getNickname());

        
       // Debug.Log("" + gameManagerScript.getNumberOfRounds());
        
    }

    void Update()
    {
        healthText.text = playerScript.getHealth().ToString();

        isRoundFinished();

    }

    void isRoundFinished()
    {
        if (!isTeamWonTheRound)
        { 
        isAllTeamDead(ninjaTeam, SWAT_WON, true);
        isAllTeamDead(swatTeam, NINJAS_WON, false);
        }
    }

    void isAllTeamDead(GameObject[] team, string winMessage, bool isSwatTeam)
    {
        int deadCount = 0;

        for (int i = 0; i < team.Length; i++)
        {
            if (team[i].gameObject.tag == "Dead")
            {
                deadCount++;
            }
        }

        if (deadCount == team.Length)
        {
            isTeamWonTheRound = true;

            if (isSwatTeam)
            {
                gameManagerScript.givePointToSwatTeam();
            }
            else
            {
                gameManagerScript.givePointToNinjaTeam();
            }


            // play audio
            roundEndMessage.text = winMessage;
            roundEndMessage.gameObject.SetActive(true);

            // wait 3 seconds and start a new round if game is not finished

            // gameManagerScript.isGameFinished();

        }
    }

    
}
