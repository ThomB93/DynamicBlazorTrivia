using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChartJs.Blazor.Util;
using DynamicPotterTrivia.Models;
using MatBlazor;

namespace DynamicPotterTrivia.Pages
{
    public partial class HPCharacters
    {
        private HPCharacter[] characters;
        private HPCharacter randomCharacter;

        private string characterName = "";
        private string currentAnswer = "";

        bool correctAnswer;

        private Dictionary<string, string> characterProperties;
        private List<string> clues = new List<string>();
        private bool noMoreClues;

        private int hintCounter = 1;
        private string hintString = "";
        private bool noMoreHints;

        Random r = new Random();

        int currentPointsAwarded = 0;

        protected override async Task OnInitializedAsync()
        {
            //fetch all characters
            characters = await Http.GetFromJsonAsync<HPCharacter[]>("jsonData/hp_characters.json");

            //select a random character as first question
            GetNewQuestion();
        }

        private void CheckAnswer()
        {
            if (currentAnswer.ToLower() == characterName.ToLower())
            {
                correctAnswer = true;
                //currentScore = currentScore + currentPointsAwarded;
                ScoreTrackerService.AddToTotalScore(currentPointsAwarded, "HP");
                ScoreTrackerService.UpdateAnswerCounters("HP", true);
                StateHasChanged();
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
                //currentScore--;
                ScoreTrackerService.RemoveFromTotalScore(2, "HP");
                ScoreTrackerService.UpdateAnswerCounters("HP", false);
                StateHasChanged();
            }
        }

        private void GetNewQuestion()
        {
            //Remove all data from previous question
            clues.Clear();
            noMoreClues = false;
            correctAnswer = false;
            currentAnswer = string.Empty;

            //reset hints
            hintCounter = 1;
            noMoreHints = false;
            hintString = String.Empty;

            //select a random character
            randomCharacter = characters[r.Next(0, characters.Length - 1)];

            //load the properties of the random character to use as clues
            characterProperties = new Dictionary<string, string>();

            currentPointsAwarded = 12;

            if (randomCharacter.role != null)
            {
                characterProperties.Add("Role", randomCharacter.role);
            }
            if (randomCharacter.house != null)
            {
                characterProperties.Add("House", randomCharacter.house);
            }
            if (randomCharacter.school != null)
            {
                characterProperties.Add("School", randomCharacter.school);
            }
            if (randomCharacter.bloodStatus != null)
            {
                characterProperties.Add("Blood Status", randomCharacter.bloodStatus);
            }
            if (randomCharacter.species != null)
            {
                characterProperties.Add("Species", randomCharacter.species);
            }
            if (randomCharacter.alias != null)
            {
                characterProperties.Add("Alias", randomCharacter.alias);
            }
            if (randomCharacter.animagus != null)
            {
                characterProperties.Add("Animagus", randomCharacter.animagus);
            }
            if (randomCharacter.wand != null)
            {
                characterProperties.Add("Wand", randomCharacter.wand);
            }
            if (randomCharacter.patronus != null)
            {
                characterProperties.Add("Patronus", randomCharacter.patronus);
            }
            //add boolean properties
            characterProperties.Add("Is a Death Eater", randomCharacter.deathEater.ToString());
            characterProperties.Add("Is a member of the Order of the Phoenix", randomCharacter.orderOfThePhoenix.ToString());
            characterProperties.Add("Is a member of Dumbledores Army", randomCharacter.dumbledoresArmy.ToString());
            characterProperties.Add("Works at the Ministry of Magic", randomCharacter.ministryOfMagic.ToString());

            //Show starting clues for question
            GetAnotherClue();
            GetAnotherClue();
            //set the character name(correct answer)
            characterName = randomCharacter.name.ToLower();
        }
        private void GetAnotherClue()
        {
            if (characterProperties.Count > 1)
            {
                var randomEntry = characterProperties.ElementAt(r.Next(0, characterProperties.Count - 1));
                String randomKey = randomEntry.Key;
                String randomValue = randomEntry.Value;
                clues.Add("<b>" + randomKey + "</b>" + ": " + randomValue);
                characterProperties.Remove(randomKey);
                currentPointsAwarded--;
                ScoreTrackerService.UpdateClueCounters("HP");
                StateHasChanged();
            }
            else
            {
                clues.Add("No more clues to give, sorry!");
                StateHasChanged();
                noMoreClues = true;
            }
        }

        public void GetHint()
        {
            if (hintCounter < randomCharacter.name.Length + 1)
            {
                if (randomCharacter.name[hintCounter - 1] == ' ')
                {
                    hintCounter++; //skip spaces when generating hints
                }
                hintString = randomCharacter.name.Substring(0, hintCounter);
                hintCounter++;
                currentPointsAwarded--;
                ScoreTrackerService.UpdateHintCounters("HP");
            }
            else
            {
                noMoreHints = true;
            }
        }

        private void ShowAnswer()
        {
            Toaster.Add("The answer is " + randomCharacter.name, MatToastType.Info, "Solution", "", config =>
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
        //MatBlazor Theme
        MatTheme hpTheme = new MatTheme()
        {
            Primary = ColorUtil.ColorHexString(116, 0, 1),
            Secondary = MatThemeColors.Brown._400.Value
        };
    }
}
