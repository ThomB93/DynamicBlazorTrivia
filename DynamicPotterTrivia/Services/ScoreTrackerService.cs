using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public class ScoreTrackerService : IScoreTrackerService
    {
        //SCORE PROPERTIES
        private int TotalScore { get; set; } = 30;
        private int HP_Score { get; set; } = 10;
        private int LOTR_Score { get; set; } = 20;
        //HINT PROPERTIES
        private int TotalHintsUsed { get; set; }
        private int HPHintsUsed { get; set; }
        private int LOTRHintsUsed { get; set; }
        //CLUES PROPERTIES
        private int TotalCluesUsed { get; set; }
        private int HPCluesUsed { get; set; }
        private int LOTRCluesUsed { get; set; }
        //ANSWERS PROPERTIES
        private int TotalCorrectAnswers { get; set; } = 5;
        private int TotalWrongAnswers { get; set; } = 3;
        private int CorrectHPAnswers { get; set; }
        private int CorrectLOTRAnswers { get; set; }
        private int WrongHPAnswers { get; set; }
        private int WrongLOTRAnswers { get; set; }

        //may be implemented later
        public int Level { get; set; }

        public event Action OnChange;

        //SCORE METHODS
        public void AddToTotalScore(int score, string category)
        {
            switch (category)
            {
                case "HP":
                    TotalScore += score;
                    HP_Score += score;
                    StateChanged();
                    break;
                case "LOTR":
                    TotalScore += score;
                    LOTR_Score += score;
                    StateChanged();
                    break;
                default:
                    break;
            }
        }

        public void RemoveFromTotalScore(int score, string category)
        {
            switch (category)
            {
                case "HP":
                    TotalScore -= score;
                    HP_Score -= score;
                    StateChanged();
                    break;
                case "LOTR":
                    TotalScore -= score;
                    LOTR_Score -= score;
                    StateChanged();
                    break;
                default:
                    break;
            }
        }
        //HINT & CLUE METHODS
        public void UpdateHintCounters(string category)
        {
            switch (category)
            {
                case "HP":
                    TotalHintsUsed++;
                    HPHintsUsed++;
                    break;
                case "LOTR":
                    TotalHintsUsed++;
                    LOTRHintsUsed++;
                    break;
                default:
                    break;
            }
        }
        public void UpdateClueCounters(string category)
        {
            switch (category)
            {
                case "HP":
                    TotalCluesUsed++;
                    HPCluesUsed++;
                    break;
                case "LOTR":
                    TotalCluesUsed++;
                    LOTRCluesUsed++;
                    break;
                default:
                    break;
            }
        }
        public void UpdateAnswerCounters(string category, bool correct)
        {
            switch (category)
            {
                case "HP":
                    if (correct)
                    {
                        TotalCorrectAnswers++;
                        CorrectHPAnswers++;
                    }
                    else
                    {
                        TotalWrongAnswers++;
                        WrongHPAnswers++;
                    }

                    break;
                case "LOTR":
                    if (correct)
                    {
                        TotalCorrectAnswers++;
                        CorrectLOTRAnswers++;
                    }
                    else
                    {
                        TotalWrongAnswers++;
                        WrongLOTRAnswers++;
                    }

                    break;
                default:
                    break;
            }
        }

        //GET Methods
        //SCORE
        public int GetTotalScore()
        {
            return TotalScore;
        }
        public int GetTotalHPScore()
        {
            return HP_Score;
        }
        public int GetTotalLOTRScore()
        {
            return LOTR_Score;
        }
        //HINTS-
        public int GetTotalHintsUsed()
        {
            return TotalHintsUsed;
        }
        public int GetHPHintsUsed()
        {
            return HPHintsUsed;
        }
        public int GetLOTRHintsUsed()
        {
            return LOTRHintsUsed;
        }
        //CLUES
        public int GetTotalCluesUsed()
        {
            return TotalCluesUsed;
        }
        public int GetHPCluesUsed()
        {
            return HPCluesUsed;
        }
        public int GetLOTRCluesUsed()
        {
            return LOTRCluesUsed;
        }
        //ANSWERS
        public int GetTotalCorrectAnswers()
        {
            return TotalCorrectAnswers;
        }
        public int GetTotalWrongAnswers()
        {
            return TotalWrongAnswers;
        }
        public int GetHPCorrectAnswers()
        {
            return CorrectHPAnswers;
        }
        public int GetLOTRCorrectAnswers()
        {
            return CorrectLOTRAnswers;
        }
        public int GetHPWrongAnswers()
        {
            return WrongHPAnswers;
        }
        public int GetLOTRWrongAnswers()
        {
            return WrongLOTRAnswers;
        }

        //To be implemented
        private void CheckAndUpdateCurrentLevel(int totalScore)
        {
            
        }
        
        private void StateChanged() => OnChange?.Invoke();
    }
}
