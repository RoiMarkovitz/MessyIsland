using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInput : MonoBehaviour
{
    string nickname;
    TMPro.TMP_InputField inputField;
    [SerializeField] TMPro.TextMeshProUGUI placeholderNickname;

    public void updateNickname()
    {
        nickname = inputField.text;     
    }

    void Start()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();
        nickname = placeholderNickname.text;     
    }

    
    void Update()
    {
        
    }

    public string getNickname()
    {
        return nickname;
    }
}
