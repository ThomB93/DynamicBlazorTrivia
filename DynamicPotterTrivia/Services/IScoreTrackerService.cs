﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPotterTrivia.Services
{
    public interface IScoreTrackerService
    {
        //RANK
        public string GetCurrentRank();

        public void UpdateRankBasedOnCurrentScore();
        //SCORE
        public void AddToTotalScore(int score, string category);
        public void RemoveFromTotalScore(int score, string category);
        public int GetTotalScore();
        public int GetTotalHPScore();
        public int GetTotalLOTRScore();
        public int GetTotalGOTScore();
        //HINTS-
        public int GetTotalHintsUsed();
        public int GetHPHintsUsed();
        public int GetLOTRHintsUsed();
        public int GetGOTHintsUsed();
        public void UpdateHintCounters(string category);

        //CLUES
        public int GetTotalCluesUsed();

        public int GetHPCluesUsed();

        public int GetLOTRCluesUsed();
        public int GetGOTCluesUsed();
        public void UpdateClueCounters(string category);

        //ANSWERS
        public int GetTotalCorrectAnswers();

        public int GetTotalWrongAnswers();

        public int GetHPCorrectAnswers();

        public int GetLOTRCorrectAnswers();
        public int GetGOTCorrectAnswers();

        public int GetHPWrongAnswers();

        public int GetLOTRWrongAnswers();
        public int GetGOTWrongAnswers();
        public void UpdateAnswerCounters(string category, bool correct);
        //IMPORT EXPORT
        public string GenerateExportStringFromPropertyValues();
        public void UpdateScoresFromImport(Dictionary<string, string> importedScores);
        //EVENTS
        event Action OnChange;
    }
}
