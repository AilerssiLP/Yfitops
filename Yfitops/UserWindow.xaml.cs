using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Yfitops.Data;
using Yfitops.Models;
using Yfitops.Services;

namespace Yfitops;

public partial class UserWindow : Window
{
    private readonly IMusicianData _musicianData;
    private readonly IAlbumData _albumData;
    private readonly ISongData _songData;
    private readonly UserService _userService;

    public UserWindow()
    {
        InitializeComponent();
        _userService = App.Services.GetRequiredService<UserService>();
        _musicianData = App.Services.GetRequiredService<IMusicianData>();
        _albumData = App.Services.GetRequiredService<IAlbumData>();
        _songData = App.Services.GetRequiredService<ISongData>();

        LoadMusicians();
    }

    private async void LoadMusicians()
    {
        var musicians = await _musicianData.GetAllAsync();
        MusicianList.ItemsSource = musicians;
    }

    private async void MusicianList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (MusicianList.SelectedItem is not Musician musician)
            return;

        var albums = await _albumData.GetByMusicianAsync(musician.Id);
        AlbumList.ItemsSource = albums;

        SongList.ItemsSource = null; 
    }

    private async void AlbumList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (AlbumList.SelectedItem is not Album album)
            return;

        var songs = await _songData.GetByAlbumAsync(album.Id);
        SongList.ItemsSource = songs;
    }
    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        LogService.Write($"User '{_userService.CurrentUser?.Username}' logged out.");
        new LoginWindow().Show();
        Close();
    }

}
