using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextureEditor
{
    public enum TextureCompression
    {
        DXT1 = 0,
        DXT2, // Shouldnt be used
        DXT3,
        DXT4, // Shouldnt be used
        DXT5,
        NON_COMP
    }

    class CT3DTexture
    {
        public byte[] TextureData { get; set; }
        public byte Format { get; set; }
        public Bitmap BitmapImage { get; set; }

        public CT3DTexture()
        {
            TextureData = null;
            Format = (byte)TextureCompression.DXT3;
            BitmapImage = null;
        }

        public void LoadTexture(uint textureID, byte[] pData, uint compressedSize, byte bFormat)
        {
            // dwData is the size of the compressed data and dwSize is the size of the uncompressed data
            TextureData = TachyonCompressor.Decompress(pData, (int)compressedSize);
            Format = bFormat;
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
