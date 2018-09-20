namespace Sulakore.Crypto
{
    public class RC4
    {
        private int _i, _j;
        private readonly int[] _table;
        private readonly object _parseLock;

        public byte[] Key { get; }

        public RC4(byte[] key)
        {
            _table = new int[256];
            _parseLock = new object();

            Key = key;

            for (int i = 0; i < 256; i++)
            {
                _table[i] = i;
            }
            for (int j = 0, x = 0; j < _table.Length; j++)
            {
                x += _table[j];
                x += key[j % key.Length];
                x %= _table.Length;
                Swap(j, x);
            }
        }

        public byte[] Parse(byte[] data)
        {
            lock (_parseLock)
            {
                var parsed = new byte[data.Length];
                for (int k = 0; k < data.Length; k++)
                {
                    _i++;
                    _i %= _table.Length;
                    _j += _table[_i];
                    _j %= _table.Length;
                    Swap(_i, _j);

                    int rightXOR = (_table[_i] + _table[_j]);
                    rightXOR = _table[rightXOR % _table.Length];

                    parsed[k] = (byte)(data[k] ^ rightXOR);
                }
                return parsed;
            }
        }
        
        private void Swap(int a, int b)
        {
            int temp = _table[a];
            _table[a] = _table[b];
            _table[b] = temp;
        }
    }
}