using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Bot.Builder.Classic.Dialogs;
using Microsoft.Bot.Builder.Classic.Luis;
using Microsoft.Bot.Builder.Classic.Luis.Models;
using Microsoft.Bot.Schema;
using VM201Bot.Model;

namespace VM201Bot.Dialogs
{
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        public RootDialog() : base(new LuisService(new LuisModelAttribute(
            "3d6d5be6-2b2f-4374-848d-ed6d7b8e5e90",
            "bfa14f500a6c4f1b95b8e25a8d6dd95a")))
        {
        }

        [LuisIntent("TurnHeatingZoneOff")]
        public async Task TurnHeatingOff(IDialogContext context, LuisResult result)
        {
            var currentStatus = await GetHeatingStatus();
            var zone = result.Entities.FirstOrDefault(e => e.Type == "Zone");
            var zoneNumber = GetZoneNumberFromZoneEntity(zone);

            if (zoneNumber != -1)
            {
                var zoneStatus = currentStatus.Zones.First(z => z.ZoneNumber == zoneNumber);

                if (!zoneStatus.IsOn)
                {
                    var message = $"The heating is not on ";
                    message += (zoneStatus.Name.ToLower() == "downstairs") ? $"{zoneStatus.Name}" : $"in the {zoneStatus.Name}.";
                    await context.PostAsync(message);
                }
                else
                {
                    if (currentStatus.Zones.Count(z => z.IsOn) > 1 && zoneNumber == 7)
                    {
                        var message = $"Sorry, I can't turn off the loft whilst other rooms are on as well.";
                        await context.PostAsync(message);
                    }
                    else
                    {
                        ToggleZone(zoneNumber);
                        currentStatus = await GetHeatingStatus();

                        if (currentStatus.Zones.Count(z => z.IsOn) == 1 &&
                            currentStatus.Zones.First(z => z.IsOn).ZoneNumber == 7)
                        {
                            // turn off the heating in the loft - TODO check if we should do this
                            ToggleZone(7);
                        }

                        if (!currentStatus.Zones.First(z => z.ZoneNumber == zoneNumber).IsOn)
                        {
                            var message = $"Ok, I have turned off the heating ";
                            message += (zoneStatus.Name.ToLower() == "downstairs")
                                ? $"{zoneStatus.Name}"
                                : $"in the {zoneStatus.Name}. ";
                            await context.PostAsync(message);
                        }
                        else
                        {
                            var message = "Sorry, I couldn't turn off the heating ";
                            message += (zoneStatus.Name.ToLower() == "downstairs")
                                ? $"{zoneStatus.Name}"
                                : $"in the {zoneStatus.Name}.";
                            await context.PostAsync(message);
                        }
                    }
                }
            }
            else
            {
                var zonesOn = currentStatus.Zones.Where(z => z.IsOn).ToList();
                foreach (var zoneToSwitchOff in zonesOn)
                {
                    ToggleZone(zoneToSwitchOff.ZoneNumber);
                }

                currentStatus = await GetHeatingStatus();

                if (!currentStatus.Zones.Any(z => z.IsOn))
                {
                    var message = $"No problem, I have turned the heating off everywhere.";
                    await context.PostAsync(message);
                    context.Wait(MessageReceived);
                }
                else
                {
                    var message = $"Sorry, There was a problem turning off the heating!";
                    await context.PostAsync(message);
                    context.Wait(MessageReceived);
                }
            }

            //if (context.Activity.ChannelData != null)
            //{
            //    var channelData = context.Activity.ChannelData.ToString();
            //    var alexaData = JsonConvert.DeserializeObject<AlexaChannelData>(channelData);
            //    messageToSpeak += $" . User Id: {alexaData?.user_userId}, Access Token: {alexaData?.user_accessToken}";
            //}
        }

        [LuisIntent("TurnHeatingZoneOn")]
        public async Task TurnHeatingOn(IDialogContext context, LuisResult result)
        {
            var currentStatus = await GetHeatingStatus();
            var zone = result.Entities.FirstOrDefault(e => e.Type == "Zone");
            var zoneNumber = GetZoneNumberFromZoneEntity(zone);

            if (zoneNumber != -1)
            {
                var zoneStatus = currentStatus.Zones.First(z => z.ZoneNumber == zoneNumber);

                if (zoneStatus.IsOn)
                {
                    var message = $"The heating is already on ";
                    message += (zoneStatus.Name.ToLower() == "downstairs") ? $"{zoneStatus.Name}" : $"in the {zoneStatus.Name}.";
                    await context.PostAsync(message);
                }
                else
                {
                    ToggleZone(zoneNumber);

                    if (!currentStatus.Zones.First(z => z.ZoneNumber == 7).IsOn)
                    {
                        ToggleZone(7);
                    }

                    currentStatus = await GetHeatingStatus();

                    if (currentStatus.Zones.First(z => z.ZoneNumber == zoneNumber).IsOn)
                    {
                        var message = $"Ok, I have turned on the heating ";
                        message += (zoneStatus.Name.ToLower() == "downstairs")
                            ? $"{zoneStatus.Name}"
                            : $"in the {zoneStatus.Name}.";
                        await context.PostAsync(message);
                    }
                    else
                    {
                        var message = "Sorry, I couldn't turn the heating on ";
                        message += (zoneStatus.Name.ToLower() == "downstairs")
                            ? $"{zoneStatus.Name}"
                            : $"in the {zoneStatus.Name}.";
                        await context.PostAsync(message);
                    }
                }
            }
            else
            {
                var message = $"Sorry, I am not sure where you wanted to turn the heating on.";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }

            //var zone = result.Entities.FirstOrDefault(e => e.Type == "Zone");
            //var messageToSpeak = @"<emphasis level=""reduced"">No problem,</emphasis> I have turned <emphasis level=""strong"">on</emphasis> the heating ";
            //var message = "You turned on the heating ";
            //message += (zone != null) ? $"in the {zone.Entity}" : "everywhere";
            //messageToSpeak += (zone != null) ? $"in the {zone.Entity}" : "everywhere";

            //if (context.Activity.ChannelData != null)
            //{
            //    var channelData = context.Activity.ChannelData.ToString();
            //    var alexaData = JsonConvert.DeserializeObject<AlexaChannelData>(channelData);
            //    messageToSpeak += $" . User Id: {alexaData?.user_userId}, Access Token: {alexaData?.user_accessToken}";
            //}
        }

