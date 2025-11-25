using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComponentAccessControl(false)] // Security is not needed, so explicitly disable.
    [JustInTimeActivation(true)] // RECOMMENDED: Enables automatic object lifetime management.
    [Transaction(TransactionOption.Required)]
    [Synchronization(SynchronizationOption.Required)]
    [ObjectPooling(true, MinPoolSize = 1, MaxPoolSize = 10, CreationTimeout = 60_000)] // In miliseconds
    public class BaseDBLayer : ServicedComponent
    {
        protected const string _connectionString = "Server=tcp:localhost\\SQLEXPRESS,1433; Database=asplogin;User Id=sa;Password=cure2000;Trusted_Connection=True;";

        protected override void Activate() { }
        protected override void Deactivate() { }
        protected override void Construct(string s) { }
        protected override void Dispose(bool disposing) { }
        protected override bool CanBePooled() => true;

    }
}
