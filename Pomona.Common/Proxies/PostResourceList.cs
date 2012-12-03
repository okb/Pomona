﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Pomona.Common.Proxies
{
    internal class PostResourceList<T> : IList<T>
    {
        private readonly string propertyName;
        private PutResourceBase owner;
        private List<T> wrapped = new List<T>();

        internal PostResourceList(PutResourceBase owner, string propertyName)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            this.owner = owner;
            this.propertyName = propertyName;
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return wrapped.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private void SetDirty()
        {
            owner.SetDirty(propertyName);
        }

        #region Implementation of ICollection<T>

        public void Add(T item)
        {
            wrapped.Add(item);
            SetDirty();
        }


        public void Clear()
        {
            if (wrapped.Count > 0)
            {
                wrapped.Clear();
                SetDirty();
            }
        }


        public bool Contains(T item)
        {
            return wrapped.Contains(item);
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            wrapped.CopyTo(array, arrayIndex);
        }


        public bool Remove(T item)
        {
            return wrapped.Remove(item);
        }


        public int Count
        {
            get { return wrapped.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of IList<T>

        public int IndexOf(T item)
        {
            return wrapped.IndexOf(item);
        }


        public void Insert(int index, T item)
        {
            wrapped.Insert(index, item);
            SetDirty();
        }


        public void RemoveAt(int index)
        {
            wrapped.RemoveAt(index);
            SetDirty();
        }


        public T this[int index]
        {
            get { return wrapped[index]; }
            set
            {
                wrapped[index] = value;
                SetDirty();
            }
        }

        #endregion
    }
}