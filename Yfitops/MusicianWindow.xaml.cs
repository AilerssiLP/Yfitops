using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Yfitops.Data;
using Yfitops.Models;
using Yfitops.Services;

namespace Yfitops;

public partial class MusicianWindow : Window
{
    private readonly UserService _userService;
    private readonly IMusicianData _musicianData;
    private readonly IAlbumData _albumData;
    private readonly ISongData _songData;

    private Musician? _musician;

    public MusicianWindow()
    {
        InitializeComponent();

        _userService = App.Services.GetRequiredService<UserService>();
        _musicianData = App.Services.GetRequiredService<IMusicianData>();
        _albumData = App.Services.GetRequiredService<IAlbumData>();
        _songData = App.Services.GetRequiredService<ISongData>();

        LoadMusicianProfile();
        LoadAlbums();   // <-- Missing before
    }

    private async void LoadMusicianProfile()
    {
        _musician = await _musicianData.GetByUserIdAsync(_userService.CurrentUser!.Id);

        if (_musician == null)
        {
            string stageName = Microsoft.VisualBasic.Interaction.InputBox("Enter Stage Name:", "Stage Name");

            _musician = new Musician
            {
                UserId = _userService.CurrentUser!.Id,
                StageName = stageName
            };

            await _musicianData.AddAsync(_musician);
        }
    }

    private async void LoadAlbums()
    {
        if (_musician == null) return;

        var albums = await _albumData.GetByMusicianAsync(_musician.Id);
        AlbumList.ItemsSource = albums;
        AlbumList.DisplayMemberPath = "Title";

        SongList.ItemsSource = null;
    }

    private async void AlbumList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (AlbumList.SelectedItem is not Album album) return;

        var songs = await _songData.GetByAlbumAsync(album.Id);
        SongList.ItemsSource = songs;
        SongList.DisplayMemberPath = "Title";
    }

    private async void CreateAlbum_Click(object sender, RoutedEventArgs e)
    {
        string name = Microsoft.VisualBasic.Interaction.InputBox("Album name:", "Create Album");

        await _albumData.AddAsync(new Album
        {
            MusicianId = _musician!.Id,
            Title = name
        });

        LoadAlbums();
    }

    private async void AddSong_Click(object sender, RoutedEventArgs e)
    {
        if (AlbumList.SelectedItem is not Album album)
        {
            MessageBox.Show("Select an album first.");
            return;
        }

        string song = Microsoft.VisualBasic.Interaction.InputBox("Song name:", "Add Song");

        await _songData.AddAsync(new Song
        {
            AlbumId = album.Id,
            Title = song
        });

        AlbumList_SelectionChanged(null, null);
    }

    private async void DeleteAlbum_Click(object sender, RoutedEventArgs e)
    {
        if (AlbumList.SelectedItem is not Album album)
        {
            MessageBox.Show("Select an album to delete.");
            return;
        }

        if (MessageBox.Show("Delete this album and all songs?", "Confirm", MessageBoxButton.YesNo)
            == MessageBoxResult.No)
            return;

        await _songData.DeleteByAlbumAsync(album.Id);
        await _albumData.DeleteAsync(album.Id);

        LoadAlbums();
    }

    private async void DeleteSong_Click(object sender, RoutedEventArgs e)
    {
        if (SongList.SelectedItem is not Song song)
        {
            MessageBox.Show("Select a song to delete.");
            return;
        }

        await _songData.DeleteAsync(song.Id);
        AlbumList_SelectionChanged(null, null);
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        new LoginWindow().Show();
        Close();
    }
}
