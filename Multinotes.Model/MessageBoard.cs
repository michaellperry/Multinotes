using System;

namespace Multinotes.Model
{
    public partial class MessageBoard
    {
        public void SendMessage(string text)
        {
            Community.Perform(async delegate
            {
                var domain = await Community.AddFactAsync(new Domain());
                await Community.AddFactAsync(new Message(this, domain, text));
            });
        }
    }
}
