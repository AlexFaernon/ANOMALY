using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountText : MonoBehaviour
{
    [SerializeField] private TMP_Text count;

    private void Awake()
    {
        var newCount = BattleResultsSingleton.LevelsCompleted;
        count.text = newCount.ToString();
    }
}
