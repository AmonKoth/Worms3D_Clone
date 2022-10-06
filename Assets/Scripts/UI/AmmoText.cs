using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{

    private Text _ammoText = null;

    private void Start()
    {
        _ammoText = this.GetComponent<Text>();
        ClearText();
    }
    public void SetAmmoText(string text)
    {
        _ammoText.text = text;
    }

    public void ClearText()
    {
        _ammoText.text = " ";
    }
}
