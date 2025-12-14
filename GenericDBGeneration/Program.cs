using GenericDBGeneration;
using GenericDBGeneration.MyLinqSql;
using GenericDBGeneration.Tables;
using GenericDBGeneration.Tables.Common;
using LinqToDB;
using LinqToDB.Data;
using System.Linq.Expressions;
using System.Threading.Channels;
//using Microsoft.IdentityModel.Tokens;

//SqkGen(u => u.userId > 10);
string[] parameters = ["userId", "fullName", "email"];
var x = TriggerGen<User, UserLogs>( (u, ul) => false);
Console.WriteLine(x);

string TriggerGen<T1,T2>(Expression<Func<T1, T2, bool>> expr) where T1 : LogableBase where T2 : LogableBase
{
    var x = new TriggerHelper<T1, T2>();

    return x.Create(expr);
}

void SqkGen(Expression<Func<User, bool>> expr)
{
    Console.WriteLine("--- Starting Lambda Expression Demo ---");
    // Now you just visit the body, exactly like before
    var visitor = new SimpleSqlVisitor();
    string sql = visitor.Translate(expr.Body);

    Console.WriteLine($"Generated SQL: {sql}");
    Console.WriteLine("--- End Demo ---");
}

//await GenerateSomeSqlAsync();
async Task GenerateSomeSqlAsync()
{
    using var db = new AppDataConnection();
    var options = new BulkCopyOptions
    {
        KeepIdentity = false,
        CheckConstraints = true,
        FireTriggers = true,
        NotifyAfter = 50
    };

    db.DropTable<User>();
    db.DropTable<UserLogs>();
    db.CreateTable<User>();
    db.CreateTable<UserLogs>();

    db.BulkCopy(options, User.CreateFakeUsers(20));


    //var users = db.Query<User>("select * from Users").ToList();
    var users = await db.Users.ToArrayAsync();

    foreach (var u in users)
        Console.WriteLine($"{u.userId} || {u.preferedName} || {u.password} || {u.email} || {u.lastActionDate}");

    Console.WriteLine();
    Console.WriteLine("The Users table has been successfully dropped and recreated in SQL Server.");
}

