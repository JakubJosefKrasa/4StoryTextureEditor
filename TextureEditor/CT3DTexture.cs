using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextureEditor
{
    public enum TextureCompression
    {
        DXT1 = 0,
        DXT2,
        DXT3,
        DXT4,
        DXT5,
        NON_COMP
    }

    class CT3DTexture
    {
        // Holds uncompressed texture data
        public byte[] TextureData { get; set; }
        public byte Format { get; set; }
        public Bitmap BitmapImage { get; set; }

        public CT3DTexture()
        {
            TextureData = null;
            Format = (byte)TextureCompression.DXT3;
            BitmapImage = null;
        }

        public void LoadTextureData(byte[] compressedData, uint compressedSize, uint originalSize, byte format)
        {
            IntPtr pUncompressedData = CTachyonWrapperFunctions.DoUncompressTextureData(compressedData, compressedSize, originalSize);

            if (pUncompressedData == IntPtr.Zero)
            {
                TextureData = null;
                Format = format;
            }

            byte[] uncompressedData = new byte[originalSize];
            Marshal.Copy(pUncompressedData, uncompressedData, 0, (int)originalSize);

            TextureData = uncompressedData;
            Format = format;

            CTachyonWrapperFunctions.DoFreeUncompressedTextureData(pUncompressedData);
        }

        public Bitmap GetTextureBitmap(bool bReloadImage)
        {
            if (BitmapImage != null && !bReloadImage) return BitmapImage;

            if (TextureData != null)
            {
                ddsparser.DDSImage ddsImage = new ddsparser.DDSImage(TextureData);
                BitmapImage = ddsImage.BitmapImage;

                return BitmapImage;
            }

            return null;
        }
    }
}
