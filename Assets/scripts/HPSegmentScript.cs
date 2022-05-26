using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSegmentScript : MonoBehaviour
{
    [SerializeField] private GameObject hpSquarePrefab;
    [SerializeField] private Sprite hpSprite;
    [SerializeField] private Sprite noHPSprite;
    private int maxHP;
    private readonly List<GameObject> hpSquares = new List<GameObject>();

    private void Awake()
    {
        EventAggregator.SetMaxHPSegment.Subscribe(SetMaxHP);
        EventAggregator.UpdateHPSegment.Subscribe(UpdateHPSegment);
    }

    private void SetMaxHP(GameObject obj, int maxHP)
    {
        if (gameObject != obj) return;

        this.maxHP = maxHP;
        CreateHPBar();
    }

    private void CreateHPBar()
    {
        for (var i = 0; i < maxHP; i++)
        {
            hpSquares.Add(Instantiate(hpSquarePrefab, transform));
        }
    }

    private void UpdateHPSegment(GameObject other, int hp)
    {
        if (gameObject != other) return;

        if (hp == 0)
        {
            GetComponent<Image>().color = Color.gray;
        }
        
        foreach (var hpSquare in hpSquares)
        {
            if (hp > 0)
            {
                hp--;
                hpSquare.GetComponent<Image>().sprite = hpSprite;
            }
            else
            {
                hpSquare.GetComponent<Image>().sprite = noHPSprite;
            }
        }
    }

    private void OnDestroy()
    {
        EventAggregator.SetMaxHPSegment.Unsubscribe(SetMaxHP);
        EventAggregator.UpdateHPSegment.Unsubscribe(UpdateHPSegment);
    }
}
