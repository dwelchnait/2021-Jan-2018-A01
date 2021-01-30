<Query Kind="Program">
  <Connection>
    <ID>dafb55b9-c1f3-4196-b502-b211697f4c21</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	////List all customers in alphabetic order by last name, 
	////first name who live in the USA? List customer name.
	//
	// the {.....} of the new is referred to as the initializer list
	//
	//var country = "US";
	////query syntax
	//var results = from x in Customers
	//				where x.Country.Contains(country)
	//				orderby x.LastName, x.FirstName
	//				select new
	//				{
	//					FirstName = x.FirstName,
	//					LastName = x.LastName,
	//					fullName = x.LastName + ", " + x.FirstName
	//				
	//				};
	////we need to display the contents of results
	////in LinqPad use the application method .Dump()
	//results.Dump();
	//
	//
	//
	//
	////method syntax
	//results = Customers
	//			.Where(x => x.Country.Contains(country))
	//			.OrderBy(x => x.LastName)
	//			.ThenBy(x => x.FirstName)
	//			.Select(x => new
	//					{
	//						FirstName = x.FirstName,
	//						LastName = x.LastName,
	//						fullName = x.LastName + ", " + x.FirstName
	//					
	//					});
	//results.Dump();
	//
	////create an alphabetic list of Albums by ReleaseLabel.
	////Show Title and ReleaseLabel.
	////Albums missing labels will be listed as "Unknown"
	//
	//var results3 = Albums
	//	.OrderBy(x => x.ReleaseLabel)
	//	.Select( x => new
	//				{
	//					Title = x.Title,
	//					Label = x.ReleaseLabel == null  ? "Unknown" : x.ReleaseLabel
	//				});
	//results3.Dump();
	
	//create an alphabetic list of Albums by decades
	// 70's, 80's , 90's and "others"
	//list title and decade
	
	var results4a = Albums
		.Select( x => new 
					{
						Title = x.Title,
						Year = x.ReleaseYear,
						Decade = x.ReleaseYear >= 1970 && x.ReleaseYear < 1980 ? "70's" :
								 x.ReleaseYear >= 1980 && x.ReleaseYear < 1990 ? "80's" :
								 x.ReleaseYear >= 1990 && x.ReleaseYear < 2000 ? "90's" : "others"
					});
	results4a.Dump();			
	
	var results4b = from x in Albums
					select new AlbumDecades
					{
						Title = x.Title,
						Year = x.ReleaseYear,
						Decade = x.ReleaseYear >= 1970 && x.ReleaseYear < 1980 ? "70's" :
								 x.ReleaseYear >= 1980 && x.ReleaseYear < 1990 ? "80's" :
								 x.ReleaseYear >= 1990 && x.ReleaseYear < 2000 ? "90's" : "others"
					};
	results4b.Dump();
}

// You can define other methods, fields, classes and namespaces here

//remember a class is a developer-defined datatype

public class AlbumDecades
{
	public string Title{get;set;}
	public int Year {get;set;}
	public string Decade{get;set;}
}














