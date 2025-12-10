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

    private async void CreateAlbum_Click(object sender, RoutedEventArgs e)
    {
        string albumName = Microsoft.VisualBasic.Interaction.InputBox("Album title:", "Create Album");

        await _albumData.AddAsync(new Album
        {
            MusicianId = _musician!.Id,
            Title = albumName
        });

        MessageBox.Show("Album created!");
    }

    private async void AddSong_Click(object sender, RoutedEventArgs e)
    {
        string albumIdStr = Microsoft.VisualBasic.Interaction.InputBox("Album ID:", "Add Song");
        int albumId = int.Parse(albumIdStr);

        string songName = Microsoft.VisualBasic.Interaction.InputBox("Song title:", "Add Song");

        await _songData.AddAsync(new Song
        {
            AlbumId = albumId,
            Title = songName
        });

        MessageBox.Show("Song added!");
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        new LoginWindow().Show();
        this.Close();
    }
}
