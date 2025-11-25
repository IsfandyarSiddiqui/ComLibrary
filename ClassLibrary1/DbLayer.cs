using ClassLibrary1;
using System;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Reflection;
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

    [Guid("abcdef12-3456-7890-abcd-ef1234567890")] // class GUID
    public class DbLayer : BaseDBLayer, IDbLayer
    {
        public bool ReturnTrue(string s) => s.StartsWith("s");

        [AutoComplete] public string ValidateUser(string username, string password)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd =
                    new SqlCommand("SELECT * FROM Users WHERE Username=@u AND Password=@p", conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return "No matching user found.";
                        }

                        var sb = new StringBuilder();

                        while (reader.Read())
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

        [AutoComplete] public object GetAdoobRecordset(string startsWith)
        {
            // 1. Fetch the data into a DataTable
            var dt = new DataTable();

            using(var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Username, Email, DisplayName FROM Users WHERE Username LIKE @p";
                using(var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p", startsWith + "%");
                    // Use a data adapter to fill the DataTable
                    using(var adapter = new SqlDataAdapter(cmd)) { adapter.Fill(dt); }
                } // Connection closes here, freeing the database resource immediately.
            }

            // 2. Convert the DataTable into a disconnected ADODB.Recordset

            // Create an instance of the ADODB Recordset object
            // It will be marshaled to the ASP client via COM.
            var recordset = new ADODB.Recordset();

            // ADO.NET provider will convert the DataTable/DataReader into ADODB Recordset for COM interop.
            // The Recordset must be opened with adOpenStatic and adLockBatch for disconnected use,
            // The data source is the DataTable itself.
            recordset.Open(
                dt,
                Missing.Value, // Using Missing.Value for optional parameters
                ADODB.CursorTypeEnum.adOpenStatic,
                ADODB.LockTypeEnum.adLockBatchOptimistic,
                (int)ADODB.CommandTypeEnum.adCmdUnknown // Treat source as a table/result set
            );

            // 3. Disconnect the Recordset (this is the key step)
            // Setting the ActiveConnection to Nothing makes it "disconnected"
            recordset.ActiveConnection = null;

            // 4. Return the Recordset object.
            // The 'object' return type allows the COM marshaller to handle the ADODB.Recordset
            return recordset;
        }
    }
}
