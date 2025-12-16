using GenericDBGeneration.Tables;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

namespace GenericDBGeneration;
public class AppDataConnection : DataConnection
{   
    public AppDataConnection(): 
        base(SqlServerTools.GetDataProvider(SqlServerVersion.v2022), Resources.ConnectionString)
    {
    }

    // This property exposes the User entity as an operable table (ITable<T>) 
    // within the context, allowing you to run LINQ queries against it.
    public ITable<Users> Users => this.GetTable<Users>();
    public ITable<UserLogs> UserLogs => this.GetTable<UserLogs>();
}