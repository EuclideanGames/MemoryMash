using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Anchor : GameMode
{
    private const float timeToHitAnchorSpawn = 1.0f;

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

    private Square anchorSquare;
    private int anchorIndex;
    private int currentScore;
    private float timeRemaining;
    private float timeSinceAnchorHold;
    private float timeWithoutAnchorHold;

    public Anchor()
    {

    }

    public override void InitializeGame()
    {
        base.InitializeGame();

        SummaryTitle = "Game Over!";
        CurrentScore = 0;
        TimeRemaining = SelectedDifficulty.AnchorStartTimer;

        timeSinceAnchorHold = 0.0f;
        timeWithoutAnchorHold = 0.0f;
    }

    public override IEnumerator InitializeGrid()
    {
        SwitchAnchor();

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
            { "Final Score", currentScore.ToString() },
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

        if (timeRemaining <= 0.0f)
        {
            SummaryTitle = "Out of Time!";
            EndGame();
            return;
        }

        timeWithoutAnchorHold += Time.deltaTime;
        if(timeWithoutAnchorHold >= timeToHitAnchorSpawn)
        {
            SummaryTitle = "Anchors Away!";
            EndGame();
            return;
        }

        timeSinceAnchorHold += Time.deltaTime;
        if(timeSinceAnchorHold >= SelectedDifficulty.AnchorSwitchTimer)
        {
            SwitchAnchor();
            timeSinceAnchorHold = 0.0f;
        }
    }

    public override void HandleSquareHit(object sender, OnSquareHitEventArgs args)
    {
        base.HandleSquareHit(sender, args);

        Square hit = sender as Square;

        if (hit == null)
        {
            return;
        }

        if (hit.SquareIndex == SpawnOrder.Peek())
        {
            //good hit
            OnGoodHit();

            AddSquareToQueue();
            hit.DestroySquare();
            SpawnOrder.Dequeue();
        }
        else
        {
            //bad hit
            OnBadHit();
        }
    }

    public override void HandleSquareHeld(object sender, OnSquareHeldEventArgs args)
    {
        base.HandleSquareHeld(sender, args);

        Square hit = sender as Square;

        if (hit == null)
        {
            return;
        }

        if (hit.SquareIndex == anchorIndex)
        {
            timeWithoutAnchorHold = 0.0f;
        }
    }

    public override void OnGoodHit()
    {
        base.OnGoodHit();

        CurrentScore += 1 + (Combo / SelectedDifficulty.ComboMultiplierRequirement);
    }

    public override void OnBadHit()
    {
        base.OnBadHit();
    }

    public override int AddSquareToQueue()
    {
        Square newSquare = ActiveFactory.CreateRandomSquare();

        if (newSquare == null)
        {
            return -1;
        }

        newSquare.OnSquareHit += HandleSquareHit;

        SpawnOrder.Enqueue(newSquare.SquareIndex);

        return newSquare.SquareIndex;
    }

    public void SwitchAnchor()
    {
        if (anchorSquare != null)
        {
            anchorSquare.DestroySquare();
        }

        anchorSquare = ActiveFactory.CreateRandomSquare();

        if (anchorSquare == null)
        {
            throw new Exception("Failed to create anchor");
        }

        anchorSquare.OnSquareHeld += HandleSquareHeld;
        anchorSquare.SetColor(Color.blue);

        anchorIndex = anchorSquare.SquareIndex;

        timeWithoutAnchorHold = 0.0f;
    }
}
