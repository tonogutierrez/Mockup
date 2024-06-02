using System;
using System.Data.SqlClient;

namespace MockUp.Helper
{
    public static class GetUserNameByEmailHelper
    {
        public static string Usuario(string email)
        {
            string userName = null;

            using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();
                using (var command =  connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.GetUserByEmail";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Correo", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userName = reader["Nombre"].ToString();
                        }
                    }
                }
                connection.Close();
            }

            return userName;
        }
    }
}

