﻿namespace ServiceLifeTime.Services
{
    public interface IScopedService
    {
        public string GetGuid();
    }

    public class ScopedService : IScopedService
    {

        private readonly Guid guid;

        public ScopedService()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid() => guid.ToString();
    }
}
