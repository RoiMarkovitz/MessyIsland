using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInput : MonoBehaviour
{
    
    TMPro.TMP_InputField inputField;
 
    [SerializeField] GameObject gameManager;
    GameManager gameManagerScript;

    public void updateNickname()
    {     
        gameManagerScript.setNickname(inputField.text);             
    }

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        inputField = GetComponent<TMPro.TMP_InputField>();

    }

    
    void Update()
    {
        
    }

}
