using Microsoft.Data.Sqlite;

namespace Yfitops.Database;

public static class DbInitializer
{
    public static void Initialize()
    {
        using var conn = new SqliteConnection("Data Source=app.db");
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText =
        @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL UNIQUE,
            PasswordHash TEXT NOT NULL,
            Salt TEXT NOT NULL,
            Role TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Musicians (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            StageName TEXT NOT NULL,
            FOREIGN KEY (UserId) REFERENCES Users(Id)
        );

        CREATE TABLE IF NOT EXISTS Albums (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            MusicianId INTEGER NOT NULL,
            Title TEXT NOT NULL,
            FOREIGN KEY (MusicianId) REFERENCES Musicians(Id)
        );

        CREATE TABLE IF NOT EXISTS Songs (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            AlbumId INTEGER NOT NULL,
            Title TEXT NOT NULL,
            FilePath TEXT,
            FOREIGN KEY (AlbumId) REFERENCES Albums(Id)
        );";

        cmd.ExecuteNonQuery();
    }
}
