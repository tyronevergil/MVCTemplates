using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Persistence;
using Persistence.Specifications;

namespace WebApplication.Infrastructure
{
    public class ApplicationUserManager
    {
        private readonly IUserDataContextFactory _contextFactory;

        public ApplicationUserManager(IUserDataContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public ApplicationUser Find(string username, string password)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var user = context.FindSingle(UserSpecs.GetUser(username));
                if (user != null)
                {
                    if (SimpleHash.VerifyHash(password, user.PasswordHash))
                    {
                        return new ApplicationUser(user.Username, user.Roles.Select(r => r.Role).ToArray());
                    }
                }
            }

            return null;
        }

        public void UpdatePassword(string username, string password)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var user = context.FindSingle(UserSpecs.GetUser(username));
                if (user != null)
                {
                    user.PasswordHash = SimpleHash.ComputeHash(password);
                    context.SaveChanges();
                }
            }
        }

        /* https://github.com/obviex/Samples/blob/master/Hash.md */
        public class SimpleHash
        {
            public static string ComputeHash(string password)
            {
                var random = new Random();
                var saltSize = random.Next(4, 8);
                var saltBytes = new byte[saltSize];
                var rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(saltBytes);

                return ComputeHash(password, saltBytes);
            }

            public static string ComputeHash(string password, byte[] saltBytes)
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(password);

                var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
                for (int i = 0; i < plainTextBytes.Length; i++)
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                for (int i = 0; i < saltBytes.Length; i++)
                    plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

                var hash = new MD5CryptoServiceProvider();
                var hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

                var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
                for (int i = 0; i < hashBytes.Length; i++)
                    hashWithSaltBytes[i] = hashBytes[i];
                for (int i = 0; i < saltBytes.Length; i++)
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

                string hashValue = Convert.ToBase64String(hashWithSaltBytes);
                return hashValue;
            }

            public static bool VerifyHash(string password, string hashValue)
            {
                var hashWithSaltBytes = Convert.FromBase64String(hashValue);
                var hashSizeInBytes = 16;
                if (hashWithSaltBytes.Length < hashSizeInBytes)
                    return false;

                var saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];
                for (int i = 0; i < saltBytes.Length; i++)
                    saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

                string expectedHashString = ComputeHash(password, saltBytes);
                return (hashValue == expectedHashString);
            }
        }
    }

    public class ApplicationUser
    {
        public ApplicationUser(string username, IEnumerable<string> roles)
        {
            UserName = username;
            Roles = roles;
        }

        public string UserName { get; private set; }
        public IEnumerable<string> Roles { get; private set; }
    }
}
