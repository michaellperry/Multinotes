using Multinotes.Model;
using Multinotes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Multinotes.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var domain = MvcApplication.SynchronizationService.Domain;
            var messageBoardTasks = new string[] { "Azure", "Windows Store", "Windows Phone" }
                .Select(topic => MvcApplication.SynchronizationService.Community.AddFactAsync(
                    new MessageBoard(topic)));
            var messageBoards = await Task.WhenAll(messageBoardTasks);
            var messageBoardViewModelTasks = messageBoards
                .Select(b => LoadMessageBoardAsync(b));
            var messageBoardViewModels = await Task.WhenAll(messageBoardViewModelTasks);
            var viewModel = new HomeViewModel
            {
                RecentMessageBoards = messageBoardViewModels.ToList()
            };
            return View(viewModel);
        }

        private async Task<MessageBoardViewModel> LoadMessageBoardAsync(MessageBoard messageBoard)
        {
            var messages = await messageBoard.Messages.EnsureAsync();
            return new MessageBoardViewModel
            {
                Topic = messageBoard.Topic,
                Messages = messages
                    .Take(3)
                    .Select(m => m.Text)
                    .ToList()
            };
        }

        public ActionResult About()
        {
            ViewBag.Message = "Multinotes example application";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me for more information";

            return View();
        }
    }
}