using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    const float ACTIVATED_WEAPONS_PERCENTAGE = 0.5f;

    [SerializeField] GameObject[] grenades;
    [SerializeField] GameObject[] pistols;
    [SerializeField] GameObject[] swatTeam;
    [SerializeField] GameObject[] ninjaTeam;


    void Start()
    {
        bool[] isActivatedGrenades = new bool[grenades.Length];
        bool[] isActivatedPistols = new bool[pistols.Length];

        setActivatedWeapons(isActivatedGrenades, grenades);
        setActivatedWeapons(isActivatedPistols, pistols);

    }

    void Update()
    {

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
}
