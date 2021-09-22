using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLWR
{
    /// <summary>
    /// Create an instance and set the ConnectionString property.
    /// </summary>
    public class SQL
    {
        public string ConnectingString { get; set; }
        public delegate void GetObject<T>(SqlDataReader reader, List<T> list);
        public delegate void FillParams(SqlCommand com);
        public SQL() { }
        public SQL(string connectionString)
        {
            this.ConnectingString = connectionString;
        }
        /// <summary>
        /// Insert / Update Function
        /// </summary>
        /// <param name="query"></param>
        /// <param name="funcion to set parameters"></param>
        /// <returns></returns>
        public int Write(string query, FillParams func)
        {

            using (SqlConnection conn = new SqlConnection(this.ConnectingString))
            {
                int result = 0;
                try
                {
                    conn.Open();
                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        func(com);
                        result = com.ExecuteNonQuery();
                    }
                    using (SqlCommand com = new SqlCommand($"SELECT COUNT(*) FROM [dbo].[User];", conn))
                    {
                        result = (int)com.ExecuteScalar();
                    }
                }
                catch (SqlException)
                {

                }
                catch (Exception)
                {

                }
                finally
                {
                    conn.Close();
                }
                return result;
            }
        }
        /// <summary>
        /// Read Data Function
        /// </summary>
        /// <typeparam name="object type"></typeparam>
        /// <param name="query"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public List<T> Read<T>(string query, GetObject<T> func)
        {
            List<T> users = new List<T>();
            using (SqlConnection conn = new SqlConnection(this.ConnectingString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand com = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            func(reader, users);
                        }
                        reader.Close();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                catch (Exception)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
            return users;
        }
        public string Help()
        {
            return @"
using SQLWR;

Create Instance:
    SQL sql = new SQL(@'Server=.\SQLEXPRESS;Database=databsename;Trusted_Connection=True;');
        OR
    SQL sql = new SQL();
    sql.ConnectingString = @'Server =.\SQLEXPRESS; Database = databsename; Trusted_Connection = True;';

Write:
    sql.Write('INSERT INTO[dbo].[User] VALUES(@id, @username, @password);',
    com => {
        com.Parameters.AddWithValue('@id', 1);
        com.Parameters.AddWithValue('@username', 'my_username');
        com.Parameters.AddWithValue('@password', 'my_password');
    });

Read:
    List<User> user_s = sql.Read<User>('SELECT* FROM[dbo].[User]', (reader, users) =>
    {
        User user = new User();
        user.id = int.Parse(reader['ID'].ToString().Trim());
        user.username = reader['username'].ToString().Trim();
        user.password = reader['password'].ToString().Trim();
        users.Add(user);
    });
            ";
        }
    }
}
