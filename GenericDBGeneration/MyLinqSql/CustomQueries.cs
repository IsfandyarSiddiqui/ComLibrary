using GenericDBGeneration.Tables.Common;
using Microsoft.Data.SqlClient;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace GenericDBGeneration.MyLinqSql;

public class CustomQueries
{
    SqlServerCompiler sqlCompiler = new();
    SqlConnection conn = new(Resources.ConnectionString);
    QueryFactory dbFactory;

    public CustomQueries() => this.dbFactory = new QueryFactory(conn, sqlCompiler);

    public void Create(string ltbName)
    {
        var query = new Query(ltbName).AsInsert(
            new ConcreteLogTableBase(
                new() { lastActionByUserId=-1, lastActionDate=DateTime.UtcNow },
                SQLActionType.Insert
            )
            { changes = "" }
        );
       
        var res = dbFactory.Compiler.Compile(query);
        Console.WriteLine(res.Sql);
    }
}

