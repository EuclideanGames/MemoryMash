using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : GameMode
{
    public float TimeSurvived
    {
        get { return timeSurvived; }
        set
        {
            timeSurvived = value;
            UpdateTimer(value);
        }
    }

    public int LivesRemaining
    {
        get { return livesRemaining; }
        set
        {
            livesRemaining = value;
            UpdateLives(value);
        }
    }

    private float timeSurvived;
    private float timeSinceLastHit;
    private int livesRemaining;

    public Survival()
    {

    }

    public override void InitializeGame()
    {
        base.InitializeGame();

        SummaryTitle = "Out of Lives!";
        TimeSurvived = 0.0f;
        LivesRemaining = SelectedDifficulty.StartLives;

        timeSinceLastHit = 0.0f;
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

        string leaderboard;
        string diff_name = SelectedDifficulty.Name;

        switch (diff_name)
        {
            case "Easy":
                leaderboard = GPGSIds.leaderboard_easy_survival;
                break;
            case "Medium":
                leaderboard = GPGSIds.leaderboard_medium_survival;
                break;
            case "Hard":
                leaderboard = GPGSIds.leaderboard_hard_survival;
                break;
            case "Insane":
                leaderboard = GPGSIds.leaderboard_insane_survival;
                break;
            default:
                return;
        }

        Social.Active.ReportScore(Mathf.FloorToInt(timeSurvived), 
            leaderboard, (bool success) => { });

        AchievementManager.CheckTimeAchievements(diff_name, timeSurvived);
    }

    public override Dictionary<string, string> GetGameSummary()
    {
        return new Dictionary<string, string>()
        {
            { "Time Survived", timeSurvived.ToString("F2") + "s" },
            { "Highest Combo", HighestCombo.ToString() },
            { "Good Hits", GoodHits.ToString() },
            { "Bad Hits", BadHits.ToString() },
            { "Accuracy", ((float)GoodHits / (GoodHits + BadHits)).ToString("P") }
        };
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        TimeSurvived += Time.deltaTime;
        timeSinceLastHit += Time.deltaTime;

        if (timeSinceLastHit >= SelectedDifficulty.TimeAllowedBetweenHits)
        {
            TakeDamage();
            timeSinceLastHit = 0.0f;
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
            OnGoodHit();

            AddSquareToQueue();
            hit.DestroySquare();
            SpawnOrder.Dequeue();
        }
        else
        {
            OnBadHit();
        }
    }

    public override void OnGoodHit()
    {
        base.OnGoodHit();
        
        timeSinceLastHit = 0.0f;

        if (Combo % SelectedDifficulty.ExtraLifeComboRequirement == 0)
        {
            LivesRemaining++;
        }
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

    public void TakeDamage()
    {
        LivesRemaining--;
        if (livesRemaining <= 0)
            EndGame();
    }
}
