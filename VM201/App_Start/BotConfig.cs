// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.Integration.AspNet.WebApi;
using System.Configuration;
using System.Web.Http;
using Microsoft.Bot.Builder.Alexa.Integration.AspNet.WebApi;
using Microsoft.Bot.Builder.Alexa.Middleware;

namespace VM201
{
    public class BotConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapBotFramework(botConfig =>
                {
                    botConfig
                        .UseMicrosoftApplicationIdentity(
                            ConfigurationManager.AppSettings["BotFramework.MicrosoftApplicationId"],
                            ConfigurationManager.AppSettings["BotFramework.MicrosoftApplicationPassword"]);
                })
                .MapAlexaBotFramework(botConfig =>
                    {
                        botConfig.UseMiddleware(new AlexaIntentRequestToMessageActivityMiddleware());
                        botConfig.AlexaBotOptions.ValidateIncomingAlexaRequests = false;
                    });
        }
    }
}