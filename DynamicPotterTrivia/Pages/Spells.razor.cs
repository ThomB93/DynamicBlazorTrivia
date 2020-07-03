using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DynamicPotterTrivia.Models;
using MatBlazor;

namespace DynamicPotterTrivia.Pages
{
    public partial class Spells
    {
        private Spell[] spells;
        private Spell randomSpell;

        private string currentAnswer = "";
        private int currentPointsAwarded = 0;

        private bool correctAnswer = false;

        private string hintString = "";
        private bool noMoreHints = false;
        int hintCounter = 1;

        Random r = new Random();

        protected override async Task OnInitializedAsync()
        {
            //fetch all spells
            spells = await Http.GetFromJsonAsync<Spell[]>("jsonData/spells.json");
            GetNewQuestion();
        }

        private void CheckAnswer()
        {
            if (currentAnswer.ToLower() == randomSpell.spell.ToLower())
            {
                correctAnswer = true;
                //currentScore = currentScore + 10;
                ScoreTrackerService.AddToTotalScore(currentPointsAwarded, "HP");
                ScoreTrackerService.UpdateAnswerCounters("HP", true);
            }
            else
            {
                Toaster.Add("Wrong Answer!", MatToastType.Danger, "Sorry", "", config =>
                {
                    config.ShowCloseButton = true;
                    config.ShowProgressBar = true;
                    config.ShowTransitionDuration = Convert.ToInt32(false);
                    config.VisibleStateDuration = Convert.ToInt32(true);
                    config.HideTransitionDuration = Convert.ToInt32(true);

                    config.RequireInteraction = true;
                });
                ScoreTrackerService.RemoveFromTotalScore(2, "HP");
                ScoreTrackerService.UpdateAnswerCounters("HP", false);
            }
        }

        private void ShowAnswer()
        {
            Toaster.Add("The answer is " + randomSpell.spell, MatToastType.Info, "Solution", "", config =>
            {
                config.ShowCloseButton = true;
                config.ShowProgressBar = true;
                config.ShowTransitionDuration = Convert.ToInt32(false);
                config.VisibleStateDuration = Convert.ToInt32(true);
                config.HideTransitionDuration = Convert.ToInt32(true);

                config.RequireInteraction = true;
            });
            GetNewQuestion();
        }

        private void GetNewQuestion()
        {
            correctAnswer = false;
            noMoreHints = false;
            currentAnswer = string.Empty;
            currentPointsAwarded = 10;

            hintString = string.Empty;
            hintCounter = 1;

            randomSpell = spells[r.Next(0, spells.Length - 1)];
            StateHasChanged();
        }

        private void GetHint()
        {
            if (hintCounter < randomSpell.spell.Length + 1)
            {
                if (randomSpell.spell[hintCounter - 1] == ' ') //skip spaces when generating hints
                {
                    hintCounter++; 
                }
                hintString = randomSpell.spell.Substring(0, hintCounter);
                hintCounter++;
                currentPointsAwarded--;
                ScoreTrackerService.UpdateHintCounters("HP");
            }
            else
            {
                noMoreHints = true;
            }
        }
    }
}
