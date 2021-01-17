using Minerva.Server.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Minerva.Server.Core.PlayerBuffs
{
    public class BuffCollection
        : IList<PlayerBuff>
    {
        private readonly List<PlayerBuff> _buffs;
        private readonly ServerPlayer _player;

        public BuffCollection(ServerPlayer player)
        {
            _buffs = new List<PlayerBuff>();
            _player = player;
        }

        public PlayerBuff this[int index] { get => _buffs[index]; set => _buffs[index] = value; }

        public int Count => _buffs.Count;

        public void Add(PlayerBuff buff)
        {
            if (buff == null)
            {
                throw new NotSupportedException("buff cannot be null");
            }

            if (!_player.Exists)
            {
                throw new NotSupportedException("player does not exist anymore");
            }

            _buffs.Add(buff);

            buff.AppliedAt = DateTime.Now;
            buff.OnApplied(_player);
        }

        public void Clear()
        {
            if (!_player.Exists)
            {
                throw new NotSupportedException("player does not exist anymore");
            }

            foreach (var buff in _buffs)
            {
                buff.OnRemoved(_player);
            }

            _buffs.Clear();
        }

        public bool Contains(PlayerBuff buff)
        {
            return _buffs.Contains(buff);
        }

        public IEnumerator<PlayerBuff> GetEnumerator()
        {
            return _buffs.GetEnumerator();
        }

        public int IndexOf(PlayerBuff buff)
        {
            return _buffs.IndexOf(buff);
        }

        public void Insert(int index, PlayerBuff buff)
        {
            if (buff == null)
            {
                throw new NotSupportedException("buff cannot be null");
            }

            if (!_player.Exists)
            {
                throw new NotSupportedException("player does not exist anymore");
            }

            _buffs.Insert(index, buff);

            buff.AppliedAt = DateTime.Now;
            buff.OnApplied(_player);
        }

        public bool Remove(PlayerBuff buff)
        {
            if (buff == null)
            {
                throw new NotSupportedException("buff cannot be null");
            }

            if (!_player.Exists)
            {
                throw new NotSupportedException("player does not exist anymore");
            }

            var result = _buffs.Remove(buff);

            if (result)
            {
                buff.OnRemoved(_player);
            }

            return result;
        }

        public void RemoveAt(int index)
        {
            if (!_player.Exists)
            {
                throw new NotSupportedException("player does not exist anymore");
            }

            if (index < 0 || index >= _buffs.Count)
            {
                throw new ArgumentOutOfRangeException("index is out of range");
            }

            var buffAtIndex = _buffs[index];

            _buffs.RemoveAt(index);

            buffAtIndex.OnRemoved(_player);
        }

        #region hidden by explicit implementation

        bool ICollection<PlayerBuff>.IsReadOnly => ((ICollection<PlayerBuff>)_buffs).IsReadOnly;

        void ICollection<PlayerBuff>.CopyTo(PlayerBuff[] array, int arrayIndex)
        {
            _buffs.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_buffs).GetEnumerator();
        }

        #endregion
    }
}