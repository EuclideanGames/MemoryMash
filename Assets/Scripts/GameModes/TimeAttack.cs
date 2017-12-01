using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAttack : GameMode
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

    public TimeAttack()
    {

    }

    public override void InitializeGame()
    {
        base.InitializeGame();

        SummaryTitle = "Out of Time!";
        CurrentScore = 0;
        TimeRemaining = SelectedDifficulty.TimeAttackStartTimer;
    }

    public override IEnumerator InitializeGrid()
    {
        for(int i = 0; i < SelectedDifficulty.ActiveSquareCount; i++)
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

        string leaderboard;
        string diff_name = SelectedDifficulty.Name;

        switch (diff_name)
        {
            case "Easy":
                leaderboard = GPGSIds.leaderboard_easy_score;
                break;
            case "Medium":
                leaderboard = GPGSIds.leaderboard_medium_score;
                break;
            case "Hard":
                leaderboard = GPGSIds.leaderboard_hard_score;
                break;
            case "Insane":
                leaderboard = GPGSIds.leaderboard_insane_score;
                break;
            default:
                return;
        }

        Social.Active.ReportScore(currentScore, leaderboard, (bool success) => { });

        AchievementManager.CheckScoreAchievements(diff_name, currentScore);
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
            EndGame();
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
        Square newSquare = ActiveFactory.CreateRandomSquare();

        if (newSquare == null)
        {
            return -1;
        }

        newSquare.OnSquareHit += HandleSquareHit;

        SpawnOrder.Enqueue(newSquare.SquareIndex);

        return newSquare.SquareIndex;
    }
}
