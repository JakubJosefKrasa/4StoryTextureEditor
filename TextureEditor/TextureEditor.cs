using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace TextureEditor
{
    class IndexFile
    {
        public uint TextureID { get; }
        public uint FileID { get; }
        public uint Position { get; }

        public IndexFile(uint textureID, uint fileID, uint position)
        {
            TextureID = textureID;
            FileID = fileID;
            Position = position;
        }
    }

    class TextureEditor
    {
        public string DirectoryPath { get; set; }

        // Contains Texture file names without path
        public List<string> TextureFileNames { get; set; } = new List<string>();

        // Dictionary where key is the texture file name and value is another dictionary where key is the texture id and value is the texture data
        public Dictionary<string, Dictionary<uint, CT3DTexture>> MapTexture { get; set; } = new Dictionary<string, Dictionary<uint, CT3DTexture>>();

        // Dictionary where key is the texture file name and value is another dictionary where key is the texture id and value is the texture set data that holds texture and its all info
        public Dictionary<string, Dictionary<uint, TextureSet>> MapTextureSet { get; set; } = new Dictionary<string, Dictionary<uint, TextureSet>>();

        public static string LoadString(BinaryReader br)
        {
            string strToReturn = "";

            int nLength = br.ReadInt32();
            if (nLength > 0)
            {
                byte[] bytes = br.ReadBytes(nLength);
                strToReturn = Encoding.Default.GetString(bytes);
            }

            return strToReturn;
        }

        public void LoadTEX(int textureComp, string indexFile)
        {
            MapTexture.Clear();
            MapTextureSet.Clear();

            TextureFileNames.Clear();

            if (!File.Exists(indexFile))
            {
                Console.WriteLine("Index file not found: " + indexFile);
                return;
            }

            using (BinaryReader br = new BinaryReader(File.Open(indexFile, FileMode.Open)))
            {
                int textureFileCount = br.ReadInt32();
                int textureTotalCount = br.ReadInt32();

                Dictionary<string, Dictionary<uint, CT3DTexture>> mapTextureTemp = new Dictionary<string, Dictionary<uint, CT3DTexture>>();
                Dictionary<string, Dictionary<uint, TextureSet>> mapTextureSetTemp = new Dictionary<string, Dictionary<uint, TextureSet>>();

                // Contains texture file names with path
                List<string> textureFilesToLoad = new List<string>();

                // Load from index file all texture (.TTX) file names
                for (int i = 0; i < textureFileCount; i++)
                {
                    string textureFile = LoadString(br);

                    // Add texture file with path
                    textureFilesToLoad.Add(textureFile);

                    // Remove path and add to list to be stored as key
                    int index = textureFile.IndexOf("\\");
                    textureFile = textureFile.Substring(index + 1);
                    TextureFileNames.Add(textureFile);
                }

                int nIndex = 0;
                int n = 0;
                for (int i = 0; i < textureFilesToLoad.Count; i++)
                {
                    MapTexture.Add(TextureFileNames[i], new Dictionary<uint, CT3DTexture>());
                    MapTextureSet.Add(TextureFileNames[i], new Dictionary<uint, TextureSet>());

                    mapTextureTemp.Add(TextureFileNames[i], new Dictionary<uint, CT3DTexture>());
                    mapTextureSetTemp.Add(TextureFileNames[i], new Dictionary<uint, TextureSet>());

                    LoadTEX(textureFilesToLoad[i], mapTextureTemp[TextureFileNames[i]], mapTextureSetTemp[TextureFileNames[i]], ref nIndex, textureTotalCount);
                    n++;
                }


                for (int i = 0; i < textureTotalCount; i++)
                {
                    uint dwTextureID = br.ReadUInt32();
                    uint dwFileID = br.ReadUInt32();
                    uint dwPOS = br.ReadUInt32();


                    if (mapTextureTemp[TextureFileNames[(int)dwFileID]].TryGetValue(dwPOS, out CT3DTexture texture))
                    {
                        MapTexture[TextureFileNames[(int)dwFileID]].Add(dwTextureID, texture);
                        mapTextureTemp[TextureFileNames[(int)dwFileID]].Remove(dwPOS);
                    }

                    if (mapTextureSetTemp[TextureFileNames[(int)dwFileID]].TryGetValue(dwPOS, out TextureSet textureSet))
                    {
                        MapTextureSet[TextureFileNames[(int)dwFileID]].Add(dwTextureID, textureSet);
                        mapTextureSetTemp[TextureFileNames[(int)dwFileID]].Remove(dwPOS);
                    }
                }
            }

            Console.WriteLine("TEX files loaded successfully");
        }

        private void LoadTEX(string file, Dictionary<uint, CT3DTexture> mapTexture, Dictionary<uint, TextureSet> mapTextureSet, ref int nIndex, int textureTotalCount)
        {
            string filePath = $"{DirectoryPath}\\Data\\{file}";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Texture file not found: " + filePath);
                return;
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fileStream))
            {
                long dwLength = fileStream.Length;
                long dwPos = fileStream.Position;

                while (dwPos < dwLength)
                {
                    uint originalLength = br.ReadUInt32();
                    uint compressedLength = br.ReadUInt32();

                    uint keyPos = (uint)dwPos;

                    byte[] compressedData = br.ReadBytes((int)compressedLength);

                    IntPtr ucpr = CTachyonWrapperFunctions.UncompressorCreate();

                    CTachyonWrapperFunctions.UncompressorUncompress(ucpr, compressedData, compressedLength, originalLength);

                    TextureSet textureSet = new TextureSet();

                    int textureCount = ReadInt32(ucpr);

                    bool isFirstTexture = true;
                    uint firstTextureID = 0;

                    for (int i = 0; i < textureCount; i++)
                    {
                        uint textureID = ReadUInt32(ucpr);

                        if (isFirstTexture)
                        {
                            firstTextureID = textureID;
                            isFirstTexture = false;
                        }

                        textureSet.PushTexture(textureID, null);
                    }

                    int keyCount = ReadInt32(ucpr);
                    for (int i = 0; i < keyCount; i++)
                    {
                        UVKey uvKey = new UVKey();
                        uvKey.Tick = ReadUInt32(ucpr);
                        uvKey.KeyU = ReadFloat(ucpr);
                        uvKey.KeyV = ReadFloat(ucpr);
                        uvKey.KeyR = ReadFloat(ucpr);
                        uvKey.KeySU = ReadFloat(ucpr);
                        uvKey.KeySV = ReadFloat(ucpr);

                        textureSet.UVKeys.Add(uvKey);
                    }

                    textureSet.TotalTick = ReadUInt32(ucpr);
                    textureSet.MipFilter = ReadUInt32(ucpr);
                    textureSet.MipBias = ReadFloat(ucpr);

                    textureSet.TextureOption = ReadByte(ucpr);
                    byte bFormat = ReadByte(ucpr);

                    uint dwOriginalSize = ReadUInt32(ucpr);
                    uint dwCompressedSize = ReadUInt32(ucpr);

                    if (dwCompressedSize > 0)
                    {
                        CT3DTexture texture = new CT3DTexture();

                        byte[] compressedTextureData = ReadBytes(ucpr, dwCompressedSize);

                        texture.LoadTextureData(compressedTextureData, dwCompressedSize, dwOriginalSize, bFormat);

                        CTachyonWrapperFunctions.UncompressorSeek(ucpr, dwCompressedSize, CTachyonWrapperFunctions.CFILE_CURRENT);

                        // do not overwrite existing texture
                        if (!mapTexture.ContainsKey((uint)keyPos))
                        {
                            mapTexture.Add((uint)keyPos, texture);
                        }
                    }

                    // do not overwrite existing texture set
                    if (!mapTextureSet.ContainsKey((uint)keyPos))
                    {
                        mapTextureSet.Add((uint)keyPos, textureSet);
                    }

                    CTachyonWrapperFunctions.UncompressorDestroy(ucpr);

                    dwPos = fileStream.Position;
                    nIndex++;
                    Console.WriteLine($"Progress: {nIndex * 100 / textureTotalCount}%");
                }
            }
        }

        public void CompleteTEX()
        {
            foreach (var texturesInFile in MapTextureSet)
            {
                foreach (var dwTextureById in texturesInFile.Value)
                {
                    if (dwTextureById.Value != null)
                    {
                        int nTexturesCount = dwTextureById.Value.GetTexturesCount();

                        for (int i = 0; i < nTexturesCount; i++)
                        {
                            uint textureIdToFind = dwTextureById.Value.GetTextureId(i);

                            if (MapTexture[texturesInFile.Key].TryGetValue(textureIdToFind, out CT3DTexture texture))
                            {
                                dwTextureById.Value.SetTextureData(i, texture);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("TEX files completed successfully");
        }

        public void SaveTexturesInPNG()
        {
            string mapOutputDir = ".\\DumpedTextures";
            if (!Directory.Exists(mapOutputDir))
                Directory.CreateDirectory(mapOutputDir);

            foreach (var texturesInFile in MapTextureSet)
            {
                foreach (var dwIDTexture in texturesInFile.Value)
                {
                    if (dwIDTexture.Value != null)
                    {
                        uint textureID = dwIDTexture.Key;

                        if (dwIDTexture.Value.GetTextureData(0) != null)
                        {
                            Bitmap bmp = dwIDTexture.Value.GetTextureData(0).GetTextureBitmap(false);

                            if (bmp != null)
                            {
                                string filePath = Path.Combine(mapOutputDir, $"{texturesInFile.Key}_{textureID}.png");

                                try
                                {
                                    bmp.Save(filePath, ImageFormat.Png);
                                    Console.WriteLine($"Saved texture {textureID} to {filePath}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error saving texture {textureID}: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SaveTexturesInTTX()
        {
            string listOutputDir = ".\\Data\\Skin";
            if (!Directory.Exists(listOutputDir))
                Directory.CreateDirectory(listOutputDir);

            List<IndexFile> indexFiles = new List<IndexFile>();

            IntPtr device = CTachyonWrapperFunctions.DeviceCreate();

            int fileIndex = 0;
            foreach (string textureFileName in TextureFileNames)
            {
                if (!MapTextureSet.TryGetValue(textureFileName, out var textureSets))
                    continue;

                string fileNameAndPath = Path.Combine(listOutputDir, textureFileName);

                IntPtr pFile = CTachyonWrapperFunctions.DoCreateNewFile(fileNameAndPath);


                foreach (KeyValuePair<uint, TextureSet> kvp in textureSets)
                {
                    uint filePosition = CTachyonWrapperFunctions.DoGetFilePosition(pFile);

                    IntPtr tachyonCompressor = CTachyonWrapperFunctions.CompressorCreate();

                    TextureSet textureSet = kvp.Value;

                    int texturesCount = textureSet.GetTexturesCount();

                    WriteInt32(tachyonCompressor, texturesCount);

                    for (int i = 0; i < texturesCount; i++)
                    {
                        uint textureID = textureSet.GetTextureId(i);
                        WriteUInt32(tachyonCompressor, textureID);
                    }

                    int uvKeysCount = textureSet.GetUVKeysCount();

                    WriteInt32(tachyonCompressor, uvKeysCount);

                    foreach (var uvKey in textureSet.UVKeys)
                    {
                        WriteUInt32(tachyonCompressor, uvKey.Tick);
                        WriteFloat(tachyonCompressor, uvKey.KeyU);
                        WriteFloat(tachyonCompressor, uvKey.KeyV);
                        WriteFloat(tachyonCompressor, uvKey.KeyR);
                        WriteFloat(tachyonCompressor, uvKey.KeySU);
                        WriteFloat(tachyonCompressor, uvKey.KeySV);
                    }

                    WriteUInt32(tachyonCompressor, textureSet.TotalTick);
                    WriteUInt32(tachyonCompressor, textureSet.MipFilter);
                    WriteFloat(tachyonCompressor, textureSet.MipBias);
                    WriteByte(tachyonCompressor, textureSet.TextureOption);

                    var texture = textureSet.GetTextureData(0);

                    if (texture == null)
                    {
                        WriteByte(tachyonCompressor, 0); //bFormat
                        WriteUInt32(tachyonCompressor, 0); // originalSize
                        WriteUInt32(tachyonCompressor, 0); // compressedSize
                    }
                    else
                    {
                        WriteByte(tachyonCompressor, texture.Format);

                        uint originalSize;
                        uint generatedSize;

                        IntPtr generatedDataPtr;

                        bool result = CTachyonWrapperFunctions.DoGenerateTextureDDS(device, texture.TextureData, (uint)texture.TextureData.Length, texture.Format, out generatedDataPtr, out generatedSize, out originalSize);
                        if (!result)
                        {
                            Console.WriteLine("Error generating texture data");
                            return;
                        }

                        byte[] generatedData = new byte[generatedSize];
                        Marshal.Copy(generatedDataPtr, generatedData, 0, (int)generatedSize);

                        WriteUInt32(tachyonCompressor, originalSize);
                        WriteUInt32(tachyonCompressor, generatedSize);
                        WriteBytes(tachyonCompressor, generatedData, generatedSize);
                    }

                    CTachyonWrapperFunctions.CompressorToFile(tachyonCompressor, pFile);
                    CTachyonWrapperFunctions.CompressorDestroy(tachyonCompressor);

                    indexFiles.Add(new IndexFile(kvp.Key, (uint)fileIndex, filePosition));
                }

                CTachyonWrapperFunctions.DoDestroyFile(pFile);

                fileIndex++;
            }

            CTachyonWrapperFunctions.DeviceDestroy(device);

            BuildIndexFile(indexFiles);

            MessageBox.Show("Textures saved successfully");
        }

        private void BuildIndexFile(List<IndexFile> indexFiles)
        {
            string indexOutPutDir = ".\\Index";
            if (!Directory.Exists(indexOutPutDir))
                Directory.CreateDirectory(indexOutPutDir);

            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(Path.Combine(indexOutPutDir, "0_TClientS.IDX"), FileMode.Create)))
            {
                binaryWriter.Write(TextureFileNames.Count);
                binaryWriter.Write(indexFiles.Count);

                foreach (var textureFileName in TextureFileNames)
                {
                    string textureFileNameAndPath = $"Skin\\{textureFileName}";
                    byte[] textureFileNameAndPathBytes = Encoding.Default.GetBytes(textureFileNameAndPath);
                    binaryWriter.Write(textureFileNameAndPathBytes.Length);
                    binaryWriter.Write(textureFileNameAndPathBytes);
                }

                foreach (var indexFile in indexFiles)
                {
                    binaryWriter.Write(indexFile.TextureID);
                    binaryWriter.Write(indexFile.FileID);
                    binaryWriter.Write(indexFile.Position);
                }
            }
        }

    #region CTachyon helper methods for reading and writing

        private int ReadInt32(IntPtr ucpr)
        {
            byte[] buffer = new byte[4];
            CTachyonWrapperFunctions.UncompressorRead(ucpr, buffer, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        private uint ReadUInt32(IntPtr ucpr)
        {
            byte[] buffer = new byte[4];
            CTachyonWrapperFunctions.UncompressorRead(ucpr, buffer, 4);

            return BitConverter.ToUInt32(buffer, 0);
        }

        private float ReadFloat(IntPtr ucpr)
        {
            byte[] buffer = new byte[4];
            CTachyonWrapperFunctions.UncompressorRead(ucpr, buffer, 4);

            return BitConverter.ToSingle(buffer, 0);
        }

        private byte ReadByte(IntPtr ucpr)
        {
            byte[] buffer = new byte[1];
            CTachyonWrapperFunctions.UncompressorRead(ucpr, buffer, 1);

            return buffer[0];
        }

        private byte[] ReadBytes(IntPtr ucpr, uint count)
        {
            byte[] buffer = new byte[count];
            CTachyonWrapperFunctions.UncompressorRead(ucpr, buffer, count);

            return buffer;
        }

        private void WriteInt32(IntPtr ucpr, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            CTachyonWrapperFunctions.CompressorWrite(ucpr, buffer, (uint)buffer.Length);
        }

        private void WriteUInt32(IntPtr ucpr, uint value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            CTachyonWrapperFunctions.CompressorWrite(ucpr, buffer, (uint)buffer.Length);
        }

        private void WriteFloat(IntPtr ucpr, float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            CTachyonWrapperFunctions.CompressorWrite(ucpr, buffer, (uint)buffer.Length);
        }

        private void WriteByte(IntPtr ucpr, byte value)
        {
            byte[] buffer = new byte[1] { value };
            CTachyonWrapperFunctions.CompressorWrite(ucpr, buffer, (uint)buffer.Length);
        }

        private void WriteBytes(IntPtr ucpr, byte[] data, uint count)
        {
            CTachyonWrapperFunctions.CompressorWrite(ucpr, data, count);
        }

        #endregion
    }
}
