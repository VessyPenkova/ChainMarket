using Microsoft.AspNetCore.Http;

namespace ChainMarket.Tests
{
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _data = new();

        public bool IsAvailable => true;

        public string Id { get; } = Guid.NewGuid().ToString();

        public IEnumerable<string> Keys => _data.Keys;

        public void Clear()
        {
            _data.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _data.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _data[key] = value.ToArray();
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_data.TryGetValue(key, out var storedValue))
            {
                value = storedValue.ToArray();
                return true;
            }

            value = Array.Empty<byte>();
            return false;
        }
    }
}