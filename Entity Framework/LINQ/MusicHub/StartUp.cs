namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            //Console.WriteLine(ExportSongsAboveDuration(context,4));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Albums
                .ToList()
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price,
                        Writer = s.Writer.Name
                    })
                        .ToList()
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer)
                        .ToList(),
                    AlbumPrice = a.Price
                })
                .OrderByDescending(a => a.AlbumPrice)
                .ToList();

            var sb = new StringBuilder();
            var count = 0;
            

            foreach (var album in albums)
            {
                var counter = 0;

                sb.AppendLine($"-AlbumName: {album.AlbumName}")
                .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                .AppendLine($"-ProducerName: {album.ProducerName}")
                .AppendLine($"-Songs:");

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{++counter}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Price: {song.Price}")
                    .AppendLine($"---Writer: {song.Writer}");
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");

            }

            return sb.ToString().TrimEnd();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context
                .Songs
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new
                {
                    Name = x.Name,
                    SongPerformers = x.SongPerformers,
                    PerformerFullName = x.Performers
                    .Select(x => $"{x.FirstName} {x.LastName}").FirstOrDefault(),
                    Writer = x.Writer.Name,
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Writer)
                .ToList();


            var sb = new StringBuilder();
            var count = 0;
            foreach (var song in songs)
            {
                count++;
                if (song.SongPerformers.Count == 0)
                {
                    sb.AppendLine($"-Song #{count}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Writer: {song.Writer}");
                    sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                    sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
                }
                else if (song.SongPerformers.Count > 1)
                {
                    sb.AppendLine($"-Song #{count}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Writer: {song.Writer}");
                    sb.AppendLine($"---Performer: {string.Join(", ", song.PerformerFullName)}");
                    sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                    sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
                }
                else
                {
                    sb.AppendLine($"-Song #{count}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Writer: {song.Writer}");
                    sb.AppendLine($"---Performer: {song.PerformerFullName}");
                    sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                    sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}
