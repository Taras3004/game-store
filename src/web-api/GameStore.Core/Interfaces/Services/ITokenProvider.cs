using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Services
{
    internal interface ITokenProvider
    {
        string CreateToken(User user);
    }
}
