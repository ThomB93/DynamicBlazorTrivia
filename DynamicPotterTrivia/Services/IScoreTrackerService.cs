using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public interface IScoreTrackerService
    {
        //SCORE
        public void AddToTotalScore(int score, string category);
        public void RemoveFromTotalScore(int score, string category);
        public int GetTotalScore();
        public int GetTotalHPScore();
        public int GetTotalLOTRScore();
        //HINTS-
        public int GetTotalHintsUsed();
        public int GetHPHintsUsed();
        public int GetLOTRHintsUsed();
        public void UpdateHintCounters(string category);

        //CLUES
        public int GetTotalCluesUsed();

        public int GetHPCluesUsed();

        public int GetLOTRCluesUsed();
        public void UpdateClueCounters(string category);

        //ANSWERS
        public int GetTotalCorrectAnswers();

        public int GetTotalWrongAnswers();

        public int GetHPCorrectAnswers();

        public int GetLOTRCorrectAnswers();

        public int GetHPWrongAnswers();

        public int GetLOTRWrongAnswers();
        public void UpdateAnswerCounters(string category, bool correct);
        public string GenerateExportStringFromPropertyValues();
        public void UpdateScoresFromImport(Dictionary<string, string> importedScores);

        event Action OnChange;
    }
}
