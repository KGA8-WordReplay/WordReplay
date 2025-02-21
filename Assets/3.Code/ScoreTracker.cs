using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [Serializable]
    public struct LimitedTimeByScore
    {
        public int score;
        public float time;
    }

    [Header("점수 관련 변수")]
    public int basicScore = 10;
    public int maxScore;
    public int CurScore { get; set; }

    public List<LimitedTimeByScore> limitedTimeByScores = new List<LimitedTimeByScore>();

    public float GetLimitedTime()
    {
        return GetLimitedTimeByScore().time;
    }

    private LimitedTimeByScore GetLimitedTimeByScore()
    {
        foreach (LimitedTimeByScore limitedTimeByScore in limitedTimeByScores)
        {
            if (CurScore <= limitedTimeByScore.score)
                return limitedTimeByScore;
        }
        return limitedTimeByScores[limitedTimeByScores.Count - 1];
    }

    public void CalcScoreByLength(string word)
    {
        int bonus = (word.Length - 2) * 5;
        bonus = bonus >= 0 ? bonus : 0;
        int result = basicScore + bonus;
        CurScore += result;
    }

    public bool IsMaxScore()
    {
        return CurScore >= maxScore ? true : false;
    }
}
