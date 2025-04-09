using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextureEditor
{
    enum TextureFilterType
    {
        D3DTEXF_NONE = 0,    // filtering disabled (valid for mip filter only)
        D3DTEXF_POINT = 1,    // nearest
        D3DTEXF_LINEAR = 2,    // linear interpolation
        D3DTEXF_ANISOTROPIC = 3,    // anisotropic
        D3DTEXF_PYRAMIDALQUAD = 6,    // 4-sample tent
        D3DTEXF_GAUSSIANQUAD = 7,    // 4-sample gaussian
        /* D3D9Ex only -- */
        D3DTEXF_CONVOLUTIONMONO = 8,    // Convolution filter for monochrome textures
        D3DTEXF_FORCE_DWORD = 0x7fffffff,   // force 32-bit size enum
    }

    class TextureSet
    {
        // Holds textures with their IDS as a key
        public List<KeyValuePair<uint, CT3DTexture>> Textures { get; } = new List<KeyValuePair<uint, CT3DTexture>>();
        public List<UVKey> UVKeys { get; set; } = new List<UVKey>();

        public uint TotalTick { get; set; } = 1000;
        public uint CurTick { get; set; } = 0;
        public uint MipFilter { get; set; } = (uint)TextureFilterType.D3DTEXF_LINEAR;
        public float MipBias { get; set; } = 0.0f;
        public byte TextureOption { get; set; } = 0;

        public void PushTexture(uint textureID, CT3DTexture texture)
        {
            Textures.Add(new KeyValuePair<uint, CT3DTexture>(textureID, texture));
        }

        public uint GetTextureId(int index)
        {
            return Textures[index].Key;
        }

        public CT3DTexture GetTextureData(int index)
        {
            if (index < 0 || index >= Textures.Count) return null;
            return Textures[index].Value;
        }

        public void SetTextureData(int index, CT3DTexture texture)
        {
            if (index < 0 || index >= Textures.Count) return;

            Textures[index] = new KeyValuePair<uint, CT3DTexture>(Textures[index].Key, texture);
        }

        public int GetTexturesCount()
        {
            return Textures.Count;
        }

        public UVKey GetUVKey(int index)
        {
            return UVKeys[index];
        }
        public int GetUVKeysCount()
        {
            return UVKeys.Count;
        }
    }
}
