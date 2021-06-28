using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterWeapons : MonoBehaviour
{
    const float ACTIVATED_WEAPONS_PERCENTAGE = 0.5f;

    [SerializeField] GameObject grenades;
    [SerializeField] GameObject pistols;
    GameObject[] grenadesArray;
    GameObject[] pistolsArray;


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
