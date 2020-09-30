using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        public Customer AddCustomer(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ConnectionStrings _connectionStrings;
        public readonly string TableName = "Customer";
        public CustomerService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        // customer data hardcoded for initial testing
        private List<Customer> _customers = new List<Customer>
        {
            new Customer { CustomerId = 1, CustomerCode = "ABC001", PostCode = "BB72FL", MobileNo = "078934348136" },
            new Customer { CustomerId = 2, CustomerCode = "XYZ001", PostCode = "BB99ST", MobileNo = "078934348139" },
            new Customer { CustomerId = 3, CustomerCode = "LMN001", PostCode = "BB55SA", MobileNo = "078934348189" },     
        };

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            List<Customer> customerList = new List<Customer>();

            string sql = $"SELECT CustomerId, CustomerCode, PostCode, MobileNo FROM [{TableName}]";

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
                                Customer customers = new Customer();
                                customers.CustomerId = reader.GetInt32(0);
                                customers.CustomerCode = reader.GetString(1);
                                customers.PostCode = reader.GetString(2);
                                customers.MobileNo = reader.GetString(3);                           
                                customerList.Add(customers);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return customerList;
        }

        public Customer AddCustomer(Customer customer) 
        {
            try
            {
                using var connection = new SqlConnection(_connectionStrings.PODTestDb);
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO [{TableName}] (CustomerCode, PostCode, MobileNo) Values (@getcustomer, @getpostcode, @getmobile)";
                    command.Parameters.Add("@getcustomer", SqlDbType.VarChar).Value = customer.CustomerCode;
                    command.Parameters.Add("@getpostcode", SqlDbType.VarChar).Value = customer.PostCode; 
                    command.Parameters.Add("@getmobile", SqlDbType.VarChar).Value = customer.MobileNo;           
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return customer;
        }
    }
}
