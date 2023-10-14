namespace RainHound.WeatherApi.Business.UrlBuilders;

public class BoolToYesNoConverter
{
    public static string Convert(bool value)
    {
        return value ? "yes" : "no";
    }
}
