using System;
using System.Linq;

public class RandomManager
{
    private static readonly Random random = new Random();
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public string randomString;


    // ramdom chuỗi ký tự
    public static string GenerateRandomString(int length)
    {
        if (length <= 0 || length > chars.Length)
        {
            throw new ArgumentOutOfRangeException("Độ dài", "Độ dài phải lớn hơn 0 hoặc nhỏ hơn " + chars.Length);
        }

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }


    // random id
    public static string GenerateRandomID()
    {
        return GenerateRandomString(30);
    }
}
