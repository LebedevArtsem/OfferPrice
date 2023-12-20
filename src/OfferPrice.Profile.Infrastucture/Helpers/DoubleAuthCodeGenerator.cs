namespace OfferPrice.Profile.Infrastructure.Helpers;

public static class DoubleAuthCodeGenerator
{
    public static string Generate(int length)
    {
        var code = string.Empty;

        var random = new Random();

        for (int i = 0; i < length; i++)
        {
            code += random.Next(10);
        }

        return code;
    }
}

