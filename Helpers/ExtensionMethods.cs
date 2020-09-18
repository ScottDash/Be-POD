using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using ProofOfDeliveryAPI.Entities;

namespace ProofOfDeliveryAPI.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutPassword());
        }
        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            return user;
        }
        public static string RemoveWhitespace(string file)
        {
            return Regex.Replace(file, @"\s+", "");
        }
    }
}