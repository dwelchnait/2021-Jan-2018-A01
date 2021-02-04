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
//Nested queries
//sometimes referred to as subqueries
//
//simply put: it is  a query within a query

//List all sales support employees showing their fullname (lastname, firstname)
//their title and the number of customers each supports. Order by fullname.
//In addition show a list of the customers for each employee.

//there will be 2 separate list within the same dataset collection
//one for employees
//one for customers of an employee

//C# point of view in a class definition
//classname
//   property1 (field)
//   property2 (field)
//    ...
//   collection<T> set of records

//to accomplish the list of customers, we will use a nested query
//the data source for the list of customers will be the x.collection<Customers>
//x.NavCollectionName
//x represents the current record on the outer query
//.NavCollectionName represents all the "children" of x
//var resultsq = from x in Employees
//			  where x.Title.Contains("Sales Support")
//			  orderby x.LastName, x.FirstName
//			  select new EmployeeCustomerList
//			  {
//			  	EmployeeName = x.LastName + ", " + x.FirstName,
//				Title = x.Title,
//				CustomersSupportCount = x.SupportRepCustomers.Count(),
//				CustomerList = (from y in x.SupportRepCustomers
//							   select new CustomerSupportItem
//							   {
//							   	  CustomerName = y.LastName + ", " + y.FirstName,
//								  Phone = y.Phone,
//								  City = y.City,
//								  State = y.State
//							   }).ToList()
//			  };
//resultsq.Dump();
//
//var resultsm =Employees
//		   .Where (x => x.Title.Contains ("Sales Support"))
//		   .OrderBy (x => x.LastName)
//		   .ThenBy (x => x.FirstName)
//		   .Select (
//		      x => 
//		         new  
//		         {
//		            EmployeeName = ((x.LastName + ", ") + x.FirstName), 
//		            Title = x.Title, 
//		            CustomersSupportCount = x.SupportRepCustomers.Count (), 
//		            CustomerList = x.SupportRepCustomers
//					               .Select (
//					                  y => 
//					                     new  
//					                     {
//					                        CustomerName = ((y.LastName + ", ") + y.FirstName), 
//					                        Phone = y.Phone, 
//					                        City = y.City, 
//					                        State = y.State
//					                     })
//					   				.ToList()
//		         }
//		   );
//resultsm.Dump();

//Create alist of albums showing its title and artist.
//Show albums with 25 or more tracks only.
//Show the songs on the album listing the name and song length.

var results2q = from x in Albums
				where x.Tracks.Count() >= 25
				select new 
				{
					Title = x.Title,
					Artist = x.Artist.Name,
					TracksOfAlbum = from y in x.Tracks
									select new
									{
										Song = y.Name,
										LengthofSong = y.Milliseconds / 1000.0
									}
					
				};
results2q.Dump();
}


//Define other methods and classes here

public class EmployeeCustomerList
{
	public string EmployeeName{get;set;}
	public string Title{get;set;}
	public int CustomersSupportCount{get;set;}
	public IEnumerable<CustomerSupportItem> CustomerList {get;set;}
}

public class CustomerSupportItem
{
	public string CustomerName{get;set;}
	public string Phone{get;set;}
	public string City{get;set;}
	public string State{get;set;}
}









