using Mp3Player.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3Player
{

    public partial class MainForm : Form
    {
        private List<TrackItem> TrackList = new List<TrackItem>();

        private void InitializeContextMenuListBox(object ender, EventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Add track's", new EventHandler(AddingSoundTrack));
            cm.MenuItems.Add("Clear", new EventHandler(ClearSoundTrackList));
            cm.MenuItems.Add("Sort", new EventHandler(SortSoundTrack));
            listBoxTracks.ContextMenu = cm;
        }


        private void InitializeWindowsMediaPlayer(object ender, EventArgs e)
        {
            axWMP.settings.autoStart = true;
            axWMP.Ctlenabled = true;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            InitializeContextMenuListBox(sender, e);
            ClearSoundTrackList(sender, e);
        }

        private void AddingSoundTrack(object sender, EventArgs e)
        {
            if (openTrackFileDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < openTrackFileDialog.SafeFileNames.Length; i++)
                {
                    TrackList.Add(item: new TrackItem(openTrackFileDialog.SafeFileNames[i], openTrackFileDialog.FileNames[i]));
                }

                ReloadListBoxTracks();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (axWMP.URL != null && axWMP.URL != TrackList[listBoxTracks.SelectedIndex].path)
            {
                axWMP.URL = TrackList[listBoxTracks.SelectedIndex].path;
            }

        }

        private void ReloadListBoxTracks()
        {
            listBoxTracks.Items.Clear();
            foreach (var i in TrackList)
            {
                listBoxTracks.Items.Add(i.name);
            }
        }

        private void ClearSoundTrackList(object ender, EventArgs e)
        {
            TrackList.Clear();
            listBoxTracks.Items.Clear();
            axWMP.URL = null;
        }
        private void SortSoundTrack(object ender, EventArgs e)
        {
            TrackList.Sort((TrackItem first, TrackItem second) => first.name.CompareTo(second.name));
            ReloadListBoxTracks();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                if (listBoxTracks.SelectedIndex < listBoxTracks.Items.Count)
                {
                    listBoxTracks.SelectedIndex += 1;
                    listBoxTracks.SelectedIndexChanged += listBox1_SelectedIndexChanged;
                    TimmerToStartPlay(100);
                }
            }
        }

        private async void TimmerToStartPlay(int time)
        {
            await Task.Run(() => Thread.Sleep(time));
            axWMP.Ctlcontrols.play();
        }
    }
}
