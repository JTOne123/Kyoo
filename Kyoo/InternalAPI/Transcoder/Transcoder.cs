using Kyoo.InternalAPI.TranscoderLink;
using Kyoo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Kyoo.InternalAPI
{
    public class Transcoder : ITranscoder
    {
        private readonly string tempPath;

        public Transcoder(IConfiguration config)
        {
            tempPath = config.GetValue<string>("tempPath");

            Debug.WriteLine("&Api INIT (unmanaged stream size): " + TranscoderAPI.Init() + ", Stream size: " + Marshal.SizeOf<Models.Watch.Stream>());
        }

        public async Task<Track[]> ExtractSubtitles(string path)
        {
            string output = Path.Combine(Path.GetDirectoryName(path), "Subtitles");
            Directory.CreateDirectory(output);
            return await Task.Run(() => 
            { 
                TranscoderAPI.ExtractSubtitles(path, output, out Track[] tracks);
                return tracks;
            });
        }

        public string Transmux(WatchItem episode)
        {
            string folder = Path.Combine(tempPath, episode.Link);
            string manifest = Path.Combine(folder, episode.Link + ".mpd");

            Directory.CreateDirectory(folder);
            Debug.WriteLine("&Transmuxing " + episode.Link + " at " + episode.Path + ", outputPath: " + folder);

            //FFMPEG require us to put DirectorySeparaorChar as '/' for his internal regex.
            if (File.Exists(manifest) || TranscoderAPI.transmux(episode.Path, manifest.Replace('\\', '/'), (folder + Path.DirectorySeparatorChar).Replace('\\', '/')) == 0)
                return manifest;
            else
                return null;
        }

        public string Transcode(string path)
        {
            return @"D:\Videos\Anohana\AnoHana S01E01.mp4";
        }
    }
}
