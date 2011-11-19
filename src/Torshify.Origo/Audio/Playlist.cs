using System;
using System.Collections.Generic;
using System.Linq;

namespace Torshify.Origo.Audio
{
    public class Playlist<T>
        where T : class
    {
        #region Fields

        private T _current;
        private List<T> _playlist;
        private int _playlistIndex;
        private int[] _playlistIndices;
        private Queue<T> _queue;

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

        public IEnumerable<T> Sequence
        {
            get
            {
                List<T> sequence = new List<T>();

                sequence.Add(Current);
                sequence.AddRange(_queue);

                for (int i = _playlistIndex + 1; i < _playlistIndices.Length; i++)
                {
                    sequence.Add(_playlist[_playlistIndices[i]]);
                }

                return sequence;
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