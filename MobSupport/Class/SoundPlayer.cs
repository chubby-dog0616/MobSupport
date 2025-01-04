using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;

namespace MobSupport.Class
{
    internal static class SoundPlayer
    {
        public static async void Play(Uri resouceUri)
        {
            var resourceStream = Application.GetResourceStream(resouceUri) ?? throw new FileNotFoundException($"リソースが見つかりません。") ;

            using (var mp3Reader = new Mp3FileReader(resourceStream.Stream))
            using (var player = new WaveOutEvent())
            {
                player.Init(mp3Reader);
                player.Play();

                while (player.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(100);
                }
            }
        }
    }
}
