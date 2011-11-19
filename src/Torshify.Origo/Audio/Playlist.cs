using System;
using System.Collections.Generic;
using System.Linq;

namespace Torshify.Origo.Audio
{
    public class Playlist<T>
        where T : class
    {
        #region Fields

        private static Random _random = new Random();

        private T _current;
        private List<T> _playlist;
        private int _playlistIndex;
        private int[] _playlistIndices;
        private Queue<T> _queue;
        private bool _repeat;
        private bool _shuffle;

        #endregion Fields

        #region Constructors

        public Playlist()
        {
            _playlist = new List<T>();
            _queue = new Queue<T>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler CurrentChanged;

        #endregion Events

        #region Properties

        public T Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                OnCurrentChanged();
            }
        }

        public bool Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        public IEnumerable<T> Sequence
        {
            get
            {
                List<T> sequence = new List<T>();

                sequence.Add(Current);
                sequence.AddRange(_queue);

                for (int i = _playlistIndex; i < _playlistIndices.Length; i++)
                {
                    var item = _playlist[_playlistIndices[i]];

                    if (!sequence.Contains(item))
                    {
                        sequence.Add(item);
                    }
                }

                return sequence;
            }
        }

        public bool Shuffle
        {
            get { return _shuffle; }
            set
            {
                _shuffle = value;

                if (_shuffle)
                {
                    _playlistIndices = BuildShuffledIndexArray(_playlist.Count);
                }
                else
                {
                    _playlistIndices = Enumerable.Range(0, _playlist.Count).ToArray();
                }
            }
        }

        #endregion Properties

        #region Methods

        public void Enqueue(params T[] tracks)
        {
            foreach (var track in tracks)
            {
                _queue.Enqueue(track);
            }
        }

        public void Initialize(params T[] tracks)
        {
            _playlist = new List<T>(tracks);
            _playlistIndex = 0;
            _playlistIndices = Enumerable.Range(0, _playlist.Count).ToArray();

            Current = _playlist[_playlistIndices[_playlistIndex]];
        }

        public void Next()
        {
            if (_queue.Count > 0)
            {
                Current = _queue.Dequeue();
            }
            else
            {
                _playlistIndex++;

                if (_playlistIndex >= _playlistIndices.Length)
                {
                    return;
                }

                Current = _playlist[_playlistIndices[_playlistIndex]];
            }
        }

        public void Previous()
        {
            _playlistIndex--;

            if (_playlistIndex < 0)
            {
                return;
            }

            Current = _playlist[_playlistIndices[_playlistIndex]];
        }

        private static int[] BuildShuffledIndexArray(int size)
        {
            int[] array = new int[size];

            for (int currentIndex = array.Length - 1; currentIndex > 0; currentIndex--)
            {
                int nextIndex = _random.Next(currentIndex + 1);
                Swap(array, currentIndex, nextIndex);
            }

            return array;
        }

        private static void Swap(IList<int> array, int firstIndex, int secondIndex)
        {
            if (array[firstIndex] == 0)
            {
                array[firstIndex] = firstIndex;
            }

            if (array[secondIndex] == 0)
            {
                array[secondIndex] = secondIndex;
            }

            int temp = array[secondIndex];
            array[secondIndex] = array[firstIndex];
            array[firstIndex] = temp;
        }

        private void OnCurrentChanged()
        {
            var handler = CurrentChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion Methods
    }
}