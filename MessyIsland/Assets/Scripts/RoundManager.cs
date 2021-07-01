using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundManager : MonoBehaviour
{
    const string NINJAS_WON_ROUND = "Ninjas Won The Round";
    const string SWAT_WON_ROUND = "Swat Won The Round";

    bool isTeamWonTheRound;

    GameObject roundCanvas;

    [SerializeField] GameObject[] swatTeam;
    [SerializeField] GameObject[] ninjaTeam;

    [SerializeField] AudioClip ninjasWonRoundSound;
    [SerializeField] AudioClip swatWonRoundSound;
    AudioSource audioPlayer;

    Player playerScript;

    Text healthText;
    Text roundEndMessage;


    void Start()
    {     
        roundCanvas = GameObject.Find("CanvasRound");

        audioPlayer = GetComponent<AudioSource>();

        isTeamWonTheRound = false;
      
        healthText = roundCanvas.transform.GetChild(7).gameObject.GetComponent<Text>();
        roundEndMessage = roundCanvas.transform.GetChild(8).gameObject.GetComponent<Text>();

        playerScript = swatTeam[0].GetComponent<Player>();
        playerScript.setNickname(GameManager.instance.getNickname());
            
    }

    void Update()
    {
        healthText.text = playerScript.getCurrentHealth().ToString();
        
        isRoundFinished();

    }

    void isRoundFinished()
    {
        if (!isTeamWonTheRound)
        { 
            isAllTeamDead(ninjaTeam, SWAT_WON_ROUND, true);
            isAllTeamDead(swatTeam, NINJAS_WON_ROUND, false);
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
                GameManager.instance.givePointToSwatTeam();
                audioPlayer.PlayOneShot(swatWonRoundSound);

            }
            else
            {
                GameManager.instance.givePointToNinjaTeam();
                audioPlayer.PlayOneShot(ninjasWonRoundSound);         
            }

 
            roundEndMessage.text = winMessage;
            roundEndMessage.gameObject.SetActive(true);
       
            Invoke("isGameFinished", 4.0f);

        }
    }

    void isGameFinished()
    {
        setRoundCanvasToDefault();
        GameManager.instance.isGameFinished(this.gameObject);      
    }

    public void setRoundCanvasToDefault()
    {
        roundCanvas.transform.GetChild(8).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(2).gameObject.SetActive(true);
        roundCanvas.transform.GetChild(3).gameObject.SetActive(true);
        roundCanvas.transform.GetChild(4).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(5).gameObject.SetActive(false);
    }

    
}
