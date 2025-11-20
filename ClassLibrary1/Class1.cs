using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;

namespace MyDbLayer
{
    [ComVisible(true)]
    [Guid("87654321-dcba-1234-fe00-0987654321ab")] // another unique GUID
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDbLayer
    {
        string ValidateUser(string username, string password);
        bool ReturnTrue(string s);
    }

    [ComVisible(true)]
    [Guid("abcdef12-3456-7890-abcd-ef1234567890")] // class GUID
    [ClassInterface(ClassInterfaceType.None)]
    public class DbLayer : IDbLayer
    {
        const string _connectionString =
            "Server=tcp:localhost\\SQLEXPRESS,1433;Database=asplogin;User Id=sa;Password=cure2000;Trusted_Connection=True;";

        public bool ReturnTrue(string s) => s.StartsWith("s");

        public string ValidateUser(string username, string password)
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using(var cmd = 
                    new SqlCommand("SELECT * FROM Users WHERE Username=@u AND Password=@p", conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(!reader.HasRows)
                        {
                            return "No matching user found.";
                        }

                        var sb = new StringBuilder();

                        while(reader.Read())
                        {
                            sb.AppendLine($"<li>{reader.GetInt32(0)}</li>"); // Id
                            sb.AppendLine($"<li>{reader.GetString(1)}</li>"); // Username
                            sb.AppendLine($"<li>{reader.GetString(2)}</li>"); // Password
                            sb.AppendLine($"<li>{reader.GetString(3)}</li>"); // Email
                            sb.AppendLine($"<li>{reader.GetString(4)}</li>"); // FirstName
                            sb.AppendLine($"<li>{reader.GetString(5)}</li>"); // LastName
                            sb.AppendLine($"<li>{reader.GetString(6)}</li>"); // DisplayName
                            sb.AppendLine($"<li>{reader.GetDateTime(7):yyyy-MM-dd HH:mm:ss}</li>"); // DOB
                        }

                        return sb.ToString();
                    }
                }
            }
        }
    }
}
