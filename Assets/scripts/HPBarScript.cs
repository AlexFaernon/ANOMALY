using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HPBarScript : MonoBehaviour
{
    [SerializeField] private GameObject hpSegmentPrefab;
    private ICharacter character;
    private readonly List<GameObject> hpSegments = new List<GameObject>();
    private void Awake()
    {
        EventAggregator.BindHPBarToCharacter.Subscribe(BindHPBarToCharacter);
        EventAggregator.UpdateHP.Subscribe(UpdateHP);
    }

    private void BindHPBarToCharacter(GameObject other, ICharacter character)
    {
        if (gameObject != other) return;

        this.character = character;
        CreateHPSegments();
    }

    private void CreateHPSegments()
    {
        var hp = character.MaxHP;
        var segmentLength = character.HPSegmentLength;
        var hpSegmentsCount = hp / segmentLength + (hp % segmentLength != 0 ? 1 : 0);
        
        for (var i = 0; i < hpSegmentsCount; i++)
        {
            var segment = Instantiate(hpSegmentPrefab, transform);
            int segmentMaxHP;
            if (hp - segmentLength > 0)
            {
                segmentMaxHP = segmentLength;
                hp -= segmentLength;
            }
            else
            {
                segmentMaxHP = hp;
                hp = 0;
            }
            EventAggregator.SetMaxHPSegment.Publish(segment, segmentMaxHP);
            hpSegments.Add(segment);
        }
        
        UpdateHP(character);
    }

    private void UpdateHP(IUnit unit)
    {
        if (character != unit) return;

        var hp = character.HP;
        var segmentLength = character.HPSegmentLength;

        foreach (var hpSegment in hpSegments)
        {
            int hpSegmentValue;
            if (hp - segmentLength > 0)
            {
                hpSegmentValue = segmentLength;
                hp -= segmentLength;
            }
            else
            {
                hpSegmentValue = hp;
                hp = 0;
            }
            
            EventAggregator.UpdateHPSegment.Publish(hpSegment, hpSegmentValue);
        }
        
        Assert.AreEqual(0, hp);
    }

    private void OnDestroy()
    {
        EventAggregator.BindHPBarToCharacter.Unsubscribe(BindHPBarToCharacter);
        EventAggregator.UpdateHP.Unsubscribe(UpdateHP);
    }
}
