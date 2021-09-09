using System;
using System.Collections.Generic;
using System.Text;
using UnusArmatusLattro.Models;
using System.Linq;
using Npgsql;

namespace UnusArmatusLattro.Repositories
{
    public class UserRepository
    {
        private static readonly string connectionString = "Server=localhost;Port=5432;Database=sup_db1;User ID= sup_g1; Password=Kosing;";
        public List<Username> GetUsers() // List users
        {
            string stmt = "select * from username order by username";


            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();

            Username user = null;
            var users = new List<Username>();

            while (reader.Read())
            {
                user = new Username
                {
                    UserId = (int)reader["id"],
                    Name = (string)reader["name"],
                    Points = (int)reader["points"],
                   

                };
                users.Add(user);

            }
            return users;
        }

        public bool sendUser(User user)
        {
            try
            {
                string stmt = "insert into username(name, points) values(@Name, @Points) ";

                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("Name", user.UserName);
                command.Parameters.AddWithValue("Points", user.Points);

                using var reader = command.ExecuteReader();

                return true;
            }
            catch (PostgresException Ex)
            {
                string errorCode = Ex.SqlState;
                throw new Exception(errorCode);
            }
            

            
        }

    }
}
