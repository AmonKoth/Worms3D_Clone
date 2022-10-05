using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour
{
    [SerializeField]
    private float _resetTime = 2.0f;
    private Text _turnText = null;
    // Start is called before the first frame update
    void Start()
    {
        _turnText = this.GetComponent<Text>();
        ResetText();
    }

    private void ResetText()
    {
        _turnText.text = " ";
    }
    public void SetText(string text)
    {
        _turnText.text = text;
        Invoke("ResetText", _resetTime);
    }
}
