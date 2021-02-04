<Query Kind="Statements">
  <Connection>
    <ID>dafb55b9-c1f3-4196-b502-b211697f4c21</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Aggregrates
//.Count(), .Sum(), .Min(), .Max(), .Average()

//aggregrates operate on collections
var ex1 = Albums.Count();

//for aggregrates .Sum, .Min, Max, Average
//you need to specific a field to the aggregate

//How much room does the music collection on the
//  database take for albums of the 1990.

//Tracks table has a field called Bytes
//   this field holds the size of a track
//   summing the field of all tracks will get me
//      the total required disk space
//var ex2 = Tracks.Where(x => x.Album.ReleaseYear == 1990).Sum(x => x.Bytes);
var ex2 = (from x in Tracks
			where x.Album.ReleaseYear == 1990
			select x.Bytes).Sum();
var ex3 = Tracks.Where(x => x.Album.ReleaseYear == 1990).Min(x => x.Bytes);
var ex4 = Tracks.Where(x => x.Album.ReleaseYear == 1990).Max(x => x.Bytes);
var ex5 = Tracks.Where(x => x.Album.ReleaseYear == 1990).Average(x => x.Bytes);
ex2.Dump();
//ex3.Dump();
//ex4.Dump();
//ex5.Dump();

//List of all albums showing their title, artist name, and the number of
//tracks for that album. Show only albums of the 60's. Order by the number of
//tracks from most to least.
//
//var ex6qa = from x in Albums
//			where x.ReleaseYear > 1959 && x.ReleaseYear < 1970
//			orderby x.Tracks.Count() descending
//			select new
//			{
//				Title = x.Title,
//				Artist = x.Artist.Name,
//				Year = x.ReleaseYear,
//				NumberOfTracks = x.Tracks.Count()
//			};
//var ex6qb = from x in Albums
//			where x.ReleaseYear > 1959 && x.ReleaseYear < 1970
//			orderby x.Tracks.Count() descending
//			select new
//			{
//				Title = x.Title,
//				Artist = x.Artist.Name,
//				Year = x.ReleaseYear,
//				NumberOfTracks = (from y in Tracks
//								 where y.AlbumId == x.AlbumId
//								 select y).Count()
//				//NumberOfTracks = (from y in x.Tracks
//				//				 select y).Count()
//									
//			};
//var ex6ma = Albums
//   .Where (x => ((x.ReleaseYear > 1959) && (x.ReleaseYear < 1970)))
//   .Select (
//		      x => 
//		         new  
//		         {
//		            Title = x.Title, 
//		            Artist = x.Artist.Name, 
//		            Year = x.ReleaseYear, 
//		            NumberOfTracks = x.Tracks.Count()
//		         }
//   			)
//	.OrderByDescending (x => x.NumberOfTracks);
//var ex6mb = Albums
//   .Where (x => ((x.ReleaseYear > 1959) && (x.ReleaseYear < 1970)))
//	.OrderByDescending (x => x.Tracks.Count())
//   .Select (
//		      x => 
//		         new  
//		         {
//		            Title = x.Title, 
//		            Artist = x.Artist.Name, 
//		            Year = x.ReleaseYear, 
//		            NumberOfTracks = x.Tracks.Count()
//		         }
//   			);
//ex6qa.Dump();
//ex6qb.Dump();
//ex6ma.Dump();
//ex6ma.Dump();


//Produce a list of 60's albums which have tracks showing
//their title, artist, number of tracks on album,
//total price of all tracks on album, the longest album track,
//the shortest album track and the average track length.

var ex7 = from x in Albums
			where x.ReleaseYear > 1959 && x.ReleaseYear < 1970
			   && x.Tracks.Count() > 0
			select new
			{
				Title = x.Title,
				Artist = x.Artist.Name,
				numberoftracks = x.Tracks.Count(),
				methodcostoftracks = x.Tracks.Sum(tr => tr.UnitPrice),
				querycostoftracks = (from tr in x.Tracks
									  select tr.UnitPrice).Sum(),
				longest = x.Tracks.Max(tr => tr.Milliseconds),
				longestTrackSong = (from y in x.Tracks
								   where y.Milliseconds == x.Tracks.Max(tr => tr.Milliseconds)
								   select y.Name).FirstOrDefault(),
				shortestinseconds = x.Tracks.Min(tr => tr.Milliseconds / 1000.0),
				averagelength = x.Tracks.Average(tr => tr.Milliseconds)
			};
ex7.Dump();















