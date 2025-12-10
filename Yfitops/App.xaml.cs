using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Yfitops.Data;
using Yfitops.Services;
using Yfitops.Database;

namespace Yfitops;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IUserData, UserData>();
        services.AddSingleton<IMusicianData, MusicianData>();
        services.AddSingleton<IAlbumData, AlbumData>();
        services.AddSingleton<ISongData, SongData>();
        services.AddSingleton<UserService>();

        Services = services.BuildServiceProvider();

        DbInitializer.Initialize();
    }
}
