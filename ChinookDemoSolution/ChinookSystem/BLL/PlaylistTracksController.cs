using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using ChinookSystem.DAL;
using System.ComponentModel;
using FreeCode.Exceptions;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        //class level variable that will hold multiple strings representing
        //   any number of error messages
        List<Exception> brokenRules = new List<Exception>();

        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookSystemContext())
            {
                var results = from x in context.PlaylistTracks
                              where x.Playlist.Name.Equals(playlistname) &&
                                    x.Playlist.UserName.Equals(username)
                              orderby x.TrackNumber
                              select new UserPlaylistTrack
                              {
                                  TrackID = x.TrackId,
                                  TrackNumber = x.TrackNumber,
                                  TrackName = x.Track.Name,
                                  Milliseconds = x.Track.Milliseconds,
                                  UnitPrice = x.Track.UnitPrice
                              };
                

                return results.ToList();
            }
        }//eom
        
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            Playlist playlistExists = null;
            PlaylistTrack playlisttrackExists = null;
            int tracknumber = 0;
            using (var context = new ChinookSystemContext())
            {
                //This class is in what is called the Business Logic Layer
                //Business Logic is the rules of your business
                //  rule: a track can only exist once on a playlist
                //  rule: each track on a playlist is assigned a continious
                //        track number
                //
                //The BLL method should also ensure that data exists for
                //   the processing of the transaction
                if(string.IsNullOrEmpty(playlistname))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to add track", nameof(playlistname), playlistname));
                }
                else if (string.IsNullOrEmpty(username))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("User name was not supplied", nameof(username), username));
                }
                else
                {
                    //does the playlist exist?
                    playlistExists = (from x in context.Playlists
                                      where (x.Name.Equals(playlistname)
                                          && x.UserName.Equals(username))
                                      select x).FirstOrDefault();
                    if (playlistExists == null)
                    {
                        //the playlist DOES NOT exists
                        //tasks:
                        //      create a new instance of a playlist object
                        //      load the instance with data
                        //      stage the add of the new instance
                        //      set a variable representing the tracknumber to 1
                        playlistExists = new Playlist()
                                        {
                                            Name = playlistname,
                                            UserName = username
                                        };
                        context.Playlists.Add(playlistExists);
                        tracknumber = 1;
                    }
                    else
                    {
                        //the playlist already exists
                        //verify track not already on playlist (business rule)
                        //what is the next tracknumber
                        //add 1 to the tracknumber
                        playlisttrackExists = (from x in context.PlaylistTracks
                                               where x.Playlist.Name.Equals(playlistname)
                                                 && x.Playlist.UserName.Equals(username)
                                                 && x.TrackId == trackid
                                               select x).FirstOrDefault();
                        if (playlisttrackExists == null)
                        {
                            tracknumber = (from x in context.PlaylistTracks
                                           where x.Playlist.Name.Equals(playlistname)
                                             && x.Playlist.UserName.Equals(username)
                                           select x.TrackNumber).Max();
                            tracknumber++;
                        }
                        else
                        {
                            brokenRules.Add(new BusinessRuleException<string>("Track already on playlist.", nameof(playlisttrackExists.Track.Name), playlisttrackExists.Track.Name));
                        }
                    }

                    //create/load/ stage the adding of the track
                }
                
             
            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookSystemContext())
            {
               //code to go here


            }
        }//eom
    }
}
