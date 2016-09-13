using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Buffer = Windows.Storage.Streams.Buffer;

namespace BufferMemoryLeak
{
    public static class LeakyExample
    {
        public static async Task<ulong> Run()
        {
            var source = await PrepareFileSource();
            using (var memStream = new InMemoryRandomAccessStream())
            {
                using (var sourceStr = await source.OpenReadAsync())
                {
                    await CopyStreamAsync(sourceStr, memStream);
                    return memStream.Size;
                }
            }
        }

        public static async Task CopyStreamAsync(this IInputStream src, IOutputStream outStream, uint bufferSize = 16384)
        {
            IBuffer buf2;
            do
            {
                var buf = new Buffer(bufferSize);
                buf2 = await src.ReadAsync(buf, bufferSize, InputStreamOptions.None);
                if (buf2.Length > 0)
                {
                    await outStream.WriteAsync(buf2);
                }
            } while (buf2.Length > 0);
        }

        public static async Task<StorageFile> PrepareFileSource()
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("example.dat", CreationCollisionOption.OpenIfExists);
            var p = await file.GetBasicPropertiesAsync();
            if (p.Size == 0)
            {
                var b = new byte[1024];
                for (var j = 0; j < b.Length; j++)
                {
                    b[j] = (byte)(j % 256);
                }
                using (var str = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var wr = new DataWriter(str))
                    {
                        for (int i = 0; i < 4096; i++)
                        {
                            wr.WriteBytes(b);
                            if (i%5 == 4)
                            {
                                await wr.FlushAsync();
                            }
                        }
                        await wr.FlushAsync();
                    }
                }
            }
            return file;
        }
    }
}
