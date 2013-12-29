using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Multinotes.Model;

namespace Multinotes.Web.Models
{
    public class MessageBoardViewModel
    {
        public string Topic { get; set; }
        public List<string> Messages { get; set; }
    }
}
