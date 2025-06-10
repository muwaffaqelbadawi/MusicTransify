using System;

namespace MusicTransify.src.Contracts
{
    public interface IAuthController
    {
        void Login(string username, string password);
        void logout(string username, string password);
    }
}