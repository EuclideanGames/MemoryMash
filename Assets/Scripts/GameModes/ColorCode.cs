using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCode : GameMode
{
    public int CurrentScore
    {
        get { return currentScore; }
        set
        {
            currentScore = value;
            UpdateScore(value);
        }
    }

    public float TimeRemaining
    {
        get { return timeRemaining; }
        set
        {
            timeRemaining = value;
            UpdateTimer(value);
        }
    }

    private int currentScore;
    private float timeRemaining;

    public ColorCode()
    {
        
    }

    public override void InitializeGame()
    {
        base.InitializeGame();

        SummaryTitle = "Out of Time!";
        CurrentScore = 0;
        TimeRemaining = SelectedDifficulty.ColorCodeStartTimer;
    }

    public override IEnumerator InitializeGrid()
    {
        for (int i = 0; i < SelectedDifficulty.ActiveSquareCount; i++)
        {
            AddSquareToQueue();
            yield return new WaitForSeconds(0.5f);
        }

        Running = true;
    }

    public override void CleanupGame()
    {
        base.CleanupGame();
    }

    public override void PostScore()
    {
        base.PostScore();
    }

    public override Dictionary<string, string> GetGameSummary()
    {
        return new Dictionary<string, string>()
        {
            { "Final Score", CurrentScore.ToString() },
            { "Highest Combo", HighestCombo.ToString() },
            { "Good Hits", GoodHits.ToString() },
            { "Bad Hits", BadHits.ToString() },
            { "Accuracy", ((float)GoodHits / (GoodHits + BadHits)).ToString("P") }
        };
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        TimeRemaining -= Time.deltaTime;

        if(TimeRemaining <= 0.0f)
            EndGame();
    }

    public override void HandleSquareHit(object sender, OnSquareHitEventArgs args)
    {
        base.HandleSquareHit(sender, args);
    }

    public override void OnGoodHit()
    {
        base.OnGoodHit();

        CurrentScore += 1 + (Combo / SelectedDifficulty.ComboMultiplierRequirement);
    }

    public override void OnBadHit()
    {
        base.OnBadHit();

        TimeRemaining -= SelectedDifficulty.BadHitTimePenalty;
    }

    public override int AddSquareToQueue()
    {
        return base.AddSquareToQueue();

        Square newSquare = ActiveFactory.CreateRandomSquare();


    }
}
