using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryStatUIObject : MonoBehaviour
{
    public Text StatNameText;
    public Text StatValueText;

    void Start()
    {

    }

    void Update()
    {

    }

    public void UpdateStat(KeyValuePair<string, string> data)
    {
        StatNameText.text = data.Key;
        StatValueText.text = data.Value;
    }
}
