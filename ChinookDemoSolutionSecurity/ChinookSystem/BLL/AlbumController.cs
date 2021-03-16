using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Entities;
using ChinookSystem.DAL;
using ChinookSystem.ViewModels;
using System.ComponentModel;    //is for ODS
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        #region Queries
        //due to the fact that the entities are internal
        //  you CAN NOT use the entity class as a return datatype
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<ArtistAlbums>  Albums_GetArtistAlbums()
        {
            using(var context = new ChinookSystemContext())
            {
                //Linq to Entity

                IEnumerable<ArtistAlbums> results = from x in context.Albums
                                                    select new ArtistAlbums
                                                    {
                                                        Title = x.Title,
                                                        ReleaseYear = x.ReleaseYear,
                                                        ArtistName = x.Artist.Name
                                                    };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AlbumDecades> Albums_ListAlbumsByDecade()
        {
            using (var context = new ChinookSystemContext())
            {
                //Linq to Entity

                IEnumerable<AlbumDecades> results = from x in context.Albums
                                                    select new AlbumDecades
                                                    {
                                                        Title = x.Title,
                                                        Year = x.ReleaseYear,
                                                        Decade = x.ReleaseYear >= 1970 && x.ReleaseYear < 1980 ? "70's" :
                                                                 x.ReleaseYear >= 1980 && x.ReleaseYear < 1990 ? "80's" :
                                                                 x.ReleaseYear >= 1990 && x.ReleaseYear < 2000 ? "90's" : "others"
                                                    };
                return results.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ArtistAlbums> Albums_GetAlbumsForArtist(int artistid)
        {
            using (var context = new ChinookSystemContext())
            {
                //Linq to Entity

                IEnumerable<ArtistAlbums> results = from x in context.Albums
                                                    where x.ArtistId == artistid
                                                    select new ArtistAlbums
                                                    {
                                                        Title = x.Title,
                                                        ReleaseYear = x.ReleaseYear,
                                                        ArtistName = x.Artist.Name,
                                                        ArtistId = x.ArtistId
                                                    };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<AlbumItem> Albums_List()
        {
            using (var context = new ChinookSystemContext())
            {
                IEnumerable<AlbumItem> results = from x in context.Albums
                                                    select new AlbumItem
                                                    { 
                                                        AlbumId = x.AlbumId,
                                                        Title = x.Title,
                                                        ArtistId = x.ArtistId,
                                                        ReleaseYear = x.ReleaseYear,
                                                        ReleaseLabel = x.ReleaseLabel
                                                    };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AlbumItem Albums_FindById(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                AlbumItem results = (from x in context.Albums
                                    where x.AlbumId == albumid
                                    select new AlbumItem
                                    {
                                        AlbumId = x.AlbumId,
                                        Title = x.Title,
                                        ArtistId = x.ArtistId,
                                        ReleaseYear = x.ReleaseYear,
                                        ReleaseLabel = x.ReleaseLabel
                                    }).FirstOrDefault();
                return results;
            }
        }
        #endregion

        #region Add, Update, Delete (CRUD)

        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public int Albums_Add(AlbumItem item)
        {
            using(var context = new ChinookSystemContext())
            {
                //need to move the data from the viewmodel class into
                //  an entity instance BEFORE staging

                //the pkey of the Albums table is an Identity() pKey
                //    therefore you do NOT need to supply the AlbumId value

                Album entityItem = new Album
                {
                    Title = item.Title,
                    ArtistId = item.ArtistId,
                    ReleaseYear = item.ReleaseYear,
                    ReleaseLabel = item.ReleaseLabel
                };

                //stagging is to local memory
                context.Albums.Add(entityItem);

                //At this point, the new pkey value is NOT available
                //the new pkey value is created by the database

                //commit is the action of sending your request to
                //    the database for action.
                //Also, any validation annotation in your entity definition class
                //    is execute during this command
                context.SaveChanges();
                //since I have an int as the return datatype
                //  I will return the new identity value
                return entityItem.AlbumId;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Albums_Update(AlbumItem item)
        {
            using (var context = new ChinookSystemContext())
            {
                //need to move the data from the viewmodel class into
                //  an entity instance BEFORE staging

                //on update you NEED to supply the table's pkey value
                Album entityItem = new Album
                {
                    AlbumId = item.AlbumId,
                    Title = item.Title,
                    ArtistId = item.ArtistId,
                    ReleaseYear = item.ReleaseYear,
                    ReleaseLabel = item.ReleaseLabel
                };

                //stagging is to local memory
                context.Entry(entityItem).State = System.Data.Entity.EntityState.Modified;
                //commit is the action of sending your request to
                //    the database for action.
                context.SaveChanges();
               
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public void Albums_Delete(AlbumItem item)
        {
            //this method will call the actual delete method and pass it
            //   the only need piece of data: pkey
            Albums_Delete(item.AlbumId);
        }

        public void Albums_Delete(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                var exists = context.Albums.Find(albumid);
                context.Albums.Remove(exists);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
