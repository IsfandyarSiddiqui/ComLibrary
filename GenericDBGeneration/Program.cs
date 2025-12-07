using GenericDBGeneration;
using GenericDBGeneration.MyLinqSql;
using GenericDBGeneration.Tables;
using LinqToDB;
using LinqToDB.Data;
using System.Linq.Expressions;
//using Microsoft.IdentityModel.Tokens;



RunManualExpressionDemo(u => u.userId > 10);


void RunManualExpressionDemo(Expression<Func<User, bool>> expr)
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

