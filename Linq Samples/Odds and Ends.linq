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

var distinctmq = Customers
				 .Select(x => x.Country)
				 .Distinct();
//distinctmq.Dump();
 
var distinctsq = (from x in Customers
					select x.Country).Distinct();
//distinctsq.Dump();

//Any() and All()
//Show Genres that have tracks which are not on any playlist
var genreTrackAny = from g in Genres
				 where g.Tracks.Any(tr => tr.PlaylistTracks.Count() == 0)
				 select g;
//genreTrackAny.Dump();

var numberofgenres = Genres.Count();
//numberofgenres.Dump();

//show Genres that have all their tracks appearing at least once on a playlist

var genreTrackAll = from g in Genres
				 where g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0)
				 orderby g.Name
				 select new
				 {
				 	name = g.Name,
					thetracks = from y in g.Tracks
								where y.PlaylistTracks.Count() > 0
								select new
								{
									song = y.Name,
									count = y.PlaylistTracks.Count()
								}
				 };
//genreTrackAll.Dump();

//comparing the playlists of Roberto Almeida (AlmeidaR) and Michelle Brooks (BrooksM)
//comparing two lists to each other

//obtain a distinct list of all playlist tracks for Roberto Almeida
//the .Distinct() can destroy the sort of a query syntax, thus add the
//		sort after the .Distinct()
var almeida = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("AlmeidaR")
				select new
				{
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					song = x.Track.Name,
					artist = x.Track.Album.Artist.Name
				}).Distinct().OrderBy(y => y.song);
//almeida.Dump(); //110

var brooks = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("BrooksM")
				select new
				{
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					song = x.Track.Name,
					artist = x.Track.Album.Artist.Name
				}).Distinct().OrderBy(y => y.song);
//brooks.Dump();  //88

//list the tracks that both Roberto and Michelle like
//comparing 2 datasets together
//lista (Roberto) compared to listb (Michelle)

var likes = almeida
			.Where(a => brooks.Any(b => b.id == a.id))
			.OrderBy(a => a.genre)
			.ThenBy(a => a.song)
			.Select(a => a);
//likes.Dump();

//list the tracks that Roberto likes but Michelle does not listen to
var almeidaDiff = almeida
					.Where(a => !brooks.Any(b => b.id == a.id))
					.OrderBy(a => a.song)
					.Select(a => a);
//almeidaDiff.Dump();

//list the tracks that Michelle likes but Roberta does not listen to
var brooksDiff = brooks
					.Where(a => !almeida.Any(b => b.id == a.id))
					.OrderBy(a => a.song)
					.Select(a => a);
//brooksDiff.Dump();

//using multiple statements to solve a problem
//what is the problem
//	you have to do some type of pre processing where the solution
//  is used the remaining processing

//produce a report (display) where the track is flag as shorter than average,
// longer than average or average in play length (milliseconds)


//pre-processing to obtain a value need for the next query.
var resultsavg = Tracks.Average(tr => tr.Milliseconds);
//resultsavg.Dump();

//uses a value created earlier in another query
var resultsTrackAvgLength = (from x in Tracks
							select new
							{
								song = x.Name,
								milliseconds = x.Milliseconds,
								length = x.Milliseconds < resultsavg ? "Shorter" :
								         x.Milliseconds > Tracks.Average(tr => tr.Milliseconds) ? "Longer" :
										 "Average"
							}).OrderBy(x => x.length);
//resultsTrackAvgLength.Dump();

//Union
//the join of mutliple results into a single query dataset
//syntax (query).union(query).union(query) ...
//rules same as sql
//number of columns must be the same
//datatype of column must be the same
//Ordering should be done as a method on the unioned dataset

//list the stats of Albums on Tracks (Count, $cost, average track length)
//Note: for cost and average, one will need an instance to actually process
//      if an album contains no tracks then no Sum or Average will be allow

var unionresults = (from x in Albums
					where x.Tracks.Count() > 0
					select new
					{
						title = x.Title,
						TotalTracks = x.Tracks.Count(),
						TotalPrice = x.Tracks.Sum(tr => tr.UnitPrice),
						AverageLength = x.Tracks.Average(tr => tr.Milliseconds)/1000.0
					}).Union(from x in Albums
							where x.Tracks.Count() == 0
							select new
							{
								title = x.Title,
								TotalTracks = x.Tracks.Count(),
								TotalPrice = 0.00m,
								AverageLength = 0.0
							}).OrderBy(u => u.TotalTracks);
unionresults.Dump();