        [LuisIntent("IsHeatingOn")]
        public async Task IsHeatingOn(IDialogContext context, LuisResult result)
        {
            var currentStatus = await GetHeatingStatus();
            var zone = result.Entities.FirstOrDefault(e => e.Type == "Zone");
            var zoneNumber = GetZoneNumberFromZoneEntity(zone);

            if (zoneNumber != -1)
            {
                var zoneStatus = currentStatus.Zones.First(z => z.ZoneNumber == zoneNumber);

                if (zoneStatus.IsOn)
                {
                    var message = $"Yes, the heating is on ";
                    message += (zoneStatus.Name.ToLower() == "downstairs") ? $"{zoneStatus.Name}" : $"in the {zoneStatus.Name}.";
                    await context.PostAsync(message);
                }
                else
                {
                    var message = $"No, the heating is off ";
                    message += (zoneStatus.Name.ToLower() == "downstairs") ? $"{zoneStatus.Name}" : $"in the {zoneStatus.Name}.";
                    await context.PostAsync(message);
                }
            }
            else
            {
                var zones = string.Empty;
                var zonesCurrentlyOn = currentStatus.Zones.Where(z => z.IsOn).ToList();

                if (zonesCurrentlyOn.Any())
                {
                    if (zonesCurrentlyOn.Count == 1)
                    {
                        var zoneName = zonesCurrentlyOn.First().Name.ToLower();
                        var message = "The heating is on ";
                        message += (zoneName == "downstairs") ? $"{zoneName}" : $"in the {zoneName}";
                        await context.PostAsync(message);
                    }
                    else
                    {
                        foreach (var currentZone in zonesCurrentlyOn)
                        {
                            // add 'in' if first element and not downstairs
                            zones += (Equals(zonesCurrentlyOn.First(), currentZone)
                                && currentZone.Name.ToLower() != "downstairs")
                                ? "in "
                                : string.Empty;

                            // if last element add 'and' into sentence and not downstairs
                            zones += (Equals(zonesCurrentlyOn.Last(), currentZone))
                                ? "and "
                                : string.Empty;

                            zones += (currentZone.Name.ToLower() != "downstairs")
                                ? $"the {currentZone.Name}, "
                                : $"{currentZone.Name}, ";
                        }

                        await context.PostAsync($"The heating is on {zones}");
                    }
                }
                else
                {
                    await context.PostAsync("The heating is off everywhere");
                }

                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry. I don't know what you wanted.");
            context.Wait(MessageReceived);
        }

        private static void ToggleZone(int zoneNumber)
        {
            var webClient = new WebClient();
            webClient.DownloadString($"http://garypretty3.ddns.net/cgi/leds.cgi?led={zoneNumber}");
        }

        private static int GetZoneNumberFromZoneEntity(EntityRecommendation zone)
        {
            int zoneNumber = -1;

            if (zone != null)
            {
                var dict = zone.Resolution.Values.GetEnumerator();
                dict.MoveNext();
                var valuesList = (List<object>)dict.Current;
                var zoneNumberResolution = Convert.ToInt16((string)valuesList[0]);

                if (zoneNumberResolution != null)
                {
                    zoneNumber = Convert.ToInt16(zoneNumberResolution);
                }
            }
            return zoneNumber;
        }

        private static Attachment GetHeroCard(string message)
        {
            var heroCard = new HeroCard
            {
                // title of the card  
                Title = "Home Heating",
                //subtitle of the card  
                Subtitle = "Subtitle of the card",
                // navigate to page , while tab on card  
                Tap = new CardAction(ActionTypes.OpenUrl, "Learn More", value: "http://www.garypretty.co.uk"),
                //Detail Text  
                Text = message,
                // list of  Large Image  
                Images = new List<CardImage> { new CardImage("https://i.pinimg.com/originals/f2/85/58/f28558497b56c4225eb2d4c0ad566c6f.jpg") }
            };

            return heroCard.ToAttachment();
        }

        public async Task<HeatingStatus> GetHeatingStatus()
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create("http://garypretty3.ddns.net/cgi/status.cgi");
            var httpResponse = (HttpWebResponse)await httpRequest.GetResponseAsync();

            if (httpResponse != null)
            {
                using (StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.ASCII))
                {
                    string xmlStr = sr.ReadToEnd();
                    xmlStr = $"<xml xmlns=\"xxx\">{xmlStr}</xml>";
                    xmlStr = xmlStr.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\\", "");

                    using (StringReader strr = new StringReader(xmlStr))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(xml));
                        xml vm201status = new xml();
                        try
                        {
                            vm201status = (xml)serializer.Deserialize(strr);
                            return new HeatingStatus(vm201status);
                        }
                        catch (Exception ex)
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }
    }
}
