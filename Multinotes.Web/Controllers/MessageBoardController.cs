using Multinotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Multinotes.Web.Models;

namespace Multinotes.Web.Controllers
{
    public class MessageBoardController : Controller
    {
        //
        // GET: /MessageBoard/tapestries
        public async Task<ActionResult> Index(string id)
        {
            var messageBoard = await MvcApplication.SynchronizationService.Community.AddFactAsync(
                new MessageBoard(id));
            return View(new MessageBoardViewModel
            {
                Topic = messageBoard.Topic,
                Messages = (await messageBoard.Messages.EnsureAsync())
                    .Select(m => m.Text)
                    .ToList()
            });
        }
	}
}