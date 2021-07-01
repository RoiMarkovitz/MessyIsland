using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInput : MonoBehaviour
{
    
    TMPro.TMP_InputField inputField;
 
    public void updateNickname()
    {

        if (inputField.text.Length == 0)
        {
            GameManager.instance.setNickname(GameManager.DEFAULT_NICKNAME);
        }
        else if (inputField.text.Length > 10)
        {
            GameManager.instance.setNickname(inputField.text.Substring(0, 10));
        }
        else
        {
            GameManager.instance.setNickname(inputField.text);
        }

    }

    void Start()
    {     
        inputField = GetComponent<TMPro.TMP_InputField>();
    }

    
    void Update()
    {
        
    }

}
