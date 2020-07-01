using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public interface IScoreTrackerService
    {
        public void AddToTotalScore(int score);
        public void RemoveFromTotalScore(int score);
        public string GetTotalScore();

        event Action OnChange;
    }
}
