namespace ProductAPI.Extensions;

public static class DecimalExtension
{
    public static int ToInt(this decimal number)
    {
        return (int)Math.Floor(number); 
    }
    
}