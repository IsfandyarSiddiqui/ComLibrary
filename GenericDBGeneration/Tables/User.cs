using GenericDBGeneration.Tables.Common;
using LinqToDB.Mapping;
using Bogus;
using GenericDBGeneration.Utils;

namespace GenericDBGeneration.Tables;

[Table("Users")] public class User: LogableBase
{
    [PrimaryKey, Identity] public int userId { get; set; }
    [Str64] public required string fullName { get; set; }
    [Str32] public required string preferedName { get; set; }
    [Str64] public required string password { get; set => field = Crypto.sha256(value); }
    [Str64] public string? email { get; set; }

    private const string gmail = "gmail.com";
    private const string outlook = "outlook.com";
    private static readonly string[] dummyPasswords = ["aa", "aaa", "aaaa"];
    
    public static List<User> CreateFakeUsers(int count) =>
        new Faker<User>()
        .Rules((f, u) =>
        {
            u.fullName = f.Name.FullName();
            u.preferedName = u.fullName.Split(' ').First();

            u.password = f.Random.ArrayElement(dummyPasswords);

            u.email = $"{u.fullName}_{f.Random.Int(10, 99)}@".ToLowerInvariant().Replace(' ', '_');
            if(f.Random.Bool()) u.email += gmail;
            else u.email += outlook;

            u.lastActionByUserId = 0;
        }).Generate(count);
}

[Table("UsersLogs")]
public class UserLogs : LogTableBase
{
    [Association(ThisKey = nameof(originalId), OtherKey = nameof(User.userId), CanBeNull = false)]
    public override int originalId { get; set; }
}
