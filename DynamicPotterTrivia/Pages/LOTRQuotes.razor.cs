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
    public partial class LOTRQuotes
    {
        private LotrQuote[] _quotes;
        private LotrQuote _randomQuote;

        private LotrCharacter[] _characters;

        private string _characterName = "";
        private string _currentAnswer = "";

        private bool _correctAnswer;

        private int _hintCounter = 1;
        private string _hintString = "";
        private bool _noMoreHints;

        Random r = new Random();

        private int _currentPointsAwarded;

        protected override async Task OnInitializedAsync()
        {
            //fetch all _characters
            _quotes = await Http.GetFromJsonAsync<LotrQuote[]>("jsonData/lotr_quotes.json");
            _characters = await Http.GetFromJsonAsync<LotrCharacter[]>("jsonData/lotr_characters.json");
            GetNewQuestion();
        }

        private string GetCharacterNameFromId(string id)
        {
            return _characters.FirstOrDefault(c => c._id == id).name.ToString();
        }
        private void GetNewQuestion()
        {
            _correctAnswer = false;
            _currentAnswer = string.Empty;

            //reset hints
            _hintCounter = 1;
            _noMoreHints = false;
            _hintString = string.Empty;

            _currentPointsAwarded = 10;

            _randomQuote = _quotes[r.Next(0, _quotes.Length - 1)];
            _characterName = GetCharacterNameFromId(_randomQuote.Character);
            if(string.IsNullOrEmpty(_characterName) || _characterName  == "MINOR_CHARACTER")
            {
                GetNewQuestion();
            }
            StateHasChanged();
        }
        private void GetHint()
        {
            if (_hintCounter < _characterName.Length + 1)
            {
                if (_characterName[_hintCounter - 1] == ' ')
                {
                    _hintCounter++; //skip spaces when generating hints
                }
                _hintString = _characterName.Substring(0, _hintCounter);
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
            Toaster.Add("The answer is " + _characterName, MatToastType.Info, "Solution", "", config =>
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
            //Logic for allowing shortened names as answers
            switch (_currentAnswer.ToLower())
            {
                case "aragorn":
                    if (_characterName.ToLower() == "aragorn ii elessar")
                    {
                        _characterName = "aragorn";
                    }

                    break;
                case "sam":
                    if (_characterName.ToLower() == "samwise gamgee")
                    {
                        _characterName = "sam";
                    }

                    break;
                case "frodo":
                    if (_characterName.ToLower() == "frodo baggins")
                    {
                        _characterName = "frodo";
                    }

                    break;
                case "merry":
                    if (_characterName.ToLower() == "meriadoc brandybuck")
                    {
                        _characterName = "merry";
                    }

                    break;
                case "pippin":
                    if (_characterName.ToLower() == "peregrin took")
                    {
                        _characterName = "pippin";
                    }

                    break;
                case "bilbo":
                    if (_characterName.ToLower() == "bilbo baggins")
                    {
                        _characterName = "bilbo";
                    }

                    break;
            }

            if (_currentAnswer.ToLower() == _characterName.ToLower())
            {
                _correctAnswer = true;
                //currentScore = currentScore + _currentPointsAwarded;
                ScoreTrackerService.AddToTotalScore(_currentPointsAwarded, "LOTR");
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
        MatTheme lotrTheme = new MatTheme()
        {
            Primary = ColorUtil.ColorHexString(203, 138, 49),
            Secondary = MatThemeColors.Brown._400.Value
        };
    }
}
