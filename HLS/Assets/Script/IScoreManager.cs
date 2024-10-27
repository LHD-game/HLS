public interface IScoreManager
{
    void AddScore(int questionIndex, int answerIndex);
    void ResetScores();
}
