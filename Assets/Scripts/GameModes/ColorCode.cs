using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCode : GameMode
{
    //TODO: Allow multiple squares of the same color
    //TODO: Change hit check to use spawn order for squares of the same color

    public static readonly string HelpText = string.Empty;

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

    private ColorBar mainColorBar;

    private int currentScore;
    private float timeRemaining;

    private List<Color> colorOrder;
    private List<Color> colorSpawnOrder;
    private List<Color> availableColors;

    public ColorCode()
    {
        
    }

    public override void InitializeGame()
    {
        base.InitializeGame();

        SummaryTitle = "Out of Time!";
        CurrentScore = 0;
        TimeRemaining = SelectedDifficulty.ColorCodeStartTimer;

        mainColorBar = GameObject.Find("MainColorBar").GetComponent<ColorBar>();

        colorOrder = new List<Color>();
        colorSpawnOrder = new List<Color>();
        availableColors = new List<Color>();
        
        foreach (Color color in SelectedDifficulty.ColorCodeColors)
        {
            colorOrder.Add(color);
            availableColors.Add(color);
        }

        colorOrder.Shuffle();

        mainColorBar.DisplayColorList(colorOrder);
    }

    public override IEnumerator InitializeGrid()
    {
        mainColorBar.Show();

        for (int i = 0; i < SelectedDifficulty.ActiveSquareCount; i++)
        {
            AddSquareToQueue();
            yield return new WaitForSeconds(0.5f);
        }

        Running = true;

        mainColorBar.Hide();
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

        Square hit = sender as Square;

        if (hit == null)
        {
            return;
        }

        if (hit.GetColor() == colorSpawnOrder[0])
        {
            OnGoodHit();

            colorSpawnOrder.RemoveAt(0);
            availableColors.Add(hit.GetColor());

            AddSquareToQueue();
            hit.DestroySquare();
        }
        else
        {
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

        Color nextColor = GetAvailableColor();

        availableColors.Remove(nextColor);
        newSquare.SetColor(nextColor);
        int colorIndex = 
            Mathf.Min(colorOrder.IndexOf(nextColor), Mathf.Max(colorSpawnOrder.Count - 1, 0));
        colorSpawnOrder.Insert(colorIndex, nextColor);

        return newSquare.SquareIndex;
    }

    public Color GetAvailableColor()
    {
        return availableColors[Random.Range(0, availableColors.Count)];
    }
}
