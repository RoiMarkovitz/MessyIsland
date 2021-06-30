using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    const string NINJAS_WON_GAME = "Ninjas Won The Game";
    const string SWAT_WON_GAME = "Swat Won The Game";

    string nickname = "Player";
    int numberOfRounds = 1;
    int currentRound = 1;
    int ninjaTeamRoundsWon = 0;
    int swatTeamRoundsWon = 0;
          
    public static GameManager instance = null;

    [SerializeField] GameObject round;

    [SerializeField] GameObject gameCanvas;
    Text roundCounterText;
    Text winsCountText;

    [SerializeField] GameObject gameEndScreen;
    
    [SerializeField] TMPro.TextMeshProUGUI gameEndMessageText;
    [SerializeField] TMPro.TextMeshProUGUI ninjaScoreText;
    [SerializeField] TMPro.TextMeshProUGUI swatScoreText;

    [SerializeField] Text killMessageText;
    Queue<Text> killMessagesQueue = new Queue<Text>();
    Text lastItemInQueue;

    AudioSource audioPlayer;
    [SerializeField] AudioClip ninjasWonGameSound;
    [SerializeField] AudioClip swatWonGameSound;

   
    void Awake()
    {
        if (instance == null)
        {
            instance = this; // asign instance to this instance of the class
        }
        else if(instance != this) // Determine if instance is alerady assigned to something else
        {
            Destroy(gameObject); // do not allow duplicates
        }
       
    }

    void Start()
    {      
        audioPlayer = GetComponent<AudioSource>();

        roundCounterText = gameCanvas.transform.GetChild(0).gameObject.GetComponent<Text>();
        winsCountText = gameCanvas.transform.GetChild(1).gameObject.GetComponent<Text>();
     
    }

   
    void Update()
    {
        roundCounterText.text = "Round " + currentRound + " / " + numberOfRounds;
        winsCountText.text = "Rounds Won: " + swatTeamRoundsWon;
        
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

    public void showKillText(string killer, string victim)
    {
        //killMessageText.gameObject.SetActive(true);
        //killMessageText.text = killer + " Killed " + victim;
        StartCoroutine(playkillText(killer, victim));
       // Invoke("hideKillText", 3.0f);
    }

    public void isGameFinished(GameObject roundObject)
    {        
        if (currentRound == numberOfRounds)
        {
            
            gameCanvas.SetActive(false);
            gameEndScreen.SetActive(true);
            if (ninjaTeamRoundsWon > swatTeamRoundsWon)
            {
                gameEndMessageText.text = NINJAS_WON_GAME;
                audioPlayer.PlayOneShot(ninjasWonGameSound);
            }
            else
            {
                gameEndMessageText.text = SWAT_WON_GAME;
                audioPlayer.PlayOneShot(swatWonGameSound);
            }
            ninjaScoreText.text = "Ninja Score: " + ninjaTeamRoundsWon;
            swatScoreText.text = "Swat Score: " + swatTeamRoundsWon;
              
            Invoke("reloadScene", 4.0f);         
        }
        else
        {
            currentRound++;          
            GameObject newRound = Instantiate(round, round.transform.position, round.transform.rotation);
            newRound.SetActive(true);
            
            Destroy(roundObject);
        }
        
    }

    void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator playkillText(string killer, string victim)
    {
        Text cloneKillMessageText = null;

        if (killMessagesQueue.Count == 0)
        {
            cloneKillMessageText = Instantiate(killMessageText, killMessageText.transform.position, killMessageText.transform.rotation);
        }
                  
        if (killMessagesQueue.Count > 0)
        {
            Vector3 newPosition = new Vector3(lastItemInQueue.transform.position.x, lastItemInQueue.transform.position.y - 100.0f, lastItemInQueue.transform.position.z);
            cloneKillMessageText = Instantiate(lastItemInQueue, lastItemInQueue.transform.position, lastItemInQueue.transform.rotation);
            cloneKillMessageText.transform.position = newPosition;
        }

        cloneKillMessageText.transform.SetParent(gameCanvas.transform);

        killMessagesQueue.Enqueue(cloneKillMessageText);
        lastItemInQueue = cloneKillMessageText; // better be a copy maybe (lastItemInQueue) in-case destroyed

        cloneKillMessageText.gameObject.SetActive(true);
        cloneKillMessageText.text = killer + " Killed " + victim;

        yield return new WaitForSeconds(3.0f);
        // destroy the highest message onscreen
        Destroy(killMessagesQueue.Dequeue().gameObject);
    }

}
