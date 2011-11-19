using NUnit.Framework;
using Torshify.Origo.Audio;
using System.Linq;

namespace Torshify.Origo.Tests
{
    [TestFixture]
    public class PlaylistTest
    {
        #region Fields

        private Playlist<Track> _playlist;

        #endregion Fields

        #region Methods

        [SetUp]
        public void Setup()
        {
            _playlist = new Playlist<Track>();
        }

        [Test]
        public void InitializeEmptyPlaylist_SequenceShouldMatch()
        {
            var tracks = new[]
                             {
                                 new Track("Track1"),
                                 new Track("Track2"),
                                 new Track("Track3")
                             };

            _playlist.Initialize(tracks);

            CollectionAssert.AreEqual(tracks, _playlist.Sequence.ToArray());
        }

        [Test]
        public void InitializeNonEmptyPlaylist_SequenceShouldIncludeEnqueudTracks()
        {
            var enqueued1 = new Track("Enqueued1");
            var enqueued2 = new Track("Enqueued2");
            _playlist.Enqueue(enqueued1, enqueued2);

            var track1 = new Track("Track1");
            var track2 = new Track("Track2");
            var track3 = new Track("Track3");
            _playlist.Initialize(track1, track2, track3);

            var expected = new[]
                               {
                                   track1,
                                   enqueued1,
                                   enqueued2,
                                   track2,
                                   track3
                               };

            CollectionAssert.AreEqual(expected, _playlist.Sequence.ToArray());
        }

        [Test]
        public void NextWithNoQueuedTracks_SequenceShouldMatch()
        {
            var expected = new[]
                             {
                                 new Track("Track1"),
                                 new Track("Track2"),
                                 new Track("Track3")
                             };

            _playlist.Initialize(expected);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], _playlist.Current);
                
                _playlist.Next();
            }
        }

        [Test]
        public void NextWithQueuedTracks_SequenceShouldIncludeEnqueuedTracks()
        {
            var track1 = new Track("Track1");
            var track2 = new Track("Track2");
            var track3 = new Track("Track3");
            var enqueued1 = new Track("Enqueued1");
            var enqueued2 = new Track("Enqueued2");
            _playlist.Initialize(track1, track2, track3);
            _playlist.Enqueue(enqueued1, enqueued2);

            var expected = new[]
                               {
                                   track1,
                                   enqueued1,
                                   enqueued2,
                                   track2,
                                   track3
                               };

            for (int i = 0; i < expected.Length - 1; i++)
            {
                Assert.AreEqual(expected[i], _playlist.Current);

                _playlist.Next();
            }
        }

        [Test]
        public void Previous_SequenceShouldBeReverse()
        {
            var track1 = new Track("Track1");
            var track2 = new Track("Track2");
            var track3 = new Track("Track3");
            _playlist.Initialize(track1, track2, track3);
            _playlist.Next();
            _playlist.Next();

            var expected = new[]
                               {
                                   track3,
                                   track2,
                                   track1
                               };

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], _playlist.Current);

                _playlist.Previous();
            }
        }

        #endregion Methods

        #region Nested Types

        public class Track
        {
            #region Constructors

            public Track(string name)
            {
                Name = name;
            }

            #endregion Constructors

            #region Properties

            public string Name
            {
                get;
                set;
            }

            #endregion Properties

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion Nested Types
    }
}