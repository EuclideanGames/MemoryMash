using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class GameMode
{
    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged = (sender, e) => { };
    public event EventHandler<OnTimerChangedEventArgs> OnTimerChanged = (sender, e) => { };
    public event EventHandler<OnLivesChangedEventArgs> OnLivesChanged = (sender, e) => { };
    public event EventHandler<OnGameEndedEventArgs> OnGameEnded = (sender, e) => { };

    public SquareFactory ActiveFactory;

    public Difficulty SelectedDifficulty;
    public string SummaryTitle;
    public bool Running;

    protected Queue<int> SpawnOrder;
    protected int Combo;
    protected int HighestCombo;
    protected int GoodHits;
    protected int BadHits;

    protected GameMode()
    {
        
    }

    public virtual void InitializeGame()
    {
        ActiveFactory.InitializeFactory(SelectedDifficulty.GridDefinition);

        SpawnOrder = new Queue<int>();
        Combo = 0;
        HighestCombo = 0;
        GoodHits = 0;
        BadHits = 0;
    }

    public virtual IEnumerator InitializeGrid()
    {
        yield return null;
    }

    public virtual void CleanupGame()
    {
        Running = false;

        ActiveFactory.ClearGrid();
        Object.Destroy(ActiveFactory.gameObject);
    }

    public virtual void PostScore()
    {

    }

    public virtual Dictionary<string, string> GetGameSummary()
    {
        return null;
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void HandleSquareHit(object sender, OnSquareHitEventArgs args)
    {
        
    }

    public virtual void HandleSquareHeld(object sender, OnSquareHeldEventArgs args)
    {
        
    }

    public virtual void OnGoodHit()
    {
        Combo++;
        if (Combo > HighestCombo)
            HighestCombo = Combo;
        GoodHits++;
    }

    public virtual void OnBadHit()
    {
        Combo = 0;
        BadHits++;
    }

    public virtual int AddSquareToQueue()
    {
        return -1;
    }

    public virtual void UpdateScore(int score)
    {
        OnScoreChangedEventArgs args = new OnScoreChangedEventArgs() {NewScore = score};
        OnScoreChanged.Invoke(this, args);
    }

    public virtual void UpdateTimer(float timer)
    {
        OnTimerChangedEventArgs args = new OnTimerChangedEventArgs() {NewTime = timer};
        OnTimerChanged.Invoke(this, args);
    }

    public virtual void UpdateLives(int lives)
    {
        OnLivesChangedEventArgs args = new OnLivesChangedEventArgs() {NewLives = lives};
        OnLivesChanged.Invoke(this, args);
    }

    public virtual void EndGame()
    {
        CleanupGame();
        PostScore();

        OnGameEndedEventArgs args = new OnGameEndedEventArgs();
        OnGameEnded.Invoke(this, args);
    }
}

public class OnScoreChangedEventArgs : EventArgs
{
    public int NewScore;
}

public class OnTimerChangedEventArgs : EventArgs
{
    public float NewTime;
}

public class OnLivesChangedEventArgs : EventArgs
{
    public int NewLives;
}

public class OnGameEndedEventArgs : EventArgs
{
    
}
