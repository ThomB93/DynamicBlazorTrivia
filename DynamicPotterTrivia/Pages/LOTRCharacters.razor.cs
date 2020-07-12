using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ChartJs.Blazor.Util;
using DynamicPotterTrivia.Models;
using MatBlazor;

namespace DynamicPotterTrivia.Pages
{
    public partial class LOTRCharacters
    {
        private LotrCharacter[] _characters;
        private LotrCharacter _randomCharacter;

        private string _characterName = "";
        private string _currentAnswer = "";

        private bool _correctAnswer;

        private Dictionary<string, string> _characterProperties;
        private List<string> _clues;
        private bool _noMoreClues;

        private int _hintCounter = 1;
        private string _hintString = "";
        private bool _noMoreHints;

        Random r = new Random();

        private int _currentPointsAwarded = 0;
        protected override async Task OnInitializedAsync()
        {
            //fetch all _characters
            _characters = await Http.GetFromJsonAsync<LotrCharacter[]>("jsonData/lotr_characters.json");
            _clues = new List<string>();
            _characterProperties = new Dictionary<string, string>();
            //select a random character as first question
            GetNewQuestion();
        }

        private void GetNewQuestion()
        {
            Console.WriteLine("GetNewQuestion");
            //Remove all data from previous question
            _clues.Clear();
            _noMoreClues = false;
            _correctAnswer = false;
            _currentAnswer = string.Empty;

            //reset hints
            _hintCounter = 1;
            _noMoreHints = false;
            _hintString = string.Empty;

            //select a random character
            _randomCharacter = _characters[r.Next(0, _characters.Length - 1)];
            //Check if random character contains enough properties to for _clues
            //propertyCounter = NonNullPropertiesCount(_randomCharacter);
            //if (!(propertyCounter >= 4))
            //{
            //    propertyCounter = 0;
            //    GetNewQuestion();
            //}

            //load the properties of the random character to use as _clues
            _characterProperties.Clear();
            _currentPointsAwarded = 12;

            if (_randomCharacter.birth != null)
            {
                _characterProperties.Add("Birth", _randomCharacter.birth.ToString());
            }
            if (_randomCharacter.death != null)
            {
                _characterProperties.Add("Death", _randomCharacter.death.ToString());
            }
            if (_randomCharacter.gender != null)
            {
                _characterProperties.Add("Gender", _randomCharacter.gender.ToString());
            }
            if (_randomCharacter.hair != null)
            {
                _characterProperties.Add("Hair", _randomCharacter.hair.ToString());
            }
            if (_randomCharacter.height != null)
            {
                _characterProperties.Add("Height", _randomCharacter.height.ToString());
            }
            if (_randomCharacter.race != null)
            {
                _characterProperties.Add("Race", _randomCharacter.race.ToString());
            }
            if (_randomCharacter.realm != null)
            {
                _characterProperties.Add("Realm", _randomCharacter.realm.ToString());
            }
            if (_randomCharacter.spouse != null)
            {
                _characterProperties.Add("Spouse", _randomCharacter.spouse.ToString());
            }

            //remove empty _clues
            List<string> cluesToRemove = new List<string>();

            foreach (var entry in _characterProperties)
            {
                if (entry.Value == "")
                {
                    cluesToRemove.Add(entry.Key);
                }
            }

            foreach (var entry in cluesToRemove)
            {
                _characterProperties.Remove(entry);
            }

            //check if there are enough _clues after removing empty _clues
            if (!(_characterProperties.Count >= 3))
            {
                GetNewQuestion();
            }
            else
            {
                //Show starting _clues for question
                GetAnotherClue();
                GetAnotherClue();
                //set the character name(correct answer)
                _characterName = _randomCharacter.name.ToString().ToLower();
            }
        }
        private void GetAnotherClue()
        {
            if (_currentPointsAwarded < 1)
            {
                _noMoreClues = true;
                Toaster.Add("No more clues allowed!", MatToastType.Danger, "Sorry", "", config =>
                {
                    config.ShowCloseButton = true;
                    config.ShowProgressBar = true;
                    config.ShowTransitionDuration = Convert.ToInt32(false);
                    config.VisibleStateDuration = Convert.ToInt32(true);
                    config.HideTransitionDuration = Convert.ToInt32(true);

                    config.RequireInteraction = true;
                });
                return;
            }
            if (_characterProperties.Count > 1)
            {
                var randomEntry = _characterProperties.ElementAt(r.Next(0, _characterProperties.Count - 1));
                String randomKey = randomEntry.Key;
                String randomValue = randomEntry.Value;
                
                _clues.Add("<b>" + randomKey + "</b>" + ": " + randomValue);
                _characterProperties.Remove(randomKey);
                _currentPointsAwarded--;
                ScoreTrackerService.UpdateClueCounters("LOTR");
                StateHasChanged();
            }
            else
            {
                _clues.Add("No more clues to give, sorry!");
                StateHasChanged();
                _noMoreClues = true;
            }
        }
        private void GetHint()
        {
            //do not allow more hints of current points will go into negative
            if (_currentPointsAwarded < 1)
            {
                _noMoreHints = true;
                Toaster.Add("No more hints allowed!", MatToastType.Danger, "Sorry", "", config =>
                {
                    config.ShowCloseButton = true;
                    config.ShowProgressBar = true;
                    config.ShowTransitionDuration = Convert.ToInt32(false);
                    config.VisibleStateDuration = Convert.ToInt32(true);
                    config.HideTransitionDuration = Convert.ToInt32(true);

                    config.RequireInteraction = true;
                });
                return;
            }

            if (_hintCounter < _randomCharacter.name.ToString().Length + 1)
            {
                if (_randomCharacter.name.ToString()[_hintCounter - 1] == ' ')
                {
                    _hintCounter++; //skip spaces when generating hints
                }
                _hintString = _randomCharacter.name.ToString().Substring(0, _hintCounter);
                _hintCounter++;
                _currentPointsAwarded--;
                ScoreTrackerService.UpdateHintCounters("LOTR");
            }
            else
            {
                _noMoreHints = true;
            }
        }
        private void ShowAnswer()
        {
            Toaster.Add("The answer is " + _randomCharacter.name, MatToastType.Info, "Solution", "", config =>
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
            if (_currentAnswer.ToLower() == _characterName.ToLower())
            {
                _correctAnswer = true;
                //currentScore = currentScore + _currentPointsAwarded;
                ScoreTrackerService.AddToTotalScore(_currentPointsAwarded, "LOTR");
                ScoreTrackerService.UpdateAnswerCounters("LOTR", true);
                ScoreTrackerService.UpdateRankBasedOnCurrentScore();
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
                ScoreTrackerService.UpdateRankBasedOnCurrentScore();
                StateHasChanged();
            }
        }

        //HELPER METHOD
        private int NonNullPropertiesCount(object entity)
        {
            return entity.GetType()
                         .GetProperties()
                         .Select(x => x.GetValue(entity, null))
                         .Count(v => v != null);
        }

        //MatBlazor Theme
        MatTheme lotrTheme = new MatTheme()
        {
            Primary = ColorUtil.ColorHexString(203, 138, 49),
            Secondary = MatThemeColors.Brown._400.Value
        };
    }
}
