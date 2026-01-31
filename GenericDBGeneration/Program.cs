using GenericDBGeneration;
using GenericDBGeneration.MyLinqSql;
using GenericDBGeneration.Tables;
using GenericDBGeneration.Tables.Common;
using GenericDBGeneration.Triggers;
using LinqToDB;
using LinqToDB.Data;

var myUsersTable = new Users() {
    fullName = "isfanyar siddiqui",
    preferedName = "isfanyar",
    password = "password",
};
var myUserLogsTable = new UserLogs(myUsersTable, SQLActionType.Insert) { changes = ""};

TriggerType[] tts = [TriggerType.Insert, TriggerType.Delete, TriggerType.Update];
foreach(var tt in tts)
{
    var temp = LogginTriggers.Create
    (
        nameof(AppDataConnection.Users),
        nameof(AppDataConnection.UserLogs),
        nameof(AppDataConnection.Users),
        tt
    );
    Console.WriteLine(temp);
    Console.WriteLine();
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

    db.DropTable<Users>();
    db.DropTable<UserLogs>();
    db.CreateTable<Users>();
    db.CreateTable<UserLogs>();

    db.BulkCopy(options, Users.CreateFakeUsers(20));


    //var users = db.Query<User>("select * from Users").ToList();
    var users = await db.Users.ToArrayAsync();

    foreach (var u in users)
        Console.WriteLine($"{u.userId} || {u.preferedName} || {u.password} || {u.email} || {u.lastActionDate}");

    Console.WriteLine();
    Console.WriteLine("The Users table has been successfully dropped and recreated in SQL Server.");
}

