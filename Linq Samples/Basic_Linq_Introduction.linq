<Query Kind="Expression">
  <Connection>
    <ID>5707cc56-27a8-41c4-98de-eba6baecf5bb</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

////method syntax (code as objects)
//Albums
//.Select(x => x)

////query syntax
//// the placehoder "currentrow" represents any individual row
////		on your table at any point in time during processing
//// "currentrow" can be any name you wish
//from currentrow in Albums
//select currentrow

////partial table rows
////in this example we will create a new output instance layout
////the default layout is the specified receiving field names and order
//// the data that fills the new output instance comes from the current row of the table
////query syntax
//from x in Albums
//select new
//{
//	Title = x.Title,
//	Year = x.ReleaseYear
//}

////method syntax
//Albums
//   .Select (x =>  new  
//         {
//            Title = x.Title, 
//            Year = x.ReleaseYear
//         }
//   		)

////where clause
////is used for filtering your selections
////query syntax
////select only albums in the year 1990
//from x in Albums
//where x.ReleaseYear == 1990
//select x
//
////method syntax
//Albums
//   .Where (x => (x.ReleaseYear == 1990))
//   .Select(x => x)

//orderby clause
//ascending and/or descending
//query syntax
//from x in Albums
//orderby x.ReleaseYear ascending, x.Title descending
//where x.ReleaseYear >= 1990 && x.ReleaseYear < 2000
//select x

//method syntax
//Albums
//	.OrderBy(x => x.ReleaseYear)
//	.ThenByDescending(x => x.Title)
//	.Where(x => x.ReleaseYear >= 1990 && x.ReleaseYear < 2000)
//	.Select(x => x)


////Create a list of albums showing the Album title, artist name and release year for
//// the good old 70's. Order alphabetically by artist then title.
//
////query
//from x in Albums
//where x.ReleaseYear < 1980 && x.ReleaseYear >= 1970
//orderby x.Artist.Name, x.Title
//select new
//{
//	Artist = x.Artist.Name,
//	Title = x.Title,
//	Year = x.ReleaseYear
//}

////method
//Albums
//	.Where(x => x.ReleaseYear >= 1970 && x.ReleaseYear < 1980)
//	.OrderBy(x => x.Artist.Name)
//	.ThenBy(x => x.Title)
//	.Select(x => new 
//			{
//				Artist = x.Artist.Name, 
//				Title = x.Title, 
//				Year = x.ReleaseYear
//			})


















