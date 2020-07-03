using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicPotterTrivia.Models;
using MatBlazor;

namespace DynamicPotterTrivia.Pages
{
    public partial class LOTRCharacters
    {
        private LotrCharacter[] characters;
        private LotrCharacter randomCharacter;

        private string characterName = "";
        private string currentAnswer = "";

        bool correctAnswer = false;

        private Dictionary<string, string> characterProperties;
        private List<string> clues = new List<string>();
        private bool noMoreClues = false;

        private int hintCounter = 1;
        private string hintString = "";
        private bool noMoreHints = false;

        Random r = new Random();

        private int currentPointsAwarded = 0;
        protected override async Task OnInitializedAsync()
        {
            //fetch all characters
            characters = await Http.GetFromJsonAsync<LotrCharacter[]>("jsonData/lotr_characters.json");

            //select a random character as first question
            GetNewQuestion();
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

            //Check if random character contains enough properties to for clues
            //propertyCounter = NonNullPropertiesCount(randomCharacter);
            //if (!(propertyCounter >= 4))
            //{
            //    propertyCounter = 0;
            //    GetNewQuestion();
            //}

            //load the properties of the random character to use as clues
            characterProperties = new Dictionary<string, string>();

            currentPointsAwarded = 12;

            if (randomCharacter.birth != null)
            {
                characterProperties.Add("Birth", randomCharacter.birth);
            }
            if (randomCharacter.death != null)
            {
                characterProperties.Add("Death", randomCharacter.death);
            }
            if (randomCharacter.gender != null)
            {
                characterProperties.Add("Gender", randomCharacter.gender);
            }
            if (randomCharacter.hair != null)
            {
                characterProperties.Add("Hair", randomCharacter.hair);
            }
            if (randomCharacter.height != null)
            {
                characterProperties.Add("Height", randomCharacter.height);
            }
            if (randomCharacter.race != null)
            {
                characterProperties.Add("Race", randomCharacter.race.ToString());
            }
            if (randomCharacter.realm != null)
            {
                characterProperties.Add("Realm", randomCharacter.realm.ToString());
            }
            if (randomCharacter.spouse != null)
            {
                characterProperties.Add("Spouse", randomCharacter.spouse.ToString());
            }

            //remove empty clues
            List<string> cluesToRemove = new List<string>();

            foreach (var entry in characterProperties)
            {
                if (entry.Value == "")
                {
                    cluesToRemove.Add(entry.Key);
                }
            }

            foreach (var entry in cluesToRemove)
            {
                characterProperties.Remove(entry);
            }

            //check if there are enough clues after removing empty clues
            if (!(characterProperties.Count >= 4))
            {
                GetNewQuestion();
            }

            //Show starting clues for question
            GetAnotherClue();
            GetAnotherClue();
            //set the character name(correct answer)
            characterName = randomCharacter.name.ToString().ToLower();
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
                ScoreTrackerService.UpdateClueCounters("LOTR");
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
            if (hintCounter < randomCharacter.name.ToString().Length + 1)
            {
                if (randomCharacter.name.ToString()[hintCounter - 1] == ' ')
                {
                    hintCounter++; //skip spaces when generating hints
                }
                hintString = randomCharacter.name.ToString().Substring(0, hintCounter);
                hintCounter++;
                currentPointsAwarded--;
                ScoreTrackerService.UpdateHintCounters("LOTR");
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
        private void CheckAnswer()
        {
            if (currentAnswer.ToLower() == characterName.ToLower())
            {
                correctAnswer = true;
                //currentScore = currentScore + currentPointsAwarded;
                ScoreTrackerService.AddToTotalScore(currentPointsAwarded, "LOTR");
                ScoreTrackerService.UpdateAnswerCounters("LOTR", true);
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
                ScoreTrackerService.RemoveFromTotalScore(2, "LOTR");
                ScoreTrackerService.UpdateAnswerCounters("LOTR", false);
                StateHasChanged();
            }
        }

        //HELPER METHOD
        public int NonNullPropertiesCount(object entity)
        {
            return entity.GetType()
                         .GetProperties()
                         .Select(x => x.GetValue(entity, null))
                         .Count(v => v != null);
        }

    }
}
