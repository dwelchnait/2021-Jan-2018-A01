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

//Grouping

//a) by a column            Key
//b) by multiple columns    Key.attribute
//c) by an entity           Key.attribute

//groups have 2 components
//a) key component (group by); to reference this component use Key[.attribute]
//b) data (instances in the group)

//process
//start with a "pile" of data
//specify the grouping
//result is smaller "piles" of data determined by the group

//grouping can be save temporarying into
//  datasets and the individual group dataset
//  can be reported on

//report albums by ReleaseYear
//order by
//var resultsorderby = from x in Albums
//					orderby x.ReleaseYear
//					select x;
//resultsorderby.Dump();

//group by ReleaseYear
var resultsgroupby = from x in Albums
					group x by x.ReleaseYear;
//resultsgroupby.Dump();					

//group by Artist name and album ReleaseYear
var resultsgroupbycolumns = from x in Albums
					group x by new{x.Artist.Name,x.ReleaseYear};
//resultsgroupbycolumns.Dump();	

//group tracks by their album
var resultsgroupbyentity = from x in Tracks
							group x by x.Album;
//resultsgroupbyentity.Dump();

//IMPORTANT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//if you wish to report on groups (after the group by)
//   you MUST save the grouping in a temporary dataset
//   then you MUST use the temporary dataset to report from

//group by ReleaseYear
var resultsgroupbyReport = from x in Albums
					group x by x.ReleaseYear into gAlbumYear
					select new
					{
						KeyValue = gAlbumYear.Key,
						numberofAlbums = gAlbumYear.Count(),
						albumandartist = from y in gAlbumYear
										 select new
										 {
										 	Title = y.Title,
											Artist = y.Artist.Name
										 }
					};
//resultsgroupbyReport.Dump();		

//group by an entity
var groupAlbumsbyArtist = from x in Albums
							where x.ReleaseYear > 1969 && x.ReleaseYear < 1980
							group x by x.Artist into gArtistAlbums
							orderby gArtistAlbums.Key.Name
							where gArtistAlbums.Count() > 1
							select new
							{
								Artist = gArtistAlbums.Key.Name,
								numberofAlbums=gArtistAlbums.Count(),
								AlbumList = gArtistAlbums
											.Select(y => new
														{
														 Title = y.Title,
														 Year = y.ReleaseYear
														})
								
							};
groupAlbumsbyArtist.Dump();

//Create a query which will report the employee and their customer base.
//List the employee fullname (phone), number of customers in their base.
//list the fullname, city and state for the customer base.

//how to attack this question
//tips:
//What is the detail of the query? What is report most on?
//			Customers (big pile of data)
//Is this report one large order report OR many smaller reports
//          orderby  VS grouping
//Can I subdivide (group) my details into specific piles? If so, on what?
//			Employee  (smaller piles of data)
//Is there an association between Customers and Employees?
//          nav property SupportRep
var groupCustomersOfEmployees = from x in Customers
								group x by x.SupportRep into gTemp
								select new
								{
									Employee = gTemp.Key.LastName + ", " +
												gTemp.Key.FirstName + "(" +
												gTemp.Key.Phone + ")",
									BaseCount = gTemp.Count(),
									CustomerList = from y in gTemp
													select new
													{
													  CustName = y.LastName + ", " +
													  	         y.FirstName,
													  City = y.City,
													  State = y.State
													}
								};
//groupCustomersOfEmployees.Dump();								
































