using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TextureEditor
{
    public class TachyonCompressor : IDisposable
    {
        private MemoryStream _memoryStream = new MemoryStream();
        private byte[] _compressedData;
        private uint _compressedSize;

        public void Write(byte[] buffer, int offset, int count)
        {
            _memoryStream.Write(buffer, offset, count);
        }

        public static BinaryReader UcprStream(BinaryReader br)
        {
            uint decompressedOriginalLength = br.ReadUInt32();
            uint decompressedLength = br.ReadUInt32();

            byte[] decompressedData = TachyonCompressor.Decompress(br.ReadBytes(Convert.ToInt32(decompressedLength)));

            return new BinaryReader(new MemoryStream(decompressedData));
        }

        public static byte[] Compress(byte[] data)
        {
            try
            {
                using (var output = new MemoryStream())
                using (var compressor = new GZipStream(output, CompressionMode.Compress))
                {
                    compressor.Write(data, 0, data.Length);
                    compressor.Close();
                    return output.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Compression error: " + ex.Message);
                return null;
            }
        }

        public static byte[] Decompress(byte[] compressedData, int compressedSize)
        {
            try
            {
                using (var input = new MemoryStream(compressedData, 0, compressedSize))
                using (var deflate = new GZipStream(input, CompressionMode.Decompress))
                using (var output = new MemoryStream())
                {
                    deflate.CopyTo(output);
                    return output.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decompression error: " + ex.Message);
                return null;
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            try
            {
                using (var input = new MemoryStream(compressedData)) // can be overloaded with index and compressedSize if needed
                using (var dcpr = new GZipStream(input, CompressionMode.Decompress))
                using (var output = new MemoryStream())
                {
                    dcpr.CopyTo(output);
                    return output.ToArray(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decompression error: " + ex.Message);

                return null;
            }
        }

        public void ToFile(FileStream file)
        {
            Compress();

            uint dwOriginalSize = (uint)_memoryStream.Length;
            byte[] originSizeBytes = BitConverter.GetBytes(dwOriginalSize);
            byte[] compressedSizeBytes = BitConverter.GetBytes(_compressedSize);

            file.Write(originSizeBytes, 0, originSizeBytes.Length);
            file.Write(compressedSizeBytes, 0, compressedSizeBytes.Length);
            file.Write(_compressedData, 0, _compressedData.Length);
        }

        public void Dispose()
        {
            _memoryStream?.Dispose();
            _compressedData = null;
        }

        private void Compress()
        {
            byte[] input = _memoryStream.ToArray();
            using (MemoryStream outputMs = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outputMs, CompressionMode.Compress, true))
                {
                    gzipStream.Write(input, 0, input.Length);
                }
                _compressedData = outputMs.ToArray();
                _compressedSize = (uint)_compressedData.Length;
            }
        }
    }
}
