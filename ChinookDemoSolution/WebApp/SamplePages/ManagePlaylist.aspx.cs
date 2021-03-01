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
                    List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                    PlayList.DataSource = info;
                    PlayList.DataBind();
                },"Playlist Search","View the requested playlist below.");
            }

        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
 
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            //code to go here
            
        }

    }
}