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
    public partial class GOTHouses
    {
        private GotHouse[] _houses;
        private GotCharacter[] _characters;
        private GotHouse _randomHouse;

        private string _houseName = "";
        private string _currentAnswer = "";

        private bool _correctAnswer;

        private Dictionary<string, string> _houseProperties;
        private List<string> _clues;
        private bool _noMoreClues = false;

        private int _hintCounter = 6;
        private string _hintString = "House";
        private bool _noMoreHints = false;

        Random r = new Random();

        private int _currentPointsAwarded = 0;

        protected override async Task OnInitializedAsync()
        {
            //fetch all _characters and houses
            _characters = await Http.GetFromJsonAsync<GotCharacter[]>("jsonData/got_houses.json");
            _houses = await Http.GetFromJsonAsync<GotHouse[]>("jsonData/got_houses.json");
            _clues = new List<string>();
            _houseProperties = new Dictionary<string, string>();
            //select a random house as first question
            GetNewQuestion();
        }
        private string GetCharacterNameFromUrl(string url)
        {
            return _characters.FirstOrDefault(c => c.Url == url)?.Name;
        }
        private void GetNewQuestion()
        {
            //Remove all data from previous question
            _clues.Clear();
            _noMoreClues = false;
            _correctAnswer = false;
            _currentAnswer = string.Empty;

            //reset hints
            _hintCounter = 6; //do not include "house" as part of hint
            _noMoreHints = false;
            _hintString = "House";

            //select a random house
            _randomHouse = _houses[r.Next(0, _houses.Length - 1)];
            _houseProperties.Clear();
            _currentPointsAwarded = 12;

            if (_randomHouse.Region != null)
            {
                _houseProperties.Add("Region", _randomHouse.Region);
            }
            if (_randomHouse.CoatOfArms != null)
            {
                _houseProperties.Add("Coat of Arms", _randomHouse.CoatOfArms);
            }
            if (_randomHouse.Words != null)
            {
                _houseProperties.Add("Words", _randomHouse.Words);
            }
            if (_randomHouse.Titles.Count > 0)
            {
                if (_randomHouse.Titles[0] != "")
                {
                    for (int i = 0; i < _randomHouse.Titles.Count; i++)
                    {
                        _houseProperties.Add("Title " + (i + 1), _randomHouse.Titles[i]);
                    }
                }
            }
            if (_randomHouse.Seats.Count > 0)
            {
                if (_randomHouse.Seats[0] != "")
                {
                    for (int i = 0; i < _randomHouse.Seats.Count; i++)
                    {
                        _houseProperties.Add("Seat " + (i + 1), _randomHouse.Seats[i]);
                    }
                }
            }
            if (_randomHouse.SwornMembers.Count > 0)
            {
                if (_randomHouse.SwornMembers[0] != "")
                {
                    for (int i = 0; i < _randomHouse.SwornMembers.Count; i++)
                    {
                        _houseProperties.Add("Sworn Member " + (i + 1), GetCharacterNameFromUrl(_randomHouse.SwornMembers[i]));
                    }
                }
            }
            if (_randomHouse.CurrentLord != null)
            {
                _houseProperties.Add("Current Lord", GetCharacterNameFromUrl(_randomHouse.CurrentLord));
            }
            if (_randomHouse.Heir != null)
            {
                _houseProperties.Add("Heir", GetCharacterNameFromUrl(_randomHouse.Heir));
            }
            if (_randomHouse.Founded != null)
            {
                _houseProperties.Add("Founded", _randomHouse.Founded);
            }
            if (_randomHouse.DiedOut != null)
            {
                _houseProperties.Add("Died Out", _randomHouse.DiedOut);
            }
            if (_randomHouse.Founder != null)
            {
                _houseProperties.Add("Founder", GetCharacterNameFromUrl(_randomHouse.Founder));
            }

            //remove empty _clues
            List<string> cluesToRemove = new List<string>();

            foreach (var entry in _houseProperties)
            {
                if (entry.Value == "" || entry.Value == string.Empty || entry.Value == null)
                {
                    cluesToRemove.Add(entry.Key);
                }
            }

            foreach (var entry in cluesToRemove)
            {
                _houseProperties.Remove(entry);
            }

            //check if there are enough _clues after removing empty _clues
            if (!(_houseProperties.Count >= 4))
            {
                GetNewQuestion();
            }
            else
            {
                //set the character name(correct answer)
                _houseName = _randomHouse.Name.ToLower();
                //Show starting _clues for question
                GetAnotherClue();
                GetAnotherClue();
            }
        }
        private void GetAnotherClue()
        {
            Console.WriteLine("Get Another Clue");
            if (_houseProperties.Count >= 1)
            {
                var randomEntry = _houseProperties.ElementAt(r.Next(0, _houseProperties.Count - 1));
                String randomKey = randomEntry.Key;
                String randomValue = randomEntry.Value;

                _clues.Add("<b>" + randomKey + "</b>" + ": " + randomValue);
                _houseProperties.Remove(randomKey);
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
            if (_hintCounter < _randomHouse.Name.Length + 1)
            {
                if (_randomHouse.Name[_hintCounter - 1] == ' ')
                {
                    _hintCounter++; //skip spaces when generating hints
                }
                _hintString = _randomHouse.Name.Substring(0, _hintCounter);
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
            Toaster.Add("The answer is " + _randomHouse.Name, MatToastType.Info, "Solution", "", config =>
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
            if (_currentAnswer.ToLower() == _houseName.ToLower())
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
