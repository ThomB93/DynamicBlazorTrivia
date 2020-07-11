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
    public partial class GOTCharacters
    {
        private GotCharacter[] _characters;
        private GotCharacter _randomCharacter;
        private GotHouse[] _houses;

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
            _characters = await Http.GetFromJsonAsync<GotCharacter[]>("jsonData/got_characters.json");
            _houses = await Http.GetFromJsonAsync<GotHouse[]>("jsonData/got_houses.json");
            _clues = new List<string>();
            _characterProperties = new Dictionary<string, string>();
            //select a random character as first question
            GetNewQuestion();
        }

        private string GetCharacterNameFromUrl(string url)
        {
            return _characters.FirstOrDefault(c => c.Url == url)?.Name;
        }

        private string GetHouseNameFromUrl(string url)
        {
            string name;
            name = _houses.FirstOrDefault(c => c.Url == url)?.Name;
            if (name != null)
            {
                return name;
            }

            return "";
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
            //_randomCharacter = _characters.FirstOrDefault(c => c.Name == "Bethany Bolton");
            _characterProperties.Clear();
            _currentPointsAwarded = 12;

            if (_randomCharacter.Gender != null)
            {
                _characterProperties.Add("Gender", _randomCharacter.Gender);
            }
            if (_randomCharacter.Culture != null)
            {
                _characterProperties.Add("Culture", _randomCharacter.Culture);
            }
            if (_randomCharacter.Born != null)
            {
                _characterProperties.Add("Born", _randomCharacter.Born);
            }
            if (_randomCharacter.Died != null)
            {
                _characterProperties.Add("Died", _randomCharacter.Died);
            }

            if (_randomCharacter.Titles.Count > 0)
            {
                if (_randomCharacter.Titles[0] != "")
                {
                    for (int i = 0; i < _randomCharacter.Titles.Count; i++)
                    {
                        _characterProperties.Add("Title " + (i+1), _randomCharacter.Titles[i]);
                    }
                }
            }
            if (_randomCharacter.Aliases.Count > 0)
            {
                if (_randomCharacter.Aliases[0] != "")
                {
                    for (int i = 0; i < _randomCharacter.Aliases.Count; i++)
                    {
                        _characterProperties.Add("Alias " + (i+1), _randomCharacter.Aliases[i]);
                    }
                }
            }
            if (_randomCharacter.Allegiances.Count > 0)
            {
                for (int i = 0; i < _randomCharacter.Aliases.Count; i++)
                {
                    _characterProperties.Add("Allegiance " + (i + 1), GetHouseNameFromUrl(_randomCharacter.Allegiances[i]));
                }
            }
            //Console.WriteLine("Adding family relations");
            if (_randomCharacter.Father != null)
            {
                _characterProperties.Add("Father", GetCharacterNameFromUrl(_randomCharacter.Father));
            }
            //Console.WriteLine("Father: " + GetCharacterNameFromUrl(_randomCharacter.Father));
            //Console.WriteLine("Added father relations");
            if (_randomCharacter.Mother != null)
            {
                _characterProperties.Add("Mother", GetCharacterNameFromUrl(_randomCharacter.Mother));
            }
            //Console.WriteLine("Mother: " + GetCharacterNameFromUrl(_randomCharacter.Mother));
            //Console.WriteLine("Adding spouse relations");
            if (_randomCharacter.Spouse != null)
            {
                string spouse = GetCharacterNameFromUrl(_randomCharacter.Spouse);
                _characterProperties.Add("Spouse", spouse);
            }
            //Console.WriteLine("Spouse: " + GetCharacterNameFromUrl(_randomCharacter.Spouse));

            //remove empty _clues
            List<string> cluesToRemove = new List<string>();

            foreach (var entry in _characterProperties)
            {
                if (entry.Value == "" || entry.Value == string.Empty || entry.Value == null)
                {
                    cluesToRemove.Add(entry.Key);
                }
            }

            foreach (var entry in cluesToRemove)
            {
                _characterProperties.Remove(entry);
            }

            //check if there are enough _clues after removing empty _clues
            if (!(_characterProperties.Count >= 4))
            {
                GetNewQuestion();
            }
            else
            {
                //set the character name(correct answer)
                _characterName = _randomCharacter.Name.ToLower();
                //Show starting _clues for question
                GetAnotherClue();
                GetAnotherClue();
            }
        }
        private void GetAnotherClue()
        {
            Console.WriteLine("Get Another Clue");
            if (_characterProperties.Count >= 1)
            {
                var randomEntry = _characterProperties.ElementAt(r.Next(0, _characterProperties.Count - 1));
                String randomKey = randomEntry.Key;
                String randomValue = randomEntry.Value;

                _clues.Add("<b>" + randomKey + "</b>" + ": " + randomValue);
                _characterProperties.Remove(randomKey);
                _currentPointsAwarded--;
                ScoreTrackerService.UpdateClueCounters("GOT");
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
            if (_hintCounter < _randomCharacter.Name.ToString().Length + 1)
            {
                if (_randomCharacter.Name.ToString()[_hintCounter - 1] == ' ')
                {
                    _hintCounter++; //skip spaces when generating hints
                }
                _hintString = _randomCharacter.Name.ToString().Substring(0, _hintCounter);
                _hintCounter++;
                _currentPointsAwarded--;
                ScoreTrackerService.UpdateHintCounters("GOT");
            }
            else
            {
                _noMoreHints = true;
            }
        }
        private void ShowAnswer()
        {
            Toaster.Add("The answer is " + _randomCharacter.Name, MatToastType.Info, "Solution", "", config =>
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
                ScoreTrackerService.AddToTotalScore(_currentPointsAwarded, "GOT");
                ScoreTrackerService.UpdateAnswerCounters("GOT", true);
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
                ScoreTrackerService.RemoveFromTotalScore(2, "GOT");
                ScoreTrackerService.UpdateAnswerCounters("GOT", false);
                StateHasChanged();
            }
        }
        //MatBlazor theme
        MatTheme gotTheme = new MatTheme()
        {
            Primary = ColorUtil.ColorHexString(76, 137, 171),
            Secondary = MatThemeColors.BlueGrey._400.Value
        };
    }
}
