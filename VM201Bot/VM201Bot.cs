using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Classic.Dialogs;
using Microsoft.Bot.Schema;
using VM201Bot.Dialogs;

namespace VM201
{
    public class VM201Bot : IBot
    {
        public async Task OnTurn(ITurnContext context)
        {
            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(context, () => new RootDialog());
            }
        }
    }    
}
