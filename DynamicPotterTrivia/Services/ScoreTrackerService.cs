using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public class ScoreTrackerService : IScoreTrackerService
    {
        public int TotalScore { get; set; }
        public int Level { get; set; }

        public event Action OnChange;

        public void AddToTotalScore(int score)
        {
            TotalScore += score;
            StateChanged();
        }

        public void RemoveFromTotalScore(int score)
        {
            TotalScore -= score;
            StateChanged();
        }

        public string GetTotalScore()
        {
            return TotalScore.ToString();
        }

        private void CheckAndUpdateCurrentLevel(int totalScore)
        {
            
        }

        private void StateChanged() => OnChange?.Invoke();
    }
}
