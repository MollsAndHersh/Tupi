﻿using System;
using System.Runtime.CompilerServices;

namespace Tupi.Indexing.Filters
{
    // based on: 
    // https://github.com/ayende/Corax/blob/master/Corax/Indexing/Filters/ArraySegmentKey.cs
    internal struct CharArraySegmentKey
        : IEquatable<CharArraySegmentKey>
    {
        private readonly string _rawString;
        private readonly char[] _buffer;
        private readonly int _size;
        private readonly int _hc;

        public CharArraySegmentKey(char[] buffer)
            : this(buffer, buffer.Length)
        {
        }

        public CharArraySegmentKey(char[] buffer, int size)
            : this()
        {
            _buffer = buffer;
            _size = size;

            unchecked
            {
                _hc = _size;
                for (var i = 0; i < _size; i++)
                {
                    var c = _buffer[i];
                    _hc = (c | (c << 16)) * 397 ^ _hc;
                }
            }
            
        }

        public CharArraySegmentKey(string buffer)
            : this()
        {
            _rawString = buffer;
            _size = _rawString.Length;

            unchecked
            {
                _hc = _size;
                for (var i = 0; i < _size; i++)
                {
                    var c = _rawString[i];
                    _hc = (c | (c << 16)) * 397 ^ _hc;
                }
            }
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CharArraySegmentKey other)
        {
            if (_size != other._size)
            {
                return false;
            }

            if (_rawString != null)
            {
                if (other._rawString != null)
                {
                    return _rawString == other._rawString;
                }

                for (var i = 0; i < _size; i++)
                {
                    if (_rawString[i] != other._buffer[i])
                        return false;
                }

                return true;
            }

            if (other._rawString != null)
            {
                for (var i = 0; i < _size; i++)
                {
                    if (_buffer[i] != other._rawString[i])
                        return false;
                }

                return true;
            }

            for (var i = 0; i < _size; i++)
            {
                if (_buffer[i] != other._buffer[i])
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is CharArraySegmentKey key && Equals(key);
        }

        public override int GetHashCode() => _hc;

        public override string ToString()
        {
            if (_rawString != null)
                return _rawString;

            if (_buffer is char[] c)
                return new string(c, 0, _size);
            
            return base.ToString();
        }

        public bool IsStable => _rawString != null;

        public CharArraySegmentKey Stabilize() => IsStable
            ? this
            : new CharArraySegmentKey(ToString());
    }
}