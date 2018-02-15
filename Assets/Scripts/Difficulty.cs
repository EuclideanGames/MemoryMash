using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevel", menuName = "Difficulty")]
public class Difficulty : ScriptableObject
{
    [Header("General Settings")]
    public string Name;
    [Range(1, 8)]
    public int ActiveSquareCount;
    public int ComboMultiplierRequirement;
    public float GoodHitTimeBonus;
    public float BadHitTimePenalty;
    public GridDefinition GridDefinition;

    [Header("Time Attack Settings")]
    public float TimeAttackStartTimer;

    [Header("Survival Settings")]
    public int StartLives;
    public float TimeAllowedBetweenHits;
    public int ExtraLifeComboRequirement;

    [Header("Anchor Settings")]
    public float AnchorStartTimer;
    public float AnchorSwitchTimer;

    [Header("Color Code Settings")]
    public float ColorCodeStartTimer;
    public List<Color> ColorCodeColors;
}
