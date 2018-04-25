using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Classic.Dialogs;
using Microsoft.Bot.Schema;
using VM201.Dialogs;

namespace VM201
{
    public class VM201Bot : IBot
    {
        public VM201Bot() { }

        public async Task OnTurn(ITurnContext context)
        {
            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:
                    await Conversation.SendAsync(context, () => new RootDialog());
                    break;
            }
        }
    }
}