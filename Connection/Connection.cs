using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Assignment.Model;

namespace Assignment.Connection
{
    public class DatabaseConnection
    {
        // Define the connection string as a constant
        private const string ConnectionString = @"Server=(localdb)\localsql;Database=user;Integrated Security=True;Encrypt=False";

        public SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }



        public List<UserDisplayModel> GetUserDetails()
        {
            var users = new List<UserDisplayModel>(); // Use UserDisplayModel here
            string query = "SELECT FirstName, LastName, Gender, DOB, Email FROM Usersdetail";

            using (var connection = OpenConnection())
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new UserDisplayModel // Populate UserDisplayModel object
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        Email = reader["Email"].ToString()
                    });
                }
            }

            return users;
        }
        public int ExecuteScalar(string query, SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                return (int)command.ExecuteScalar();
            }
        }
        // For INSERT/UPDATE/DELETE operations
        public int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
        }

        /*private SqlConnection _connection;

            // Define the connection string as a constant
            private const string ConnectionString = @"Server=(localdb)\localsql;Database=user;Integrated Security=True;Encrypt=False";

            // Open the connection in the constructor
            public DatabaseConnection()
            {
                _connection = new SqlConnection(ConnectionString);
                _connection.Open();
            }

            public SqlConnection GetConnection()
            {
                return _connection;
            }

            public List<UserDetail> GetUserDetails()
            {
                var users = new List<UserDetail>();

                string query = "SELECT FirstName, LastName, Gender, DOB, Email FROM Usersdetail";
                using (var command = new SqlCommand(query, _connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserDetail
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            Email = reader["Email"].ToString()
                        });
                    }
                }

                return users;
            }

            public int ExecuteNonQuery(string query, SqlParameter[] parameters)
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }

            public int ExecuteScalar(string query, SqlParameter[] parameters)
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddRange(parameters);
                    return (int)command.ExecuteScalar();
                }
            }

            // Close connection when no longer needed
            public void CloseConnection()
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }*/
            }
        }
    





