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
        
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid,
            string song)
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
                 if (string.IsNullOrEmpty(username))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("User name was not supplied", nameof(username), username));
                }

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
                    context.Playlists.Add(playlistExists); //stage ONLY!!!!!!!!!!
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
                        brokenRules.Add(new BusinessRuleException<string>("Track already on playlist.", nameof(song), song));
                    }
                }

                //create the playlist track
                playlisttrackExists = new PlaylistTrack();

                //load of the playlist track
                playlisttrackExists.TrackId = trackid;
                playlisttrackExists.TrackNumber = tracknumber;

                //??????
                //what is the playlist id
                //if the playlist exists then we know the id
                //BUT if the playlist is new, we DO NOT know the id

                //in one case the id is known BUT in the second case
                //    where the new record is ONLY STAGED, NO primary key
                //    value has been generated yet.
                //if you access the new playlist record the pkey would be 0 (default numeric)

                //the solution to BOTH of these scenarios is to use
                //    navigational properties during the ACTUAL .Add command
                //    for the new playlisttrack record
                //the entityframework will, on your behave, ensure that the adding
                //      of records to the database will be done in the appropriate
                //      order AND will add the missing compound primary key value
                //      (PlaylistId) to the new playlisttrack record

                //NOT LIKE this!!!! THIS IS WRONG!!!!!
                //context.PlaylistTracks.Add(playlisttrackExists);

                //INSTEAD, do the staging using the parent.navproperty.Add(xxxx)
                playlistExists.PlaylistTracks.Add(playlisttrackExists);

                //time to commit to sql
                //check: are there any errors in this transaction
                //brokenRules is a List<Exceptions>
                if (brokenRules.Count > 0)
                {
                    //at least one error was recorded during the processing of the transaction
                    throw new BusinessRuleCollectionException("Add Playlist Track Concerns:", brokenRules);
                }
                else
                {
                    //COMMIT THE TRANSACTION
                    //the ALL the staged records to sql for processing
                    context.SaveChanges();
                }
            }//eou
        }//eom
       
        public void MoveTrack(MoveTrackItem moveTrack)
        {
            using (var context = new ChinookSystemContext())
            {
                if (string.IsNullOrEmpty(moveTrack.PlaylistName))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to add track", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                }
                if (string.IsNullOrEmpty(moveTrack.UserName))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("User name was not supplied", nameof(MoveTrackItem.PlaylistName), moveTrack.UserName));
                }
                if (moveTrack.TrackId <= 0)
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<int>("Invalid track identifier was supplied", nameof(MoveTrackItem.TrackId), moveTrack.TrackId));
                }
                if (moveTrack.TrackNumber <= 0)
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<int>("Invalid track number was supplied", nameof(MoveTrackItem.TrackNumber), moveTrack.TrackNumber));
                }
                Playlist exists = (from x in context.Playlists
                                   where x.Name.Equals(moveTrack.PlaylistName)
                                      && x.UserName.Equals(moveTrack.UserName)
                                   select x).FirstOrDefault();
                if (exists == null)
                {
                    brokenRules.Add(new BusinessRuleException<string>("Playlist does not exist", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                }
                else
                {
                    PlaylistTrack trackexists = (from x in context.PlaylistTracks
                                       where x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                          && x.Playlist.UserName.Equals(moveTrack.UserName)
                                          && x.TrackId == moveTrack.TrackId
                                       select x).FirstOrDefault();
                    if (trackexists == null)
                    {
                        brokenRules.Add(new BusinessRuleException<string>("Track does not exist on the play. Refresh your playlist display.", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                    }
                    else
                    {
                        //decide the logic depending on direction
                        if (moveTrack.Direction.Equals("up"))
                        {
                            //move up
                            //business process check: already at the top
                            if (trackexists.TrackNumber == 1)
                            {
                                brokenRules.Add(new BusinessRuleException<string>("Track already to the top. Refresh your playlist display.", nameof(Track.Name), trackexists.Track.Name));
                            }
                            else
                            {
                                //do the move
                                //get the other track
                                PlaylistTrack othertrack = (from x in context.PlaylistTracks
                                                             where x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                                                && x.Playlist.UserName.Equals(moveTrack.UserName)
                                                                && x.TrackNumber ==
                                                                trackexists.TrackNumber - 1
                                                             select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    brokenRules.Add(new BusinessRuleException<string>("Track to swap seems to be missing. Refresh your playlist display.", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                                }
                                else
                                {
                                    //change the tracknumber
                                    trackexists.TrackNumber -= 1;
                                    othertrack.TrackNumber += 1;
                                    //stage
                                    context.Entry(trackexists).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                    context.Entry(othertrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                }
                            }
                        }
                        else
                        {
                            //move down
                            //business process check: already at the bottom
                            if (trackexists.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                brokenRules.Add(new BusinessRuleException<string>("Track already to the bottom. Refresh your playlist display.", nameof(Track.Name), trackexists.Track.Name));
                            }
                            else
                            {
                                //do the move
                                //get the other track
                                PlaylistTrack othertrack = (from x in context.PlaylistTracks
                                                            where x.Playlist.Name.Equals(moveTrack.PlaylistName)
                                                               && x.Playlist.UserName.Equals(moveTrack.UserName)
                                                               && x.TrackNumber ==
                                                               trackexists.TrackNumber + 1
                                                            select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    brokenRules.Add(new BusinessRuleException<string>("Track to swap seems to be missing. Refresh your playlist display.", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                                }
                                else
                                {
                                    //change the tracknumber
                                    trackexists.TrackNumber += 1;
                                    othertrack.TrackNumber -= 1;
                                    //stage
                                    context.Entry(trackexists).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                    context.Entry(othertrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                }
                            }
                        }
                    }
                }
                //commit
                if (brokenRules.Count > 0)
                {
                    throw new BusinessRuleCollectionException("Track Movement", brokenRules);
                }
                else
                {
                    context.SaveChanges();
                }
            }//eou
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {

            using (var context = new ChinookSystemContext())
            {
                if (string.IsNullOrEmpty(playlistname))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to add track", nameof(playlistname), playlistname));
                }
                if (string.IsNullOrEmpty(username))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("User name was not supplied", nameof(username), username));
                }
                if (trackstodelete.Count == 0)
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<int>("You did not select any tracks to delete", "track count", 0));
                }
                Playlist exists = (from x in context.Playlists
                                   where x.Name.Equals(playlistname)
                                      && x.UserName.Equals(username)
                                   select x).FirstOrDefault();
                if (exists == null)
                {
                    brokenRules.Add(new BusinessRuleException<string>("Playlist does not exist", nameof(playlistname), playlistname));
                }
                else
                {
                    //list of all track that are to be kept
                    var trackskept = context.PlaylistTracks
                                      .Where(tr => tr.Playlist.Name.Equals(playlistname)
                                                && tr.Playlist.UserName.Equals(username)
                                                && !trackstodelete.Any(tod => tod == tr.TrackId))
                                      .OrderBy(tr => tr.TrackNumber)
                                      .Select(tr => tr);


                    //remove the desired tracks
                    // 11, 235, 34, ...
                    PlaylistTrack item = null;
                    foreach (int deletetrackid in trackstodelete)
                    {
                        item = context.PlaylistTracks
                                      .Where(tr => tr.Playlist.Name.Equals(playlistname)
                                                && tr.Playlist.UserName.Equals(username)
                                                && tr.TrackId == deletetrackid)
                                      .Select(tr => tr).FirstOrDefault();
                        if (item != null)
                        {
                            //staged
                            //parent.navproperty.Remove()
                            exists.PlaylistTracks.Remove(item);
                        }
                    }

                    //re-sequence the kept tracks
                    //option a) use a list and update the records of the list
                    //option b) delete all children records and re-add only the 
                    //              necessary kept records

                    //within this example, you will see how to update specific
                    //    column(s) of a record
                    int tracknumber = 1;
                    foreach(var track in trackskept)
                    {
                        track.TrackNumber = tracknumber;
                        context.Entry(track).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;  //Staged
                        tracknumber++;
                    }
                }
                //save the work
                if (brokenRules.Count > 0)
                {
                    throw new BusinessRuleCollectionException("Track Removal Concerns:", brokenRules);
                }
                else
                {
                    context.SaveChanges();
                }
            }//eou
        }//eom
    }
}
