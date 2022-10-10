using System;
using System.Collections.Generic;
using System.Net;
using AdministrationCall.Interfaces;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using UnityEngine.Networking;
using Utf8Json;

namespace AdministrationCall.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CallCommand : ICommand
    {
        public string Command => "call";
        public string[] Aliases { get; } = { "calladmin", "helpop", "callstaff" };
        public string Description => "Call an admin for help.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.Get(sender) is Player player)
            {
                if (!Plugin.Singleton.Cooldowns.TryGetValue(player.UserId, out DateTime cooldown) || (DateTime.Now - cooldown).TotalSeconds > Plugin.Singleton.Config.Cooldown)
                {
                    Plugin.Singleton.Cooldowns.Add(player.UserId, DateTime.Now.AddSeconds(Plugin.Singleton.Config.Cooldown));
                    Timing.RunCoroutine(SendCall(player.Nickname, string.Join(" ", arguments)));
                    response = "Your call has been sent.";
                    return true;
                }
                response = $"You must wait {Plugin.Singleton.Config.Cooldown} seconds before sending another call.";
                return false;
            }
            response = "You must be a player to use this command.";
            return false;
        }

        private IEnumerator<float> SendCall(string player, string reason)
        {
            var webRequest = new UnityWebRequest(Plugin.Singleton.Config.WebhookUrl, UnityWebRequest.kHttpVerbPOST);
            var message = new { content = Plugin.Singleton.Translation.AdminCall.Replace("%PLAYER%", player).Replace("%REASON%", reason) };
            var uploadHandler = new UploadHandlerRaw(JsonSerializer.Serialize(message))
            {
                contentType = "application/json"
            };
            webRequest.uploadHandler = uploadHandler;

            yield return Timing.WaitUntilDone(webRequest.SendWebRequest());

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Log.Error(
                    $"An error occurred while sending call message: {webRequest.responseCode}\n{webRequest.error}");
            }
        }
    }
}