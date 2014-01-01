using Multinotes.Desktop.Models;
using Multinotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UpdateControls.Correspondence;
using UpdateControls.XAML;

namespace Multinotes.Desktop.ViewModels
{
    public class MainViewModel
    {
        private readonly Community _community;
        private readonly Individual _individual;
        private readonly MessageBoardSelectionModel _selection;
        
        public MainViewModel(Community community, Individual individual, MessageBoardSelectionModel selection)
        {
            _community = community;
            _individual = individual;
            _selection = selection;
        }

        public bool Synchronizing
        {
            get { return _community.Synchronizing; }
        }

        public string LastException
        {
            get
            {
                return _community.LastException == null
                    ? String.Empty
                    : _community.LastException.Message;
            }
        }

        public IEnumerable<MessageBoardViewModel> MessageBoards
        {
            get
            {
                return
                    from share in _individual.Shares
                    where share.MessageBoard != null
                    orderby share.MessageBoard.Topic
                    select new MessageBoardViewModel(share, _selection);
            }
        }

        public MessageBoardViewModel SelectedMessageBoard
        {
            get
            {
                return _selection.SelectedShare == null
                    ? null
                    : new MessageBoardViewModel(_selection.SelectedShare, _selection);
            }
            set
            {
                _selection.SelectedShare = value == null
                    ? null
                    : value.Share;
            }
        }

        public string Topic
        {
            get { return _selection.Topic; }
            set { _selection.Topic = value; }
        }

        public ICommand JoinGroup
        {
            get
            {
                return MakeCommand
                    .Do(async delegate
                    {
                        if (!String.IsNullOrEmpty(_selection.Topic))
                        {
                            Share share = await _individual.JoinMessageBoardAsync(_selection.Topic);
                            _selection.SelectedShare = share;
                            _selection.Topic = null;
                        }
                    });
            }
        }

        public ICommand LeaveBoard
        {
            get
            {
                return MakeCommand
                    .When(() => _selection.SelectedShare != null)
                    .Do(delegate
                    {
                        if (ConfirmLeaveBoard == null || ConfirmLeaveBoard(_selection.SelectedShare.MessageBoard))
                        {
                            _selection.SelectedShare.Leave();
                            _selection.SelectedShare = null;
                        }
                    });
            }
        }

        public ICommand Refresh
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        _community.BeginSending();
                        _community.BeginReceiving();
                    });
            }
        }

        public event Func<MessageBoard, bool> ConfirmLeaveBoard;
    }
}
