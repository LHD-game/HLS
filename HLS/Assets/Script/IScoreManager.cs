using System.Collections.Generic;

public interface IScoreManager
{
    void AddScore(int questionIndex, int answerIndex);
    void ResetScores();
    void SetData();

    int totalScore { get; }
    Dictionary<string, object> ScoreData { get; }
}
