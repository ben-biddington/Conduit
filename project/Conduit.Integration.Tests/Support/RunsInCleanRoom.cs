
namespace Conduit.Integration.Tests.Support
{
    public class RunsInCleanRoom : System.IDisposable
    {
        private readonly CleanRoom _cleanRoom;

        protected void NoDelete()
        {
            _cleanRoom.NoDelete ();
        }

        public RunsInCleanRoom()
        {
            _cleanRoom = new CleanRoom(".tmp");
            _cleanRoom.Enter();
        }

        public void Dispose()
        {
            _cleanRoom.Exit();
        }
    }
}