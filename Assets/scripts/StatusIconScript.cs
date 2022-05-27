using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIconScript : MonoBehaviour
{
    public void ChangeSprite(Status status)
    {
        GetComponent<Image>().sprite = status.Icon;
    }
}
