using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagLib;
using System.IO;

namespace Tagcmdlet
{
    /// <summary>
    /// Class used to represent tags on the shell.
    /// </summary>
    public class TagSet
    {
        string title;
        string[] performers;
        string[] albumArtists;
        string[] composers;
        string album;
        string comment;
        string[] genres;
        uint year;
        uint track;
        uint trackCount;
        uint disc;
        uint discCount;
        string lyrics;
        string grouping;
        uint beatsPerMinute;
        string conductor;
        string copyright;
        IPicture[] pictures;

        string originalDirectory;
        string originalFullname;
        string originalName;

        public TagSet(string filename)
            { feedPathInfo(filename); }

        public TagSet(string filename, TagLib.Tag t)
        {
            feedPathInfo(filename);
            Absorb(t);
        }

        #region here we play with path and give accessor to them...
        private void feedPathInfo(string filename)
        {
            originalFullname = filename;
            originalName = Path.GetFileName( filename );
            originalDirectory = Path.GetDirectoryName( filename ) + '\\';
        }

        /// <summary>
        /// Here we link the tag with his original file,
        /// this can be useful in the perspective of use in the
        /// powershell (to be able to re-apply modified tags)
        /// </summary>
    	public string OriginalName { get { return originalName; } }
        public string OName { get { return originalName; } }

        public string OriginalFullname { get { return originalFullname; } }
        public string OFullname { get { return originalFullname; } }


        public string OriginalDirectory { get { return originalDirectory; } }
        public string ODirectory { get { return originalDirectory; } }
        #endregion

        #region Boring accessor for previous field (start with a uppercase)
        public virtual string Title {
			get { return title; }
            set { title = value;  }
		}

		public virtual string [] Performers {
			get {return performers;}
            set { performers = value;  }
		}
		
		public virtual string [] AlbumArtists {
			get {return albumArtists;}
            set { albumArtists = value; }
		}
		
		public virtual string [] Composers {
			get {return composers;}
            set { composers = value; }
		}
		
		public virtual string Album {
			get {return album;}
            set { album = value; }
		}

		public virtual string Comment {
			get {return comment;}
            set { comment = value; }
		}

		public virtual string [] Genres {
			get {return genres;}
            set { genres = value; }
		}
		
		public virtual uint Year {
			get {return year;}
            set { year = value; }
		}
		
		public virtual uint Track {
			get {return track;}
            set { track = value; }
		}
		
		public virtual uint TrackCount {
			get { return trackCount; }
            set { trackCount = value; }
		}
		
		public virtual uint Disc {
			get {return disc;}
            set { disc = value; }
		}
		
		public virtual uint DiscCount {
			get {return discCount;}
            set { discCount = value; }
		}
		
		public virtual string Lyrics {
			get {return lyrics;}
            set { lyrics = value; }
		}
		
		public virtual string Grouping {
			get {return grouping;}
            set { grouping = value; }
		}
		
		public virtual uint BeatsPerMinute {
			get {return beatsPerMinute;}
            set { beatsPerMinute = value; }
		}
		
		public virtual string Conductor {
			get {return conductor;}
            set { conductor = value; }
		}
		
		public virtual string Copyright {
			get {return copyright;}
            set { copyright = value; }
		}
		
		public virtual IPicture [] Pictures {
			get {return pictures;}
            set { pictures = value; }
		}

		public string FirstAlbumArtist {
			get {return FirstInGroup(AlbumArtists);}
		}
		
		public string FirstPerformer {
			get {return FirstInGroup(Performers);}
		}
		
		public string FirstComposer {
			get {return FirstInGroup(Composers);}
		}
		
		public string FirstGenre {
			get {return FirstInGroup(Genres);}
        }
        #endregion

        #region Here we take from a taglib tag and get the data...
        public void Absorb( TagLib.Tag t )
        {
            title = t.Title;
            performers = t.Performers;
            albumArtists = t.AlbumArtists;
            composers = t.Composers;
            album = t.Album;
            comment = t.Comment;
            genres = t.Genres;
            year = t.Year;
            track = t.Track;
            trackCount = t.TrackCount;
            disc = t.Disc;
            discCount = t.DiscCount;
            lyrics = t.Lyrics;
            grouping = t.Grouping;
            beatsPerMinute = t.BeatsPerMinute;
            conductor = t.Conductor;
            copyright = t.Copyright;
            pictures = t.Pictures;
        }
        #endregion

        #region Here is the inverse of previous region
        public void InjectIn( TagLib.Tag t )
        {
            t.Title = title;
            t.Performers = performers;
            t.AlbumArtists = albumArtists;
            t.Composers = composers;
            t.Album = album;
            t.Comment = comment;
            t.Genres = genres;
            t.Year = year;
            t.Track = track;
            t.TrackCount = trackCount;
            t.Disc = disc;
            t.DiscCount = discCount;
            t.Lyrics = lyrics;
            t.Grouping = grouping;
            t.BeatsPerMinute = beatsPerMinute;
            t.Conductor = conductor;
            t.Copyright = copyright;
            t.Pictures = pictures;

        }
        #endregion

        private static string FirstInGroup(string [] group)
		{
			return group == null || group.Length == 0 ?
				null : group [0];
		}
    }
}
