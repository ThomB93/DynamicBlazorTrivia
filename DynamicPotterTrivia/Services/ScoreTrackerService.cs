﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public class ScoreTrackerService : IScoreTrackerService
    {
        //SCORE PROPERTIES
        private int TotalScore { get; set; } = 30;
        private int HPScore { get; set; } = 10;
        private int LOTRScore { get; set; } = 20;
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
                    HPScore += score;
                    StateChanged();
                    break;
                case "LOTR":
                    TotalScore += score;
                    LOTRScore += score;
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
                    HPScore -= score;
                    StateChanged();
                    break;
                case "LOTR":
                    TotalScore -= score;
                    LOTRScore -= score;
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
            return HPScore;
        }
        public int GetTotalLOTRScore()
        {
            return LOTRScore;
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

        public string GenerateExportStringFromPropertyValues()
        {
            return
                "TotalScore=" + TotalScore + "\nHPScore=" + HPScore + "\nLOTRScore=" + LOTRScore +
                "\nTotalHintsUsed=" + TotalHintsUsed + "\nHPHintsUsed=" + HPHintsUsed + "\nLOTRHintsUsed=" +
                LOTRHintsUsed + "\nTotalCluesUsed=" + TotalCluesUsed + "\nHPCluesUsed=" + HPCluesUsed +
                "\nLOTRCluesUsed=" + LOTRCluesUsed + "\nTotalCorrectAnswers=" + TotalCorrectAnswers +
                "\nTotalWrongAnswers=" + TotalWrongAnswers + "\nCorrectHPAnswers=" + CorrectHPAnswers +
                "\nCorrectLOTRAnswers=" + CorrectLOTRAnswers + "\nWrongHPAnswers=" + WrongHPAnswers +
                "\nWrongLOTRAnswers=" + WrongLOTRAnswers;
        }

        public void UpdateScoresFromImport(Dictionary<string, string> importedScores)
        {
            //update properties based on key value pairs
            foreach (var entry in importedScores)
            {
                switch (entry.Key)
                {
                    case "TotalScore":
                        TotalScore = Convert.ToInt32(entry.Value);
                        break;
                    case "HPScore":
                        HPScore = Convert.ToInt32(entry.Value);
                        break;
                    case "LOTRScore":
                        LOTRScore = Convert.ToInt32(entry.Value);
                        break;
                    case "TotalHintsUsed":
                        TotalHintsUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "LOTRHintsUsed":
                        LOTRHintsUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "TotalCluesUsed":
                        TotalCluesUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "HPCluesUsed":
                        HPCluesUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "LOTRCluesUsed":
                        LOTRCluesUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "TotalCorrectAnswers":
                        TotalCorrectAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "TotalWrongAnswers":
                        TotalWrongAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "CorrectHPAnswers":
                        CorrectHPAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "CorrectLOTRAnswers":
                        CorrectLOTRAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "WrongHPAnswers":
                        WrongHPAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "WrongLOTRAnswers":
                        WrongLOTRAnswers = Convert.ToInt32(entry.Value);
                        break;
                }
            }
        }
        
        private void StateChanged() => OnChange?.Invoke();
    }
}
