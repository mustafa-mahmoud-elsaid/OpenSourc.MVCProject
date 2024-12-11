namespace ServiceLifeTime.Services
{
    public interface ITransientService
    {
        public string GetGuid();
    }

    public class TransientService : ITransientService
    {

        private readonly Guid guid;

        public TransientService()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid() => guid.ToString();
    }
}
