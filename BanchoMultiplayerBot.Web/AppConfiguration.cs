using System.ComponentModel.DataAnnotations;

namespace BanchoMultiplayerBot.Web;

public class AppConfiguration(IConfiguration configuration)
{
    public Uri BackendUri { get; set; } = new(configuration.GetRequiredSection("BackendUrl").Value!);
}