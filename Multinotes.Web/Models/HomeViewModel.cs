using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multinotes.Web.Models
{
    public class HomeViewModel
    {
        public List<MessageBoardViewModel> RecentMessageBoards { get; set; }
    }
}