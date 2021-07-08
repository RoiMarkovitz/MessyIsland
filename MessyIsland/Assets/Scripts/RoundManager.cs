using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundManager : MonoBehaviour
{
    public enum TeamName { Swat, Ninja };
    const float ACTIVATED_WEAPONS_PERCENTAGE = 0.5f;
    const string NINJAS_WON_ROUND = "Ninjas Won The Round";
    const string SWAT_WON_ROUND = "Swat Won The Round";

    bool hasTeamWonTheRound;

    GameObject roundCanvas;
    Text healthText;
    Text roundEndMessage;
    Player playerScript;

    [SerializeField] GameObject[] swatTeam;
    [SerializeField] GameObject[] ninjaTeam;

    [SerializeField] GameObject grenades;
    [SerializeField] GameObject pistols;
    GameObject[] weapons;

    AudioSource audioPlayer;
    [SerializeField] AudioClip ninjasWonRoundSound;
    [SerializeField] AudioClip swatWonRoundSound;

    [SerializeField] GameObject roundCamera;

    void Start()
    {

        scatterWeaponsRandomly(grenades, pistols);

        audioPlayer = GetComponent<AudioSource>();

        roundCanvas = GameObject.Find("CanvasRound");
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
        if (!hasTeamWonTheRound)
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
            hasTeamWonTheRound = true;

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
        roundCanvas.transform.GetChild(0).gameObject.SetActive(true);
        roundCanvas.transform.GetChild(2).gameObject.SetActive(true);
        roundCanvas.transform.GetChild(3).gameObject.SetActive(true);
        roundCanvas.transform.GetChild(4).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(5).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(8).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(10).gameObject.SetActive(false);
    }

    void scatterWeaponsRandomly(GameObject grenades, GameObject pistols)
    {
        weapons = new GameObject[pistols.transform.childCount + grenades.transform.childCount];
        int k = 0;

        GameObject[] grenadesArray = new GameObject[grenades.transform.childCount];
        for (int i = 0; i < grenadesArray.Length; i++)
        {
            grenadesArray[i] = grenades.transform.GetChild(i).gameObject;
            weapons[k] = grenadesArray[i];
            k++;
        }

        GameObject[] pistolsArray = new GameObject[pistols.transform.childCount];
        for (int i = 0; i < pistolsArray.Length; i++)
        {
            pistolsArray[i] = pistols.transform.GetChild(i).gameObject;
            weapons[k] = pistolsArray[i];
            k++;
        }

        bool[] isActivatedGrenades = new bool[grenadesArray.Length];
        bool[] isActivatedPistols = new bool[pistolsArray.Length];

        setActivatedWeapons(isActivatedGrenades, grenadesArray);
        setActivatedWeapons(isActivatedPistols, pistolsArray);
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

    public bool teamHasMissingWeapons(TeamName name)
    {
        if (name == TeamName.Ninja)
        {
            for (int i = 0; i < ninjaTeam.Length; i++)
            {
                if (ninjaTeam[i].tag != "Dead")
                {
                    NPC npcScript = ninjaTeam[i].GetComponent<NPC>();
                    if (!npcScript.getHasGrenade() || !npcScript.getHasPistol())
                    {
                        return true;
                    }
                }
            }
        }
        else if (name == TeamName.Swat)
        {

            if (swatTeam[1].tag != "Dead")
            {
                NPC npcScript = swatTeam[1].GetComponent<NPC>();
                if (!npcScript.getHasGrenade() || !npcScript.getHasPistol())
                {
                    return true;
                }
            }

        }

        return false; // no missing weapons
    }

    public GameObject[] getWeapons()
    {
        return weapons;
    }

    public GameObject teamLeader(TeamName name)
    {
        if (name == TeamName.Ninja && ninjaTeam[0].tag != "Dead")
        {
            return ninjaTeam[0];

        }
        else if (name == TeamName.Swat && swatTeam[0].tag != "Dead")
        {
            return swatTeam[0];
        }
        else
        {
            return null;
        }
    }

    public GameObject[] getNinjaTeam()
    {
        return ninjaTeam;
    }

    public GameObject[] getSwatTeam()
    {
        return swatTeam;
    }

    public void activateRoundCamera()
    {
        roundCamera.SetActive(true);
        roundCanvas.transform.GetChild(0).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(2).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(3).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(4).gameObject.SetActive(false);
        roundCanvas.transform.GetChild(5).gameObject.SetActive(false);
    }

    public GameObject getRoundCanvas()
    {
        return roundCanvas;
    }



}
