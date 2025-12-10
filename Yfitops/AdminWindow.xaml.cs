using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Yfitops.Data;
using Yfitops.Models;
using Yfitops.Services;

namespace Yfitops;

public partial class AdminWindow : Window
{
    private readonly IUserData _userData;
    private readonly IMusicianData _musicianData;
    private readonly IAlbumData _albumData;
    private readonly ISongData _songData;
    private readonly UserService _userService;

    public AdminWindow()
    {
        InitializeComponent();

        _userService = App.Services.GetRequiredService<UserService>();
        _userData = App.Services.GetRequiredService<IUserData>();
        _musicianData = App.Services.GetRequiredService<IMusicianData>();
        _albumData = App.Services.GetRequiredService<IAlbumData>();
        _songData = App.Services.GetRequiredService<ISongData>();

        LoadUsers();
        LoadMusicians();
    }

    private async void LoadUsers()
    {
        UserList.ItemsSource = await _userData.GetAllAsync();
    }

    private async void LoadMusicians()
    {
        MusicianList.ItemsSource = await _musicianData.GetAllAsync();
    }

    private async void MusicianList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (MusicianList.SelectedItem is not Musician m)
        {
            AlbumList.ItemsSource = null;
            SongList.ItemsSource = null;
            return;
        }

        AlbumList.ItemsSource = await _albumData.GetByMusicianAsync(m.Id);
        SongList.ItemsSource = null;
    }

    private async void AlbumList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (AlbumList.SelectedItem is not Album a)
        {
            SongList.ItemsSource = null;
            return;
        }

        SongList.ItemsSource = await _songData.GetByAlbumAsync(a.Id);
    }

    private async void DeleteUser_Click(object sender, RoutedEventArgs e)
    {
        if (UserList.SelectedItem is not User user)
        {
            MessageBox.Show("Select a user.");
            return;
        }

        if (user.Id == _userService.CurrentUser!.Id)
        {
            MessageBox.Show("You cannot delete your own admin account.");
            return;
        }

        if (MessageBox.Show("Delete this user and all related data?", "Confirm", MessageBoxButton.YesNo)
            == MessageBoxResult.No)
            return;

        await _userData.DeleteAsync(user.Id);
        LogService.Write($"Admin '{_userService.CurrentUser.Username}' deleted user '{user.Username}' (ID={user.Id}).");
        LoadUsers();
        LoadMusicians();
        AlbumList.ItemsSource = null;
        SongList.ItemsSource = null;
    }


    private async void DeleteAlbum_Click(object sender, RoutedEventArgs e)
    {
        if (AlbumList.SelectedItem is not Album album)
        {
            MessageBox.Show("Select album.");
            return;
        }

        await _songData.DeleteByAlbumAsync(album.Id);
        await _albumData.DeleteAsync(album.Id);
        MessageBox.Show("Album and its songs deleted.");

        LogService.Write($"Admin '{_userService.CurrentUser.Username}' deleted album '{album.Title}' (ID={album.Id}).");

        MusicianList_SelectionChanged(null, null);
    }

    private async void DeleteSong_Click(object sender, RoutedEventArgs e)
    {
        if (SongList.SelectedItem is not Song song)
        {
            MessageBox.Show("Select song.");
            return;
        }

        await _songData.DeleteAsync(song.Id);
        MessageBox.Show("Song deleted.");
        LogService.Write($"Admin '{_userService.CurrentUser.Username}' deleted song '{song.Title}' (ID={song.Id}).");

        AlbumList_SelectionChanged(null, null);
    }

    private async void ChangeRole_Click(object sender, RoutedEventArgs e)
    {
        if (UserList.SelectedItem is not User user)
        {
            MessageBox.Show("Select a user.");
            return;
        }

        if (user.Id == _userService.CurrentUser!.Id)
        {
            MessageBox.Show("You cannot change your own admin role.");
            return;
        }
        string roleInput = Microsoft.VisualBasic.Interaction.InputBox(
            "Enter role:\n0 = Admin\n1 = Musician\n2 = User",
            "Change Role",
            ((int)user.Role).ToString());

        if (!int.TryParse(roleInput, out int role) || role < 0 || role > 2)
        {
            MessageBox.Show("Invalid role.");
            return;
        }

        user.Role = (UserRole)role;
        await _userData.UpdateRoleAsync(user);

        MessageBox.Show("Role updated.");
        LogService.Write($"Admin '{_userService.CurrentUser.Username}' changed role of user '{user.Username}' to {user.Role}.");
        LoadUsers();
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        new LoginWindow().Show();
        this.Close();
    }

}
