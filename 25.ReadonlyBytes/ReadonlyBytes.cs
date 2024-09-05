using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
		private readonly byte[] _data;
		private int _hash;

		public int Length {  get {  return _data.Length; } }

		public ReadonlyBytes(params byte[] data)
		{
            _data = data ?? throw new ArgumentNullException();
            ComputeHashCode();
		}

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;

            var other = obj as ReadonlyBytes;
			if (_data.Length != other.Length) return false;

			for (int i = 0; i < _data.Length; i++)
			{
				if (_data[i] != other[i]) return false;
			}

            return true;
        }

		private void ComputeHashCode()
		{
            unchecked
            {
                const int fnvPrime = 16777619;
                var hash = (int)2166136261;

                hash = (hash * fnvPrime) ^ GetType().GetHashCode();

                foreach (var item in _data)
                {
                    hash = (hash * fnvPrime) ^ item;
                }

                _hash = hash;
            }
        }

		public override int GetHashCode()
		{
			return _hash;
		}

        public override string ToString()
        {
			var sb = new StringBuilder();
			sb.Append('[');
			for (int i = 0; i < Length; i++)
			{
				sb.Append(_data[i]);
                if (i < Length - 1)
                {
					sb.Append(',');
                    sb.Append(' ');
                }
            }
            sb.Append(']');
            return new string(sb.ToString());
        }

        public IEnumerator<byte> GetEnumerator()
        {
			foreach (var item in _data)
			{
				yield return item;
			}
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public byte this[int index]
		{
			get
			{
                if (index < 0 || index >= _data.Length) throw new IndexOutOfRangeException();
				return _data[index];
            }
		}
    }
}