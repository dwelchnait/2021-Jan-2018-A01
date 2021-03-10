using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;

#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        #region Error Handling
        protected void SelectCheckForException(object sender,
               ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }
        protected void InsertCheckForException(object sender,
              ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process Success", "Album has been added");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }

        }
        protected void UpdateCheckForException(object sender,
             ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process Success", "Album has been updated");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }

        }
        protected void DeleteCheckForException(object sender,
             ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process Success", "Album has been removed");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }

        }
        #endregion

        protected void ArtistFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Artist";
            if (string.IsNullOrEmpty(ArtistName.Text))
            {
                MessageUserControl.ShowInfo("Artist Search", "No artist name was supplied");
            }

            //HiddentField, though text fields, are accessed by the property Value, NOT Text
            SearchArg.Value = ArtistName.Text;

            //to cause an ODS to re-execute, you only need to do a .DataBind() against the display control
            TracksSelectionList.DataBind();

          }


        protected void GenreFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Genre";
            //there is no prompt test needed for this event BECAUSE the dropdownlist
            //    does not have a prompt
            //by default there will always be a value to use

            //HiddentField, though text fields, are accessed by the property Value, NOT Text
            SearchArg.Value = GenreDDL.SelectedValue.ToString(); //as an integer
            //SearchArg.Value = GenreDDL.SelectedItem.Text; //as a string

            //to cause an ODS to re-execute, you only need to do a .DataBind() against the display control
            TracksSelectionList.DataBind();

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Album";
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                MessageUserControl.ShowInfo("Album Search", "No album title was supplied");
            }

            //HiddentField, though text fields, are accessed by the property Value, NOT Text
            SearchArg.Value = AlbumTitle.Text;

            //to cause an ODS to re-execute, you only need to do a .DataBind() against the display control
            TracksSelectionList.DataBind();
        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //username is coming from the system via security
            //since security has yet to be installed, a default will be setup for the
            //    username value
            string username = "HansenB";
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Playlist Search", "No palylist name was supplied");
            }
            else
            {
                //use some user friendly error handling
                //the way we are doing the error is using MessageUserControl instead of
                //     using try/catch
                //MessageUserControl has the try/catch embedded within the control
                //within the MessageUserControl there exists a method called .TryRun()
                //syntax
                //    MessageUserControl.TryRun(() =>{
                //
                //      coding block
                //
                //    }[,"message title","success message"]);
                MessageUserControl.TryRun(() =>
                {
                    //code to execute under error handling control of MessageUserControl
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    RefreshPlaylist(sysmgr, username);
                },"Playlist Search","View the requested playlist below.");
            }

        }
        protected void RefreshPlaylist(PlaylistTracksController sysmgr,string username)
        {
            List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
            PlayList.DataSource = info;
            PlayList.DataBind();
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Track Movement", "You must have a play list name");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Movement", "You must have a play list showing");
                }
                else
                {
                    //was anything actually selected
                    CheckBox songSelected = null;
                    int rowsSelected = 0;
                    MoveTrackItem moveTrack = new MoveTrackItem();
                   
                    //traverse the gridview control PlayList
                    //you could do this same code using a foreach()
                    for (int i = 0; i < PlayList.Rows.Count; i++)
                    {
                        //point to the checkbox control on the gridview row
                        songSelected = PlayList.Rows[i].FindControl("Selected") as CheckBox;
                        //test the setting of the checkbox
                        if (songSelected.Checked)
                        {
                            rowsSelected++;
                            moveTrack.TrackId = int.Parse((PlayList.Rows[i].FindControl("TrackID") as Label).Text);
                            moveTrack.TrackNumber = int.Parse((PlayList.Rows[i].FindControl("TrackNumber") as Label).Text);
                        }
                    }
                    //processing rule: ONLY ONE row may be moved
                    switch (rowsSelected)
                    {
                        case 0:
                            {
                                MessageUserControl.ShowInfo("Track Movement", "No song selected. You must select a single song to move.");
                                break;
                            }
                        case 1:
                            {
                                //is it the bottom row
                                if(moveTrack.TrackNumber == PlayList.Rows.Count)
                                {
                                    MessageUserControl.ShowInfo("Track Movement", "Song is the last song on the list. No movement necessary.");
                                }
                                else
                                {
                                    //move the track
                                    moveTrack.Direction = "down";
                                    MoveTrack(moveTrack);
                                }
                                break;
                            }
                        default:
                            {
                                MessageUserControl.ShowInfo("Track Movement", "You must select only a single song to move.");
                                break;
                            }
                    }
                }
            }
         }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Track Movement", "You must have a play list name");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Movement", "You must have a play list showing");
                }
                else
                {
                    //was anything actually selected
                    CheckBox songSelected = null;
                    int rowsSelected = 0;
                    MoveTrackItem moveTrack = new MoveTrackItem();

                    //traverse the gridview control PlayList
                    //you could do this same code using a foreach()
                    for (int i = 0; i < PlayList.Rows.Count; i++)
                    {
                        //point to the checkbox control on the gridview row
                        songSelected = PlayList.Rows[i].FindControl("Selected") as CheckBox;
                        //test the setting of the checkbox
                        if (songSelected.Checked)
                        {
                            rowsSelected++;
                            moveTrack.TrackId = int.Parse((PlayList.Rows[i].FindControl("TrackID") as Label).Text);
                            moveTrack.TrackNumber = int.Parse((PlayList.Rows[i].FindControl("TrackNumber") as Label).Text);
                        }
                    }
                    //processing rule: ONLY ONE row may be moved
                    switch (rowsSelected)
                    {
                        case 0:
                            {
                                MessageUserControl.ShowInfo("Track Movement", "No song selected. You must select a single song to move.");
                                break;
                            }
                        case 1:
                            {
                                //is it the top row
                                if (moveTrack.TrackNumber == 1)
                                {
                                    MessageUserControl.ShowInfo("Track Movement", "Song is the first song on the list. No movement necessary.");
                                }
                                else
                                {
                                    //move the track
                                    
                                    moveTrack.Direction = "up";
                                    MoveTrack(moveTrack);
                                }
                                break;
                            }
                        default:
                            {
                                MessageUserControl.ShowInfo("Track Movement", "You must select only a single song to move.");
                                break;
                            }
                    }
                }
            }

        }

        protected void MoveTrack(MoveTrackItem moveTrack)
        {
            string username = "HansenB";
            moveTrack.PlaylistName = PlaylistName.Text;
            moveTrack.UserName = username;
            MessageUserControl.TryRun(() =>
            {
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                sysmgr.MoveTrack(moveTrack);
                RefreshPlaylist(sysmgr, username);
            }, "Track Movement", "Selected track(s) has been moved on the playlist.");
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            string username = "HansenB";
            if(string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Track Removal", "You must have a playlist name");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Removal", "You must have a play list visible to choose removals. Select from the display playlist.");
                }
                else
                {
                    //collect the tracks indicated on the playlist for removal
                    List<int> trackids = new List<int>();
                    int rowsSelected = 0;
                    CheckBox trackSelection = null;
                    //traverse the gridview control PlayList
                    //you could do this same code using a foreach()
                    for(int i = 0; i < PlayList.Rows.Count; i++)
                    {
                        //point to the checkbox control on the gridview row
                        trackSelection = PlayList.Rows[i].FindControl("Selected") as CheckBox;
                        //test the setting of the checkbox
                        if (trackSelection.Checked)
                        {
                            rowsSelected++;
                            trackids.Add(int.Parse((PlayList.Rows[i].FindControl("TrackId") as Label).Text));
                        }
                    }

                    //was a song selected
                    if (rowsSelected == 0)
                    {
                        MessageUserControl.ShowInfo("Track Removal", "You must select at least one song to remove..");
                    }
                    else
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            PlaylistTracksController sysmgr = new PlaylistTracksController();
                            sysmgr.DeleteTracks(username, PlaylistName.Text, trackids);
                            RefreshPlaylist(sysmgr, username);
                        },"Track removal","Selected track(s) has been removed from the playlist.");
                    }
                }
            }
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            string username = "HansenB";
            //validate playlist exists
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing Data", "Enter a playlist name");
            }
            else
            {
                //grab a value from the selected ListView row
                //the row is referred to as e.Item
                //to access the column use the .FindControl("xx") as crtlType).crtlaccess
                string song = (e.Item.FindControl("NameLabel") as Label).Text;

                //Reminder: MessageUserControl will do the error handling
                MessageUserControl.TryRun(() => {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username,
                        int.Parse(e.CommandArgument.ToString()), song);
                    RefreshPlaylist(sysmgr, username);
                },"Add Track to Playlist","Track has been added to the playlist");
            }
            
        }

    }
}