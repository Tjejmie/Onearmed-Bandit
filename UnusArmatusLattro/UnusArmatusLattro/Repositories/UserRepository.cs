﻿using System;
using System.Collections.Generic;
using System.Text;
using UnusArmatusLattro.Models;
using System.Linq;
using Npgsql;
using UnusArmatusLattro.Data;

namespace UnusArmatusLattro.Repositories
{
    public class UserRepository
    {
        
        private static readonly string connectionString= "Server=studentpsql.miun.se; Port=5432; Database=sup_db1; User Id=sup_g1; Password=Kosing; Trust Server Certificate=true; sslmode=Require";
        public List<Username> GetUsers(Difficulties difficulty) 
        {
            string stmt = $"select * from highscore_{difficulty} order by points desc limit 10";


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

        public void sendUser(User user, Difficulties difficulty)
        {
            try
            {
                string stmt = $"insert into highscore_{difficulty}(name, points) values(@Name, @Points) ";

                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("Name", user.UserName);
                command.Parameters.AddWithValue("Points", user.Points);

                using var reader = command.ExecuteReader();

            }
            catch (PostgresException Ex)
            {
                string errorCode = Ex.SqlState;
                var test = Ex.Message;
                throw new Exception(errorCode);
            }



        }

    }
}
