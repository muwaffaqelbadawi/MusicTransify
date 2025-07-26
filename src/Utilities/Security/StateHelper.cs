using System;
using System.Security.Cryptography;

namespace MusicTransify.src.Utilities.Security
{
    public class StateHelper
    {
        public string GenerateSecureRandomString(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];
                rng.GetBytes(byteBuffer);

                var chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[byteBuffer[i] % validChars.Length];
                }
                return new string(chars);
            }
        }
    }
}