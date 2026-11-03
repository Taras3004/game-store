using System.Security.Cryptography;
using System.Text;

namespace GameStore.DAL.Mongo
{
    public static class GuidExtensions
    {
        public static Guid ToDeterministicGuid(this string legacyIdentifier)
        {
            using var md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(legacyIdentifier));
            return new Guid(hash);
        }
    }
}
