
namespace Conduit.Integration.Tests.Support
{
    public class RunsInCleanRoom : System.IDisposable
    {
        private readonly CleanRoom _cleanRoom;

        public RunsInCleanRoom()
        {
            _cleanRoom = new CleanRoom(".tmp");
            _cleanRoom.Enter();
        }

        public void Dispose()
        {
            //_cleanRoom.Exit();
        }
    }
}