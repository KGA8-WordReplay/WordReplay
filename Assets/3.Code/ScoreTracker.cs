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

        public LimitedTimeByScore(int score, float time)
        {
            this.score = score;
            this.time = time;
        }
    }

    [Header("점수 관련 변수")]
    public int basicScore = 10;
    public int maxScore;
    public int CurScore { get; set; }

    public List<LimitedTimeByScore> limitedTimeByScores = new List<LimitedTimeByScore>();

    private void OnValidate()
    {
        UpdateLimitedTimeByScores();
    }

    private void UpdateLimitedTimeByScores()
    {
        if (maxScore <= 0) return;

        limitedTimeByScores.Clear();

        // 제한 시간 설정 (백분율 기준)
        limitedTimeByScores.Add(new LimitedTimeByScore(Mathf.FloorToInt(maxScore * 0.25f), 20f)); // 0~25%
        limitedTimeByScores.Add(new LimitedTimeByScore(Mathf.FloorToInt(maxScore * 0.50f), 15f)); // 25~50%
        limitedTimeByScores.Add(new LimitedTimeByScore(Mathf.FloorToInt(maxScore * 0.75f), 10f)); // 50~75%
        limitedTimeByScores.Add(new LimitedTimeByScore(Mathf.FloorToInt(maxScore * 0.90f), 8f));  // 75~90%
        limitedTimeByScores.Add(new LimitedTimeByScore(maxScore, 6f));  // 90~100%

        Debug.Log("limitedTimeByScores 리스트가 자동으로 업데이트됨!");
    }

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
