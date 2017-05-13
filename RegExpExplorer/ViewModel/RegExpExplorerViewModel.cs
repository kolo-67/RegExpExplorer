using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegExpExplorer.ViewModel
{
    /// <summary>

    /// </summary>
    //-----------------------------------------------------------------------------------------------------------
    // class RegExpExplorerViewModel
    //-----------------------------------------------------------------------------------------------------------
    public class RegExpExplorerViewModel : ViewModelBase
    {
        private MatchCollection matches;
        private Match selectedMatch;
        private Group selectedGroup;
        private Capture selectedCaptures;
        private string result;
        private string pattern;
        private string replaceText;
        private string textForSearch;
        private string error;
        private bool isWorking;
        
        //-----------------------------------------------------------------------------------------------------------
        public MatchCollection Matches
        {
            get
            {
                return matches;
            }
            set
            {
                matches = value;
                RaisePropertyChanged("Matches");
            }
        }
//-----------------------------------------------------------------------------------------------------------
        public Match SelectedMatch
        {
            get
            {
                return selectedMatch;
            }
            set
            {
                selectedMatch = value;
                RaisePropertyChanged("SelectedMatch");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public Group SelectedGroup
        {
            get
            {
                return selectedGroup;
            }
            set
            {
                selectedGroup = value;
                RaisePropertyChanged("SelectedGroup");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public Capture SelectedCaptures
        {
            get
            {
                return selectedCaptures;
            }
            set
            {
                selectedCaptures = value;
                RaisePropertyChanged("SelectedCaptures");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public string Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
                RaisePropertyChanged("Pattern");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public string ReplaceText
        {
            get
            {
                return replaceText;
            }
            set
            {
                replaceText = value;
                RaisePropertyChanged("ReplaceText");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public string TextForSearch
        {
            get
            {
                return textForSearch;
            }
            set
            {
                textForSearch = value;
                RaisePropertyChanged("TextForSearch");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                RaisePropertyChanged("Error");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public bool IsWorking
        {
            get
            {
                return isWorking;
            }
            set
            {
                isWorking = value;
                RaisePropertyChanged("IsWorking");
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// </summary>
        public RegExpExplorerViewModel()
        {
            Pattern = @"<a[^<]+href=""([^<""]*)""[^<]+>";
        }
        //-------------------------------------------------------------------------------------------------------------------
        private RelayCommand matchCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public RelayCommand MatchCommand
        {
            get
            {
                if (matchCommand == null)
                {
                    matchCommand = new RelayCommand(MatchAction, CanMatchAction);
                }
                return matchCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private async void MatchAction()
        {
            IsWorking = true;
            Task < MatchCollection > task = Task.Run(() =>
                {
                    Regex regex = new Regex(Pattern);
                    return regex.Matches(TextForSearch); ;
                });
            
            try
            {
                MatchCollection lMatches = await task;
                Matches = lMatches;
                Error = "";
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            finally
            {
                IsWorking = false;
            }
           
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanMatchAction()
        {
            return !IsWorking;
        }
        //-----------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------
        private RelayCommand replaceCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public RelayCommand ReplaceCommand
        {
            get
            {
                if (replaceCommand == null)
                {
                    replaceCommand = new RelayCommand(ReplaceAction, CanReplaceAction);
                }
                return replaceCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private async void ReplaceAction()
        {
            IsWorking = true;
            Task<string> task = Task.Run(() =>
            {
                Regex regex = new Regex(Pattern);
                return regex.Replace(TextForSearch, ReplaceText);
            });
            try
            {
                Result = await task;
                Error = "";
            }
            catch ( Exception e)
            {
                Result = e.Message;
                Error = e.Message;
            }
            finally
            {
                IsWorking = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanReplaceAction()
        {
            return !string.IsNullOrWhiteSpace(ReplaceText) && !IsWorking;
        }
        //-----------------------------------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------------------------------------
}