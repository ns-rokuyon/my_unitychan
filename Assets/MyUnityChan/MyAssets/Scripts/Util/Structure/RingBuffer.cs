using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class RingBuffer<T> : IEnumerable<T> {
        public int size { get; private set; }
        private List<T> buffer;
        private int head;

        public RingBuffer(int _size) {
            size = _size;
            head = 0;
            buffer = new List<T>(size);
        }

        public void add(T item) {
            if ( buffer.Count < size ) {
                buffer.Add(item);
                head = buffer.Count - 1;
                return;
            }

            head = ++head % size;
            buffer[head] = item;
        }

        public int count() {
            return buffer.Count;
        }

        public void clear() {
            buffer.Clear();
        }

        public T getHead() {
            return buffer[head];
        }

        public T getLast() {
            return buffer[lastIndex()];
        }

        public int lastIndex() {
            int i = head + 1;
            if ( i >= size ) {
                i = 0;
            }
            return i;
        }

        public IEnumerator<T> GetEnumerator() {
            int last = lastIndex();
            for ( int i = last; i < buffer.Count; i++ ) {
                yield return buffer[i];
            }

            for ( int i = 0; i < buffer.Count; i++ ) {
                yield return buffer[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
