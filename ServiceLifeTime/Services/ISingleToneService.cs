namespace ServiceLifeTime.Services
{
    public interface ISingleToneService
    {
        public string GetGuid();
    }

    public class SingleToneService : ISingleToneService
    {
        //private const int guid1 = 1;
        private readonly Guid guid;

        public SingleToneService()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid() => guid.ToString();

    }
}


