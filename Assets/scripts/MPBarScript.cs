using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPBarScript : MonoBehaviour
{
    [SerializeField] private GameObject hpSquarePrefab;
    private ICharacter character;
    private readonly List<GameObject> mpSquares = new List<GameObject>();

    private void Awake()
    {
        EventAggregator.BindMPBarToCharacter.Subscribe(BindMPBarCharacter);
        EventAggregator.UpdateMP.Subscribe(UpdateMP);
    }

    private void BindMPBarCharacter(GameObject obj, ICharacter character)
    {
        if (gameObject != obj) return;

        this.character = character;
        CreateHPBar();
    }

    private void CreateHPBar()
    {
        var mp = character.MaxMP;

        for (var i = 0; i < mp; i++)
        {
            mpSquares.Add(Instantiate(hpSquarePrefab, transform));
        }
    }

    private void UpdateMP(ICharacter other)
    {
        if (other != character) return;

        var mp = character.MP;
        foreach (var mpSquare in mpSquares)
        {
            if (mp > 0)
            {
                mp--;
                mpSquare.GetComponent<Image>().color = Color.blue;
            }
            else
            {
                mpSquare.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    private void OnDestroy()
    {
        EventAggregator.BindMPBarToCharacter.Unsubscribe(BindMPBarCharacter);
        EventAggregator.UpdateMP.Unsubscribe(UpdateMP);
    }
}
