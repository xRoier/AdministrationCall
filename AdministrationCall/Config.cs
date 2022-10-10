using System.ComponentModel;
using Exiled.API.Interfaces;

namespace AdministrationCall
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("The webhook to send the message to.")]
        public string WebhookUrl { get; set; } = "change me";

        [Description("Command cooldown in seconds")]
        public int Cooldown { get; set; } = 60;
    }
}