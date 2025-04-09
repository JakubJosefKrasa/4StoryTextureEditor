using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

        // Dictionary where key is the texture file name and value is another dictionary where key is the dwPOS and value is the texture data
        public Dictionary<string, Dictionary<uint, CT3DTexture>> MapTexture { get; set; } = new Dictionary<string, Dictionary<uint, CT3DTexture>>();

        // Dictionary where key is the texture file name and value is another dictionary where key is the dwPOS and value is the texture set data that holds texture ids
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

            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                long fileLength = br.BaseStream.Length;
                long dwPOS = br.BaseStream.Position;

                while (dwPOS < fileLength)
                {
                    BinaryReader decompressedReader = TachyonCompressor.UcprStream(br);

                    int textureCount = decompressedReader.ReadInt32();

                    TextureSet textureSet = new TextureSet();

                    bool isFirstTexture = true;
                    uint firstTextureID = 0;

                    for (int i = 0; i < textureCount; i++)
                    {
                        uint textureID = decompressedReader.ReadUInt32();

                        if (isFirstTexture)
                        {
                            firstTextureID = textureID;
                            isFirstTexture = false;
                        }

                        textureSet.PushTexture(textureID, null);
                    }

                    int keyCount = decompressedReader.ReadInt32();
                    for (int i = 0; i < keyCount; i++)
                    {
                        UVKey uvKey = new UVKey();
                        uvKey.Tick = decompressedReader.ReadUInt32();
                        uvKey.KeyU = decompressedReader.ReadSingle();
                        uvKey.KeyV = decompressedReader.ReadSingle();
                        uvKey.KeyR = decompressedReader.ReadSingle();
                        uvKey.KeySU = decompressedReader.ReadSingle();
                        uvKey.KeySV = decompressedReader.ReadSingle();

                        textureSet.UVKeys.Add(uvKey);
                    }

                    textureSet.TotalTick = decompressedReader.ReadUInt32();
                    textureSet.MipFilter = decompressedReader.ReadUInt32();
                    textureSet.MipBias = decompressedReader.ReadSingle();

                    textureSet.TextureOption = decompressedReader.ReadByte();
                    byte bFormat = decompressedReader.ReadByte();

                    uint dwOriginalSize = decompressedReader.ReadUInt32();
                    uint dwCompressedSize = decompressedReader.ReadUInt32();

                    if (dwCompressedSize > 0)
                    {
                        CT3DTexture texture = new CT3DTexture();

                        byte[] textureData = decompressedReader.ReadBytes((int)dwCompressedSize);

                        texture.LoadTexture(firstTextureID, textureData, dwCompressedSize, bFormat);

                        // do not overwrite existing texture
                        if (!mapTexture.ContainsKey((uint)dwPOS))
                        {
                            mapTexture.Add((uint)dwPOS, texture);
                        }
                    }

                    // do not overwrite existing texture set
                    if (!mapTextureSet.ContainsKey((uint)dwPOS))
                    {
                        mapTextureSet.Add((uint)dwPOS, textureSet);
                    }

                    dwPOS = br.BaseStream.Position;
                    nIndex++;
                    //Console.WriteLine($"Progress: {nIndex * 100 / textureTotalCount}%");
                }
            }
            Console.WriteLine("Loading");
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
            string mapOutputDir = ".\\MapOutputTextures";
            if (!Directory.Exists(mapOutputDir))
                Directory.CreateDirectory(mapOutputDir);

            foreach (var texturesInFile in MapTextureSet)
            {
                foreach (var dwIDTexture in texturesInFile.Value)
                {
                    if (dwIDTexture.Value != null)
                    {
                        uint textureID = dwIDTexture.Key;
                        uint dwPOS = dwIDTexture.Value.GetTextureId(0);

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

            int fileIndex = 0;
            foreach (string textureFileName in TextureFileNames)
            {
                using (FileStream fileStream = new FileStream(Path.Combine(listOutputDir, textureFileName), FileMode.Create))
                {
                    if (!MapTextureSet.TryGetValue(textureFileName, out var textureSets))
                        continue;

                    foreach (KeyValuePair<uint, TextureSet> kvp in textureSets)
                    {
                        TextureSet textureSet = kvp.Value;
                        long startPos = fileStream.Position;

                        using (TachyonCompressor compressor = new TachyonCompressor())
                        {
                            int texturesCount = textureSet.GetTexturesCount();

                            compressor.Write(BitConverter.GetBytes(texturesCount), 0, sizeof(int));

                            for (int i = 0; i < texturesCount; i++)
                            {
                                uint textureID = textureSet.GetTextureId(i);
                                compressor.Write(BitConverter.GetBytes(textureID), 0, sizeof(uint));
                            }

                            int uvKeysCount = textureSet.GetUVKeysCount();
                            compressor.Write(BitConverter.GetBytes(uvKeysCount), 0, sizeof(int));

                            foreach (var uvKey in textureSet.UVKeys)
                            {
                                compressor.Write(BitConverter.GetBytes(uvKey.Tick), 0, sizeof(uint));
                                compressor.Write(BitConverter.GetBytes(uvKey.KeyU), 0, sizeof(float));
                                compressor.Write(BitConverter.GetBytes(uvKey.KeyV), 0, sizeof(float));
                                compressor.Write(BitConverter.GetBytes(uvKey.KeyR), 0, sizeof(float));
                                compressor.Write(BitConverter.GetBytes(uvKey.KeySU), 0, sizeof(float));
                                compressor.Write(BitConverter.GetBytes(uvKey.KeySV), 0, sizeof(float));
                            }

                            compressor.Write(BitConverter.GetBytes(textureSet.TotalTick), 0, sizeof(uint));
                            compressor.Write(BitConverter.GetBytes(textureSet.MipFilter), 0, sizeof(uint));
                            compressor.Write(BitConverter.GetBytes(textureSet.MipBias), 0, sizeof(float));
                            compressor.Write(BitConverter.GetBytes(textureSet.TextureOption), 0, sizeof(byte));

                            var texture = textureSet.GetTextureData(0);

                            if (texture == null)
                            {
                                compressor.Write(BitConverter.GetBytes(0), 0, sizeof(byte)); //bFormat
                                compressor.Write(BitConverter.GetBytes(0), 0, sizeof(uint)); // originalSize
                                compressor.Write(BitConverter.GetBytes(0), 0, sizeof(uint)); // compressedSize
                            }
                            else
                            {
                                compressor.Write(BitConverter.GetBytes(texture.Format), 0, sizeof(byte));

                                byte[] compressedTextureData = TachyonCompressor.Compress(texture.TextureData);
                                compressor.Write(BitConverter.GetBytes((uint)texture.TextureData.Length), 0, sizeof(uint));
                                compressor.Write(BitConverter.GetBytes((uint)compressedTextureData.Length), 0, sizeof(uint));
                                compressor.Write(compressedTextureData, 0, compressedTextureData.Length);
                            }

                            // Write compressed data to file and record position
                            compressor.ToFile(fileStream);
                        }

                        indexFiles.Add(new IndexFile(kvp.Key, (uint)fileIndex, (uint)startPos));
                    }
                }
                fileIndex++;
            }

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
    }
}
