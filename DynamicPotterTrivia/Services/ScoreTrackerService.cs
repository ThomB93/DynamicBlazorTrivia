using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public class ScoreTrackerService : IScoreTrackerService
    {
        //SCORE PROPERTIES
        private int TotalScore { get; set; } 
        private int HPScore { get; set; } 
        private int LOTRScore { get; set; } 
        private int GOTScore { get; set; } 
        //HINT PROPERTIES
        private int TotalHintsUsed { get; set; }
        private int HPHintsUsed { get; set; }
        private int LOTRHintsUsed { get; set; }
        private int GOTHintsUsed { get; set; }
        //CLUES PROPERTIES
        private int TotalCluesUsed { get; set; }
        private int HPCluesUsed { get; set; }
        private int LOTRCluesUsed { get; set; }
        private int GOTCluesUsed { get; set; }
        //ANSWERS PROPERTIES
        private int TotalCorrectAnswers { get; set; } 
        private int TotalWrongAnswers { get; set; } 
        private int CorrectHPAnswers { get; set; }
        private int CorrectLOTRAnswers { get; set; }
        private int CorrectGOTAnswers { get; set; }
        private int WrongHPAnswers { get; set; }
        private int WrongLOTRAnswers { get; set; }
        private int WrongGOTAnswers { get; set; }

        //may be implemented later
        public int Level { get; set; }
        public string CurrentRank { get; set; } = "Hobbit";
        private Dictionary<string, List<int>> rankings;

        public event Action OnChange;

        public ScoreTrackerService()
        {
            rankings = new Dictionary<string, List<int>>()
            {
                {"Hogwarts Student", Enumerable.Range(10, 15).ToList()},
                {"Ringbearer", Enumerable.Range(25, 25).ToList()},
                {"Warden of the North", Enumerable.Range(50, 25).ToList()},
                {"Minister of Magic", Enumerable.Range(75, 25).ToList()},
                {"Steward of Gondor", Enumerable.Range(100, 50).ToList()},
                {"Archmaester", Enumerable.Range(150, 50).ToList()},
                {"Quidditch Captain", Enumerable.Range(200, 100).ToList()},
                {"King of the West", Enumerable.Range(300, 1000).ToList()},
            };
            
        }

        //RANK METHODS
        public void UpdateRankBasedOnCurrentScore()
        {
            if (TotalScore < 10)
            {
                CurrentRank = "Hobbit";
                return;
            }

            foreach (var entry in rankings)
            {
                if (entry.Value.Contains(TotalScore))
                {
                    CurrentRank = entry.Key;
                    Console.WriteLine(entry.Value.Count);
                    Console.WriteLine(TotalScore);
                    Console.WriteLine(entry.Key);
                    return;
                }
            }
        }

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
                case "GOT":
                    TotalScore += score;
                    GOTScore += score;
                    StateChanged();
                    break;
                default:
                    break;
            }
        }

        public void RemoveFromTotalScore(int score, string category)
        {
            if (TotalScore < 1)
            {
                TotalScore = 0;
                return;
            }
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
                case "GOT":
                    TotalScore -= score;
                    GOTScore -= score;
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
                case "GOT":
                    TotalHintsUsed++;
                    GOTHintsUsed++;
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
                case "GOT":
                    TotalCluesUsed++;
                    GOTCluesUsed++;
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
                case "GOT":
                    if (correct)
                    {
                        TotalCorrectAnswers++;
                        CorrectGOTAnswers++;
                    }
                    else
                    {
                        TotalWrongAnswers++;
                        WrongGOTAnswers++;
                    }

                    break;
                default:
                    break;
            }
        }

        //GET Methods
        //RANK
        public string GetCurrentRank()
        {
            return CurrentRank;
        }
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
        public int GetTotalGOTScore()
        {
            return GOTScore;
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
        public int GetGOTHintsUsed()
        {
            return GOTHintsUsed;
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
        public int GetGOTCluesUsed()
        {
            return GOTCluesUsed;
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
        //CORRECT ANSWERS
        public int GetHPCorrectAnswers()
        {
            return CorrectHPAnswers;
        }
        public int GetLOTRCorrectAnswers()
        {
            return CorrectLOTRAnswers;
        }
        public int GetGOTCorrectAnswers()
        {
            return CorrectGOTAnswers;
        }
        //WRONG ANSWERS
        public int GetHPWrongAnswers()
        {
            return WrongHPAnswers;
        }
        public int GetLOTRWrongAnswers()
        {
            return WrongLOTRAnswers;
        }
        public int GetGOTWrongAnswers()
        {
            return WrongGOTAnswers;
        }

        //To be implemented
        private void CheckAndUpdateCurrentLevel(int totalScore)
        {
            
        }

        public string GenerateExportStringFromPropertyValues()
        {
            return
                "TotalScore=" + TotalScore + "\nHPScore=" + HPScore + "\nLOTRScore=" + LOTRScore + "\nGOTScore=" + GOTScore +
                "\nTotalHintsUsed=" + TotalHintsUsed + "\nHPHintsUsed=" + HPHintsUsed + "\nLOTRHintsUsed=" +
                LOTRHintsUsed + "\nGOTHintsUsed=" +
                GOTHintsUsed + "\nTotalCluesUsed=" + TotalCluesUsed + "\nHPCluesUsed=" + HPCluesUsed +
                "\nLOTRCluesUsed=" + LOTRCluesUsed + "\nGOTCluesUsed=" + GOTCluesUsed + "\nTotalCorrectAnswers=" + TotalCorrectAnswers +
                "\nTotalWrongAnswers=" + TotalWrongAnswers + "\nCorrectHPAnswers=" + CorrectHPAnswers +
                "\nCorrectLOTRAnswers=" + CorrectLOTRAnswers + "\nCorrectGOTAnswers=" + CorrectGOTAnswers + "\nWrongHPAnswers=" + WrongHPAnswers +
                "\nWrongLOTRAnswers=" + WrongLOTRAnswers + "\nWrongGOTAnswers=" + WrongGOTAnswers;
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
                    case "GOTScore":
                        GOTScore = Convert.ToInt32(entry.Value);
                        break;
                    case "TotalHintsUsed":
                        TotalHintsUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "LOTRHintsUsed":
                        LOTRHintsUsed = Convert.ToInt32(entry.Value);
                        break;
                    case "GOTHintsUsed":
                        GOTHintsUsed = Convert.ToInt32(entry.Value);
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
                    case "GOTCluesUsed":
                        GOTCluesUsed = Convert.ToInt32(entry.Value);
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
                    case "CorrectGOTAnswers":
                        CorrectGOTAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "WrongHPAnswers":
                        WrongHPAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "WrongLOTRAnswers":
                        WrongLOTRAnswers = Convert.ToInt32(entry.Value);
                        break;
                    case "WrongGOTAnswers":
                        WrongGOTAnswers = Convert.ToInt32(entry.Value);
                        break;
                }
            }
        }
        
        private void StateChanged() => OnChange?.Invoke();
    }
}
