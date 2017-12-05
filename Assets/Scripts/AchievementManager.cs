using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class ScoreAchievement
{
    private readonly string achievementID;
    private readonly int achievementScore;

    public ScoreAchievement(string id, int score)
    {
        achievementID = id;
        achievementScore = score;
    }

    public void CheckAchievement(int score)
    {
        if (score >= achievementScore)
        {
            Social.ReportProgress(achievementID, 100.0f, success => { });
        }
    }
}

public class TimeAchievement
{
    private readonly string achievementID;
    private readonly float achievementTime;

    public TimeAchievement(string id, float time)
    {
        achievementID = id;
        achievementTime = time;
    }

    public void CheckAchievement(float time)
    {
        if (time >= achievementTime)
        {
            Social.ReportProgress(achievementID, 100.0f, success => { });
        }
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

public class AchievementManager
{
    public static Dictionary<string, Dictionary<string, List<ScoreAchievement>>> ScoreAchievements =
        new Dictionary<string, Dictionary<string, List<ScoreAchievement>>>()
        {
            {
                "TimeAttack",
                new Dictionary<string, List<ScoreAchievement>>()
                {
                    {
                        "Easy",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_easy_time_attack_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_easy_time_attack_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_easy_time_attack_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_easy_time_attack_grandmaster, 100),
                        }
                    },
                    {
                        "Medium",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_medium_time_attack_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_medium_time_attack_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_medium_time_attack_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_medium_time_attack_grandmaster, 100),
                        }
                    },
                    {
                        "Hard",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_hard_time_attack_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_hard_time_attack_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_hard_time_attack_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_hard_time_attack_grandmaster, 100),
                        }
                    },
                    {
                        "Insane",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_insane_time_attack_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_insane_time_attack_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_insane_time_attack_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_insane_time_attack_grandmaster, 100),
                        }
                    },
                }
            },
            {
                "Anchor",
                new Dictionary<string, List<ScoreAchievement>>()
                {
                    {
                        "Easy",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_easy_anchor_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_easy_anchor_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_easy_anchor_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_easy_anchor_grandmaster, 100),
                        }
                    },
                    {
                        "Medium",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_medium_anchor_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_medium_anchor_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_medium_anchor_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_medium_anchor_grandmaster, 100),
                        }
                    },
                    {
                        "Hard",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_hard_anchor_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_hard_anchor_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_hard_anchor_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_hard_anchor_grandmaster, 100),
                        }
                    },
                    {
                        "Insane",
                        new List<ScoreAchievement>()
                        {
                            new ScoreAchievement(GPGSIds.achievement_insane_anchor_apprentice, 100),
                            new ScoreAchievement(GPGSIds.achievement_insane_anchor_expert, 100),
                            new ScoreAchievement(GPGSIds.achievement_insane_anchor_master, 100),
                            new ScoreAchievement(GPGSIds.achievement_insane_anchor_grandmaster, 100),
                        }
                    },
                }
            }
        };

    public static Dictionary<string, Dictionary<string, List<TimeAchievement>>> TimeAchievements =
        new Dictionary<string, Dictionary<string, List<TimeAchievement>>>()
        {
            {
                "Survival",
                new Dictionary<string, List<TimeAchievement>>()
                {
                    {
                        "Easy",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_easy_survival_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_easy_survival_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_easy_survival_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_easy_survival_grandmaster, 1.0f),
                        }
                    },
                    {
                        "Medium",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_medium_survival_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_medium_survival_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_medium_survival_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_medium_survival_grandmaster, 1.0f),
                        }
                    },
                    {
                        "Hard",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_hard_survival_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_hard_survival_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_hard_survival_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_hard_survival_grandmaster, 1.0f),
                        }
                    },
                    {
                        "Insane",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_insane_survival_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_insane_survival_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_insane_survival_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_insane_survival_grandmaster, 1.0f),
                        }
                    }
                }
            },
            {
                "Frenzy",
                new Dictionary<string, List<TimeAchievement>>()
                {
                    {
                        "Easy",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_easy_frenzy_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_easy_frenzy_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_easy_frenzy_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_easy_frenzy_grandmaster, 1.0f),
                        }
                    },
                    {
                        "Medium",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_medium_frenzy_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_medium_frenzy_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_medium_frenzy_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_medium_frenzy_grandmaster, 1.0f),
                        }
                    },
                    {
                        "Hard",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_hard_frenzy_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_hard_frenzy_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_hard_frenzy_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_hard_frenzy_grandmaster, 1.0f),
                        }
                    },
                    {
                        "Insane",
                        new List<TimeAchievement>()
                        {
                            new TimeAchievement(GPGSIds.achievement_insane_frenzy_apprentice, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_insane_frenzy_expert, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_insane_frenzy_master, 1.0f),
                            new TimeAchievement(GPGSIds.achievement_insane_frenzy_grandmaster, 1.0f),
                        }
                    },
                }
            }
        };

    public static GamesPlayedAchievement[] GamesAchievements =
    {
        new GamesPlayedAchievement(GPGSIds.achievement_play_10_games, 10),
        new GamesPlayedAchievement(GPGSIds.achievement_play_100_games, 100),
        new GamesPlayedAchievement(GPGSIds.achievement_play_1000_games, 1000),
    };

    public static void CheckScoreAchievements(string mode, string difficulty, int score)
    {
        foreach (ScoreAchievement achievement in ScoreAchievements[mode][difficulty])
        {
            achievement.CheckAchievement(score);
        }
    }

    public static void CheckTimeAchievements(string mode, string difficulty, float time)
    {
        foreach (TimeAchievement achievement in TimeAchievements[mode][difficulty])
        {
            achievement.CheckAchievement(time);
        }
    }

    public static void IncrementGamesAchievements(int games)
    {
        foreach (GamesPlayedAchievement achieve in GamesAchievements)
            achieve.IncrementAchievementProgress(games);
    }
}
