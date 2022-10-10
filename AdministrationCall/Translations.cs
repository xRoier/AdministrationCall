using System.ComponentModel;
using Exiled.API.Interfaces;

namespace AdministrationCall
{
    public class Translations : ITranslation
    {
        [Description("Set the message that will be sent to the webhook when someone calls for an admin.")]
        public string AdminCall { get; set; } = "%PLAYER% has called an admin with the reason: \n> %REASON%";
    }
}