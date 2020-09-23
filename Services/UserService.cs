using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Entities;
using ProofOfDeliveryAPI.Helpers;

namespace ProofOfDeliveryAPI.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAllUsers();
        public User AddUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly ConnectionStrings _connectionStrings;
        public readonly string TableName = "User";
        public UserService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;       
        }

        // user data hardcoded for initial testing
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test", Admin = true }
        };

        public async Task<User> Authenticate(string username, string password)
        {
            //var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            List<User> userList = new List<User>();

            string sql = $"SELECT Username, Password FROM [User] WHERE Username = '{username}' AND Password = '{password}'";

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.PODTestDb))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = sql;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                User users = new User();
                                users.Username = reader.GetString(0);                             
                                users.Password = reader.GetString(1);
                                userList.Add(users);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            if (userList.Count > 0) return userList.FirstOrDefault();
            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsers( )
        {
            //return await Task.Run(() => _users.WithoutPasswords());
            
            List<User> userList = new List<User>();

            string sql = $"SELECT FirstName, LastName, UserName, Admin FROM [User]";

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.PODTestDb))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = sql;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                User users = new User();
                                users.FirstName = reader.GetString(0);
                                users.LastName = reader.GetString(1);
                                users.Username = reader.GetString(2);
                                users.Admin = reader.GetBoolean(3);                           
                                userList.Add(users);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return userList;
           
    }

        public User AddUser(User user)
        {       
            try
            {
                using var connection = new SqlConnection(_connectionStrings.PODTestDb);
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO [User] (FirstName, LastName, UserName, Password, Admin) Values (@getfirst, @getlast, @getuser, @getpassword, @getadmin)";
                    command.Parameters.Add("@getfirst", SqlDbType.VarChar).Value = user.FirstName;
                    command.Parameters.Add("@getlast", SqlDbType.VarChar).Value = user.LastName;
                    command.Parameters.Add("@getuser", SqlDbType.VarChar).Value = user.Username;
                    command.Parameters.Add("@getpassword", SqlDbType.VarChar).Value = user.Password;
                    command.Parameters.Add("@getadmin", SqlDbType.Bit).Value = user.Admin;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return user.WithoutPassword();
        }
    }
}
