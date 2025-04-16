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

        // holds texture quality 0 - 2 where 0 is the worst texture quality
        public int TextureOption { get; set; }

        // path and name with extension of loaded texture file
        public string LoadedTextureFileFullPathAndName { get; set; }

        // name and extension of loaded texture file
        public string LoadedTextureFileName {  get; set; }

        // Dictionary where key is the texture id and value is the texture data
        public Dictionary<uint, CT3DTexture> MapTexture { get; set; } = new Dictionary<uint, CT3DTexture>();

        // Dictionary where key is the texture id and value is the texture set data that holds texture and its all info
        public Dictionary<uint, TextureSet> MapTextureSet { get; set; } = new Dictionary<uint, TextureSet>();

        private List<string> AllTextureFiles { get; set; } = new List<string>();

        private int IndexOfTextureFile { get; set; }

        // Holds total count of textures in index file excluding textures from loaded texture file (example: in index file total count of textures is 100 and we loaded 10 textures from texture file so it holds 90)
        private int TotalCountOfTexturesInIndexFile { get; set; }

        public void LoadTEX()
        {
            Dictionary<uint, CT3DTexture> mapTextureTemp = new Dictionary<uint, CT3DTexture>();
            Dictionary<uint, TextureSet> mapTextureSetTemp = new Dictionary<uint, TextureSet>();

            string skinKeyWord = "Skin\\";
            int indexOfSkin = LoadedTextureFileFullPathAndName.IndexOf(skinKeyWord);

            // Gets only texture file name with extension without path
            string textureFileName = indexOfSkin >= 0 ? LoadedTextureFileFullPathAndName.Substring(indexOfSkin + skinKeyWord.Length) : "";

            LoadTEX(textureFileName, mapTextureTemp, mapTextureSetTemp);


            string indexFileAndPath = $"{DirectoryPath}\\Index\\{TextureOption}_TClientS.IDX";

            if (!File.Exists(indexFileAndPath))
            {
                Console.WriteLine("Index file not found: " + indexFileAndPath);
                return;
            }

            Console.WriteLine("Index file found");

            using (BinaryReader br = new BinaryReader(File.Open(indexFileAndPath, FileMode.Open)))
            {
                int textureFileCount = br.ReadInt32();
                int textureTotalCount = br.ReadInt32();

                IndexOfTextureFile = -1;

                for (int i = 0; i < textureFileCount; i++)
                {
                    string textureFile = LoadString(br);

                    // Remove path and add to list to be stored as key
                    int index = textureFile.IndexOf("\\");
                    textureFile = textureFile.Substring(index + 1);

                    AllTextureFiles.Add(textureFile);
                    
                    if (textureFile == LoadedTextureFileName) IndexOfTextureFile = i;
                }

                if (IndexOfTextureFile == -1)
                {
                    MessageBox.Show("Texture File wasn't found in Index File");

                    return;
                }


                int loadedTexturesCount = 0;
                for (int i = 0; i < textureTotalCount; i++)
                {
                    uint dwTextureID = br.ReadUInt32();
                    uint dwFileID = br.ReadUInt32();
                    uint dwPOS = br.ReadUInt32();

                    if (dwFileID == IndexOfTextureFile)
                    {
                        if (mapTextureTemp.TryGetValue(dwPOS, out CT3DTexture texture))
                        {
                            MapTexture.Add(dwTextureID, texture);
                            mapTextureTemp.Remove(dwPOS);
                        }

                        if (mapTextureSetTemp.TryGetValue(dwPOS, out TextureSet textureSet))
                        {
                            MapTextureSet.Add(dwTextureID, textureSet);
                            mapTextureSetTemp.Remove(dwPOS);

                            loadedTexturesCount++;
                        }
                    }
                }

                TotalCountOfTexturesInIndexFile = textureTotalCount - loadedTexturesCount;
            }

            Console.WriteLine("Finished loading");
        }
        private void LoadTEX(string file, Dictionary<uint, CT3DTexture> mapTexture, Dictionary<uint, TextureSet> mapTextureSet)
        {
            string filePath = $"{DirectoryPath}\\Data\\Skin\\{file}";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Texture file not found: " + filePath);
                return;
            }

            MapTexture.Clear();
            MapTextureSet.Clear();

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
                }
            }
        }

        public void CompleteTEX()
        {
            foreach (var texture in MapTextureSet)
            {
                if (texture.Value != null)
                {
                    int nTexturesCount = texture.Value.GetTexturesCount();

                    for (int i = 0; i < nTexturesCount; i++)
                    {
                        uint textureIdToFind = texture.Value.GetTextureId(i);

                        if (MapTexture.TryGetValue(textureIdToFind, out CT3DTexture foundTexture))
                        {
                            texture.Value.SetTextureData(i, foundTexture);
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

            foreach (var textureKVP in MapTextureSet)
            {
                if (textureKVP.Value != null)
                {
                    uint textureID = textureKVP.Key;

                    if (textureKVP.Value.GetTextureData(0) != null)
                    {
                        Bitmap bmp = textureKVP.Value.GetTextureData(0).GetTextureBitmap(false);

                        string filePath = Path.Combine(mapOutputDir, $"{TextureOption}_{LoadedTextureFileName}_{textureID}.png");

                        try
                        {
                            bmp.Save(filePath, ImageFormat.Png);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving texture {textureID}: {ex.Message}");
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

            string fileNameAndPath = Path.Combine(listOutputDir, LoadedTextureFileName);

            IntPtr pFile = CTachyonWrapperFunctions.DoCreateNewFile(fileNameAndPath);

            foreach (KeyValuePair<uint, TextureSet> textureSet in MapTextureSet)
            {
                uint filePosition = CTachyonWrapperFunctions.DoGetFilePosition(pFile);

                IntPtr tachyonCompressor = CTachyonWrapperFunctions.CompressorCreate();

                TextureSet texture = textureSet.Value;

                int texturesCount = texture.GetTexturesCount();

                WriteInt32(tachyonCompressor, texturesCount);

                for (int i = 0; i < texturesCount; i++)
                {
                    uint textureID = texture.GetTextureId(i);
                    WriteUInt32(tachyonCompressor, textureID);
                }

                int uvKeysCount = texture.GetUVKeysCount();

                WriteInt32(tachyonCompressor, uvKeysCount);

                foreach (var uvKey in texture.UVKeys)
                {
                    WriteUInt32(tachyonCompressor, uvKey.Tick);
                    WriteFloat(tachyonCompressor, uvKey.KeyU);
                    WriteFloat(tachyonCompressor, uvKey.KeyV);
                    WriteFloat(tachyonCompressor, uvKey.KeyR);
                    WriteFloat(tachyonCompressor, uvKey.KeySU);
                    WriteFloat(tachyonCompressor, uvKey.KeySV);
                }

                WriteUInt32(tachyonCompressor, texture.TotalTick);
                WriteUInt32(tachyonCompressor, texture.MipFilter);
                WriteFloat(tachyonCompressor, texture.MipBias);
                WriteByte(tachyonCompressor, texture.TextureOption);

                var textureData = texture.GetTextureData(0);

                if (textureData == null)
                {
                    WriteByte(tachyonCompressor, 0); //bFormat
                    WriteUInt32(tachyonCompressor, 0); // originalSize
                    WriteUInt32(tachyonCompressor, 0); // compressedSize
                }
                else
                {
                    WriteByte(tachyonCompressor, textureData.Format);

                    uint originalSize;
                    uint generatedSize;

                    IntPtr generatedDataPtr;

                    bool result = CTachyonWrapperFunctions.DoGenerateTextureDDS(device, textureData.TextureData, (uint)textureData.TextureData.Length, textureData.Format, out generatedDataPtr, out generatedSize, out originalSize);
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

                indexFiles.Add(new IndexFile(textureSet.Key, (uint)IndexOfTextureFile, filePosition));
            }

            CTachyonWrapperFunctions.DoCloseFile(pFile);

            CTachyonWrapperFunctions.DeviceDestroy(device);

            BuildIndexFile(indexFiles);

            MessageBox.Show("Textures saved successfully");
        }

        private void BuildIndexFile(List<IndexFile> indexFiles)
        {
            string indexOutPutDir = ".\\Index";
            if (!Directory.Exists(indexOutPutDir))
                Directory.CreateDirectory(indexOutPutDir);

            int totalTexturesInNewIndexFile = TotalCountOfTexturesInIndexFile + indexFiles.Count;

            string indexFileAndPath = $"{DirectoryPath}\\Index\\{TextureOption}_TClientS.IDX";

            using (BinaryReader binaryReader = new BinaryReader(File.Open(indexFileAndPath, FileMode.Open)))
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(Path.Combine(indexOutPutDir, $"{TextureOption}_TClientS.IDX"), FileMode.Create)))
            {
                // read old index and store all info
                int readTextureFileCount = binaryReader.ReadInt32();
                int textureTotalCount = binaryReader.ReadInt32();

                
                List<string> textureFileList = new List<string>();
                for (int i = 0; i < readTextureFileCount; i++)
                {
                    textureFileList.Add(LoadString(binaryReader));
                }

                List<IndexFile> oldRecords = new List<IndexFile>();
                for (int i = 0; i < textureTotalCount; i++)
                {
                    uint dwTextureID = binaryReader.ReadUInt32();
                    uint dwFileID = binaryReader.ReadUInt32();
                    uint dwPOS = binaryReader.ReadUInt32();

                    oldRecords.Add(new IndexFile(dwTextureID, dwFileID, dwPOS));
                }

                List<IndexFile> newRecords = new List<IndexFile>();
                bool isNewInserted = false;

                foreach (IndexFile indexFile in oldRecords)
                {
                    if (indexFile.FileID == IndexOfTextureFile)
                    {
                        if (!isNewInserted)
                        {
                            newRecords.AddRange(indexFiles);
                            isNewInserted = true;
                        }
                    }
                    else
                    {
                        newRecords.Add(indexFile);
                    }
                }

                int newTotalTexturesCount = newRecords.Count;

                binaryWriter.Write(AllTextureFiles.Count);
                binaryWriter.Write(newTotalTexturesCount);

                foreach (string textureFileName in AllTextureFiles)
                {
                    string textureFileNameAndPath = $"Skin\\{textureFileName}";
                    byte[] textureFileNameAndPathBytes = Encoding.Default.GetBytes(textureFileNameAndPath);
                    binaryWriter.Write(textureFileNameAndPathBytes.Length);
                    binaryWriter.Write(textureFileNameAndPathBytes);
                }

                foreach (IndexFile indexFile in newRecords)
                {
                    binaryWriter.Write(indexFile.TextureID);
                    binaryWriter.Write(indexFile.FileID);
                    binaryWriter.Write(indexFile.Position);
                }
            }
        }

        private string LoadString(BinaryReader br)
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
