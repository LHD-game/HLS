using System.Collections.Generic;

public interface IScoreManager
{
    void AddScore(int questionIndex, int answerIndex);
    void ResetScores();
    void SetData();
    Dictionary<string, string> ScoreData { get; }
}
