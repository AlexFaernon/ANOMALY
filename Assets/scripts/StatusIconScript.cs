using UnityEngine;
using UnityEngine.UI;

public class StatusIconScript : MonoBehaviour
{
    public void ChangeSprite(Status status)
    {
        GetComponent<Image>().sprite = status.Icon;
    }
}
