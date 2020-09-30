using System.Collections.Generic;
using System.Data.SqlTypes;
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
            user.Password = "*****";
            return user;
        }
        public static string RemoveWhitespace(string file)
        {
            return Regex.Replace(file, @"\s+", "");
        }

        public static bool CheckIfPDF(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".pdf");
        }
        public static int? ToNullableInt32(this SqlInt32 value)
        {
            return value.IsNull ? (int?)null : value.Value;
        }
    }
}