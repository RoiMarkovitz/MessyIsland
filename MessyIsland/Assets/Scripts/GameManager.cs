using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    const string GAME_WON = "Game Won";
    const string GAME_LOST = "Game Lost";

    string nickname = "Player";
    int numberOfRounds = 1;
    int currentRound = 1;
    int ninjaTeamRoundsWon = 0;
    int swatTeamRoundsWon = 0;

    [SerializeField] GameObject canvasMainMenu;
    [SerializeField] GameObject gameCanvas;
    
    Text roundCounterText;
    Text winsCountText;
    Text gameEndMessage;

    void Start()
    {
        
        roundCounterText = gameCanvas.transform.GetChild(8).gameObject.GetComponent<Text>();
        winsCountText = gameCanvas.transform.GetChild(9).gameObject.GetComponent<Text>();
        gameEndMessage = gameCanvas.transform.GetChild(10).gameObject.GetComponent<Text>();
        
    }

   
    void Update()
    {
        roundCounterText.text = "Round " + currentRound + " / " + numberOfRounds;
        winsCountText.text = "Rounds Won: " + swatTeamRoundsWon;

       
        //if (Input.GetKey("t"))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        //}
    }

    public void setNickname(string newNickname)
    {
        nickname = newNickname;
    }

    public string getNickname()
    {
        return nickname;
    }

    public void setNumberOfRounds(int numOfRounds)
    {
        numberOfRounds = numOfRounds;
    }

    public int getNumberOfRounds()
    {
        return numberOfRounds;
    }

    public void givePointToSwatTeam()
    {
        swatTeamRoundsWon++;
        
    }

    public void givePointToNinjaTeam()
    {
        ninjaTeamRoundsWon++;
    }

    public void isGameFinished()
    {
        
    }

}
