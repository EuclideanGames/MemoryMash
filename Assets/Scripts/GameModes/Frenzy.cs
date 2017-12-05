using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;

public class Frenzy : GameMode
{
    private const float timeBetweenTimerUpdates = 5.0f;

    public float TimeSurvived
    {
        get { return timeSurvived; }
        set
        {
            timeSurvived = value;
            UpdateTimer(value);
        }
    }

    private float timeSurvived;
    private float currentSpawnTimer;
    private float timeSinceLastSpawn;
    private float timeSinceLastUpdate;

    public Frenzy()
    {

    }

    public override void InitializeGame()
    {
        base.InitializeGame();

        SummaryTitle = "Game Over!";
        TimeSurvived = 0.0f;

        currentSpawnTimer = SelectedDifficulty.InitialSpawnTimer;
        timeSinceLastSpawn = 0.0f;
        timeSinceLastUpdate = 0.0f;
    }

    public override IEnumerator InitializeGrid()
    {
        AddSquareToQueue();

        yield return null;

        Running = true;
    }

    public override void CleanupGame()
    {
        base.CleanupGame();
    }

    public override void PostScore()
    {
        if (!Social.localUser.authenticated)
        {
            return;
        }

        base.PostScore();

        string leaderboard;
        string diff_name = SelectedDifficulty.name;

        switch (diff_name)
        {
            case "Easy":
                leaderboard = GPGSIds.leaderboard_easy_frenzy;
                break;
            case "Medium":
                leaderboard = GPGSIds.leaderboard_medium_frenzy;
                break;
            case "Hard":
                leaderboard = GPGSIds.leaderboard_hard_frenzy;
                break;
            case "Insane":
                leaderboard = GPGSIds.leaderboard_insane_frenzy;
                break;
            default:
                return;
        }

        Social.Active.ReportScore(Mathf.FloorToInt(currentSpawnTimer), 
            leaderboard, (bool success) => { });

        AchievementManager.CheckTimeAchievements("Frenzy", diff_name, timeSurvived);
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

        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= currentSpawnTimer)
        {
            if (AddSquareToQueue() == -1)
            {
                EndGame();
                return;
            }

            timeSinceLastSpawn = 0.0f;
        }

        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= timeBetweenTimerUpdates)
        {
            currentSpawnTimer *= SelectedDifficulty.TimerDecayRate;
            timeSinceLastUpdate = 0.0f;
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

    public override void OnGoodHit()
    {
        base.OnGoodHit();
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
}
