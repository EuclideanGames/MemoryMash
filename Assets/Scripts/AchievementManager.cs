using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class ScoreAchievement
{
    private string achievementID;
    private int achievementScore;

    public ScoreAchievement(string id, int score)
    {
        achievementID = id;
        achievementScore = score;
    }

    public void CheckAchievement(int score)
    {
        if (score >= achievementScore)
            Social.ReportProgress(achievementID, 100.0f, (bool success) => { });
    }
}

public class GamesPlayedAchievement
{
    private string achievementID;

    public GamesPlayedAchievement(string id, int games)
    {
        achievementID = id;
    }

    public void IncrementAchievementProgress(int games)
    {
        //PlayGamesPlatform.Instance.IncrementAchievement(achievementID, games, (bool success) => { });
    }
}

public class TimeSurvivedAchievement
{
    private string achievementID;
    private float achievementTime;

    public TimeSurvivedAchievement(string id, float time)
    {
        achievementID = id;
        achievementTime = time;
    }

    public void CheckAchievement(float time)
    {
        if (time >= achievementTime)
            Social.ReportProgress(achievementID, 100.0f, (bool success) => { });
    }
}

public class AchievementManager
{
    public static ScoreAchievement[] EasyScoreAchievements =
    {
        new ScoreAchievement(GPGSIds.achievement_easy_score_novice, 100),
        new ScoreAchievement(GPGSIds.achievement_easy_score_apprentice, 250),
        new ScoreAchievement(GPGSIds.achievement_easy_score_journeyman, 500),
        new ScoreAchievement(GPGSIds.achievement_easy_score_expert, 1000),
        new ScoreAchievement(GPGSIds.achievement_easy_score_adept, 2500),
        new ScoreAchievement(GPGSIds.achievement_easy_score_master, 5000),
        new ScoreAchievement(GPGSIds.achievement_easy_score_grandmaster, 10000),
    };

    public static ScoreAchievement[] MediumScoreAchievements =
    {
        new ScoreAchievement(GPGSIds.achievement_medium_score_novice, 250),
        new ScoreAchievement(GPGSIds.achievement_medium_score_apprentice, 500),
        new ScoreAchievement(GPGSIds.achievement_medium_score_journeyman, 1000),
        new ScoreAchievement(GPGSIds.achievement_medium_score_expert, 2500),
        new ScoreAchievement(GPGSIds.achievement_medium_score_adept, 5000),
        new ScoreAchievement(GPGSIds.achievement_medium_score_master, 10000),
        new ScoreAchievement(GPGSIds.achievement_medium_score_grandmaster, 15000),
    };

    public static ScoreAchievement[] HardScoreAchievements =
    {
        new ScoreAchievement(GPGSIds.achievement_hard_score_novice, 100),
        new ScoreAchievement(GPGSIds.achievement_hard_score_apprentice, 200),
        new ScoreAchievement(GPGSIds.achievement_hard_score_journeyman, 300),
        new ScoreAchievement(GPGSIds.achievement_hard_score_expert, 500),
        new ScoreAchievement(GPGSIds.achievement_hard_score_adept, 1000),
        new ScoreAchievement(GPGSIds.achievement_hard_score_master, 2500),
        new ScoreAchievement(GPGSIds.achievement_hard_score_grandmaster, 5000),
    };

    public static ScoreAchievement[] InsaneScoreAchievements =
    {
        new ScoreAchievement(GPGSIds.achievement_insane_score_novice, 100),
        new ScoreAchievement(GPGSIds.achievement_insane_score_apprentice, 200),
        new ScoreAchievement(GPGSIds.achievement_insane_score_journeyman, 300),
        new ScoreAchievement(GPGSIds.achievement_insane_score_expert, 400),
        new ScoreAchievement(GPGSIds.achievement_insane_score_adept, 500),
        new ScoreAchievement(GPGSIds.achievement_insane_score_master, 1000),
        new ScoreAchievement(GPGSIds.achievement_insane_score_grandmaster, 2500),
    };

    public static TimeSurvivedAchievement[] EasyTimeAchievements =
    {
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_novice, 30),
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_apprentice, 60),
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_journeyman, 120),
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_expert, 180),
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_adept, 240),
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_master, 300),
        new TimeSurvivedAchievement(GPGSIds.achievement_easy_survival_grandmaster, 360),
    };

    public static TimeSurvivedAchievement[] MediumTimeAchievements =
    {
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_novice, 30),
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_apprentice, 60),
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_journeyman, 120),
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_expert, 180),
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_adept, 240),
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_master, 300),
        new TimeSurvivedAchievement(GPGSIds.achievement_medium_survival_grandmaster, 360),
    };

    public static TimeSurvivedAchievement[] HardTimeAchievements =
    {
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_novice, 30),
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_apprentice, 60),
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_journeyman, 120),
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_expert, 180),
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_adept, 240),
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_master, 300),
        new TimeSurvivedAchievement(GPGSIds.achievement_hard_survival_grandmaster, 360),
    };

    public static TimeSurvivedAchievement[] InsaneTimeAchievements =
    {
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_novice, 30),
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_apprentice, 60),
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_journeyman, 120),
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_expert, 180),
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_adept, 240),
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_master, 300),
        new TimeSurvivedAchievement(GPGSIds.achievement_insane_survival_grandmaster, 360),
    };

    public static GamesPlayedAchievement[] GamesAchievements =
    {
        new GamesPlayedAchievement(GPGSIds.achievement_play_10_games, 10),
        new GamesPlayedAchievement(GPGSIds.achievement_play_100_games, 100),
        new GamesPlayedAchievement(GPGSIds.achievement_play_1000_games, 1000),
    };

    public static void CheckScoreAchievements(string diffName, int score)
    {
        switch (diffName)
        {
            case "Easy":
                foreach (ScoreAchievement achieve in EasyScoreAchievements)
                    achieve.CheckAchievement(score);
                break;
            case "Medium":
                foreach (ScoreAchievement achieve in MediumScoreAchievements)
                    achieve.CheckAchievement(score);
                break;
            case "Hard":
                foreach (ScoreAchievement achieve in HardScoreAchievements)
                    achieve.CheckAchievement(score);
                break;
            case "Insane":
                foreach (ScoreAchievement achieve in InsaneScoreAchievements)
                    achieve.CheckAchievement(score);
                break;
        }
    }

    public static void CheckTimeAchievements(string diffName, float time)
    {
        switch (diffName)
        {
            case "Easy":
                foreach (TimeSurvivedAchievement achieve in EasyTimeAchievements)
                    achieve.CheckAchievement(time);
                break;
            case "Medium":
                foreach (TimeSurvivedAchievement achieve in MediumTimeAchievements)
                    achieve.CheckAchievement(time);
                break;
            case "Hard":
                foreach (TimeSurvivedAchievement achieve in HardTimeAchievements)
                    achieve.CheckAchievement(time);
                break;
            case "Insane":
                foreach (TimeSurvivedAchievement achieve in InsaneTimeAchievements)
                    achieve.CheckAchievement(time);
                break;
        }
    }

    public static void IncrementGamesAchievements(int games)
    {
        foreach (GamesPlayedAchievement achieve in GamesAchievements)
            achieve.IncrementAchievementProgress(games);
    }
}
