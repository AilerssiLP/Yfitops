namespace Yfitops.Models;

public class Song
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Title { get; set; } = "";
    public string FilePath { get; set; } = "";
}
