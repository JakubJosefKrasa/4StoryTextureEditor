using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using ddsparser;
using TextureEditor;
using System.Reflection;
using System.IO.Compression;
using System.Runtime.InteropServices;
using ImageMagick;

namespace TextureEditor
{
    public partial class Form1 : Form
    {
        private static readonly TextureEditor textureEditor = new TextureEditor();

        public Form1()
        {
            InitializeComponent();
            FillMipFilterComboBox();
            FillTextureFormatComboBox();
        }

        private void FillMipFilterComboBox()
        {
            MipFilterComboBox.DataSource = Enum.GetValues(typeof(TextureFilterType))
                .Cast<TextureFilterType>()
                .ToDictionary(e => e.ToString(), e => e)
                .ToList();

            MipFilterComboBox.DisplayMember = "Key";
            MipFilterComboBox.ValueMember = "Value";
            MipFilterComboBox.SelectedIndex = -1;
        }

        private void FillTextureFormatComboBox()
        {
            TextureFormatComboBox.DataSource = Enum.GetValues(typeof(TextureCompression))
                .Cast<TextureCompression>()
                .ToDictionary(e => e.ToString(), e => e)
                .ToList();

            TextureFormatComboBox.DisplayMember = "Key";
            TextureFormatComboBox.ValueMember = "Value";
            TextureFormatComboBox.SelectedIndex = -1;
        }


        private void LoadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Index File (*S.IDX)|*S.IDX";
                openFileDialog.Title = "Select an Index file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("Loading index file: " + openFileDialog.FileName);
                    string fullPath = openFileDialog.FileName;
                    int indexPosition = fullPath.LastIndexOf("\\Index\\");

                    if (indexPosition > -1)
                    {
                        textureEditor.DirectoryPath = fullPath.Substring(0, indexPosition);
                        textureEditor.LoadTEX(0, fullPath);
                        textureEditor.CompleteTEX();

                        textureEditor.TextureFileNames.ForEach(textureFileName => SelectFileComboBox.Items.Add(textureFileName));
                        DumpAllTexturesPngButton.Enabled = true;
                        SelectFileComboBox.Enabled = true;
                        MessageBox.Show("Textures loaded");
                    }
                    else
                    {
                        MessageBox.Show("Invalid index file path");
                    }
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textureEditor.SaveTexturesInTTX();
        }

        private void SelectFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = SelectFileComboBox.SelectedIndex;

            if (selectedIndex < 0) return;

            TexturesListBox.Items.Clear();

            foreach (var kvp in textureEditor.MapTextureSet[textureEditor.TextureFileNames[selectedIndex]])
            {
                TexturesListBox.Items.Add($"ID: {kvp.Key}");
            }
            UpdateTexturesInFileCountLabel();
        }

        private void TexturesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = TexturesListBox.SelectedIndex;

            if (index < 0) return;

            TextureIdsInTextureListBox.Items.Clear();
            UVKeysListBox.Items.Clear();

            TextureSet textureSet = textureEditor.MapTextureSet[textureEditor.TextureFileNames[SelectFileComboBox.SelectedIndex]].ElementAt(index).Value;

            if (textureSet.GetTextureData(0) != null)
            {
                PictureBox1.Image = textureSet.GetTextureData(0).GetTextureBitmap(false);
                TextureImageNotFoundLabel.Visible = false;
                TextureFormatComboBox.SelectedIndex = (int)textureSet.GetTextureData(0).Format;
            }
            else
            {
                uint textureID = textureEditor.MapTextureSet[textureEditor.TextureFileNames[SelectFileComboBox.SelectedIndex]].ElementAt(index).Key;
                PictureBox1.Image = null;
                TextureImageNotFoundLabel.Text = $"Texture [{textureID}] data image was not found";
                TextureImageNotFoundLabel.Visible = true;
                TextureFormatComboBox.SelectedIndex = -1;
            }

            TextureIdTextBox.Text = textureSet.GetTextureId(0).ToString();

            textureSet.Textures.ForEach(texture => TextureIdsInTextureListBox.Items.Add($"ID: {texture.Key}"));

            TotalTickTextBox.Text = textureSet.TotalTick.ToString();
            CurrentTickTextBox.Text = textureSet.CurTick.ToString();
            MipBiasTextBox.Text = textureSet.MipBias.ToString();
            MipFilterComboBox.SelectedIndex = (int)textureSet.MipFilter;


            for (int i = 0; i < textureSet.UVKeys.Count; i++)
            {
                UVKeysListBox.Items.Add(i);
            }

            if (UVKeysListBox.Items.Count > 0)
            {
                UVKeysListBox.SelectedIndex = 0;
                UVKey key = textureSet.UVKeys[UVKeysListBox.SelectedIndex];

                KeyTickTextBox.Text = key.Tick.ToString();
                KeyUTextBox.Text = key.KeyU.ToString();
                KeyVTextBox.Text = key.KeyV.ToString();
                KeyRTextBox.Text = key.KeyR.ToString();
                KeySUTextBox.Text = key.KeySU.ToString();
                KeySVTextBox.Text = key.KeySV.ToString();
            }
            else
            {
                KeyTickTextBox.Text = "";
                KeyUTextBox.Text = "";
                KeyVTextBox.Text = "";
                KeyRTextBox.Text = "";
                KeySUTextBox.Text = "";
                KeySVTextBox.Text = "";
            }
        }

        private void AddTextureInFileButton_Click(object sender, EventArgs e)
        {
             Dictionary<uint, TextureSet> texturesInFile = GetTexturesInSelectedFile();
            uint textureID = texturesInFile.Last().Key + 1;

            TexturesListBox.Items.Add($"ID: {textureID}");

            TextureSet newTexture = new TextureSet();
            newTexture.PushTexture(textureID, new CT3DTexture());

            texturesInFile.Add(textureID, newTexture);
            UpdateTexturesInFileCountLabel();
            MessageBox.Show("New texture added");
        }

        private void DeleteTextureFromFileButton_Click(object sender, EventArgs e)
        {
            uint selectedTextureKey = GetSelectedTextureKey();
            Dictionary<uint, TextureSet> texturesInFile = GetTexturesInSelectedFile();
            if (texturesInFile.Remove(selectedTextureKey))
            {
                TexturesListBox.Items.RemoveAt(GetSelectedTextureListBoxIndex());
                UpdateTexturesInFileCountLabel();
                MessageBox.Show("Texture deleted");
            }
            else
            {
                MessageBox.Show("Failed to delete texture");
            }
        }

        private void DumpAllTexturesPngButton_Click(object sender, EventArgs e)
        {
            textureEditor.SaveTexturesInPNG();
        }

        private void TextureIdsInTextureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFileTexture = GetSelectedFileTexture();
            int selectedTextureIndex = GetSelectedTextureListBoxIndex();
            int selectedTextureIdIndex = GetSelectedIndexInTextureIdsInTextureListBox();

            if (selectedTextureIndex < 0 || selectedTextureIdIndex < 0) return;

            TextureSet textureSet = GetSelectedTextureData();

            if (textureSet == null) return;

            TextureIdTextBox.Text = textureSet.Textures[selectedTextureIdIndex].Key.ToString();
        }

        private void AddTextureIdInTexture_Click(object sender, EventArgs e)
        {
            GetSelectedTextureData().Textures.Add(new KeyValuePair<uint, CT3DTexture>(0, new CT3DTexture()));
            TextureIdsInTextureListBox.Items.Add("ID: 0");
        }

        private void DeleteTextureIdInTexture_Click(object sender, EventArgs e)
        {
            int selectedIndex = GetSelectedIndexInTextureIdsInTextureListBox();

            TextureIdsInTextureListBox.Items.RemoveAt(selectedIndex);
            GetSelectedTextureData().Textures.RemoveAt(selectedIndex);

            // TODO: If index == 0 change id in TextureListBox?
            MessageBox.Show("Texture ID deleted");
        }

        private void SaveTextureIdInTexture_Click(object sender, EventArgs e)
        {
            int selectedIndex = GetSelectedIndexInTextureIdsInTextureListBox();

            try
            {
                if (uint.TryParse(TextureIdTextBox.Text, out uint newTextureId))
                {

                    KeyValuePair<uint, CT3DTexture> selectedTextureID = GetSelectedTextureData().Textures[selectedIndex];
                    GetSelectedTextureData().Textures[selectedIndex] = new KeyValuePair<uint, CT3DTexture>(newTextureId, selectedTextureID.Value);
                    TextureIdsInTextureListBox.Items[selectedIndex] = $"ID: {TextureIdTextBox.Text}";

                    MessageBox.Show("Texture ID successfully saved");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save texture ID");
            }
        }

        private void SaveTextureInfoButton_Click(object sender, EventArgs e)
        {
            TextureSet texture = GetSelectedTextureData();

            try
            {
                if (uint.TryParse(TotalTickTextBox.Text, out uint totalTick))
                {
                    texture.TotalTick = totalTick;
                }

                if (uint.TryParse(CurrentTickTextBox.Text, out uint currentTick))
                {
                    texture.CurTick = currentTick;
                }

                if (float.TryParse(MipBiasTextBox.Text, out float mipBias))
                {
                    texture.MipBias = mipBias;
                }

                if (MipFilterComboBox.SelectedItem != null)
                {
                    texture.MipFilter = (uint)((KeyValuePair<string, TextureFilterType>)MipFilterComboBox.SelectedItem).Value;
                }

                if (TextureFormatComboBox.SelectedItem != null)
                {
                    // TODO: Check this value
                    texture.TextureOption = (byte)((KeyValuePair<string, TextureCompression>)TextureFormatComboBox.SelectedItem).Value;
                }
                MessageBox.Show("Values have been successfuly saved");
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save texture info");
            }
        }

        private byte[] BitmapToDDSBytes(Bitmap bitmap, byte bFormat)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                using (var magickImage = new MagickImage(memoryStream))
                {
                    // Configure DDS compression (e.g., DXT5)
                    magickImage.Settings.SetDefine("dds:compression", GetCompressionText(bFormat));

                    // Save DDS data to a byte array
                    using (var outputStream = new MemoryStream())
                    {
                        magickImage.Write(outputStream, MagickFormat.Dds);
                        return outputStream.ToArray();
                    }
                }
            }
        }

        private void AddImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.dds)|*.jpg;*.jpeg;*.png;*.bmp;*.dds";
                openFileDialog.Title = "Select an image file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog.FileName;
                    Bitmap bitmap = new Bitmap(fileName);

                    TextureSet textureSet = GetSelectedTextureData();

                    CT3DTexture texture = textureSet.GetTextureData(0);

                    byte[] ddsData = BitmapToDDSBytes(bitmap, texture.Format);

                    textureSet.Textures[0].Value.TextureData = ddsData;
                    Bitmap newBitmap = texture.GetTextureBitmap(true); // Reload the bitmap image from the DDS data

                    PictureBox1.Image = newBitmap;
                }
            }
        }

        private void SaveUVKeyButton_Click(object sender, EventArgs e)
        {
            UVKey uVKey = GetSelectedTextureData().UVKeys[GetSelectedUVKeyIndex()];

            if (uint.TryParse(KeyTickTextBox.Text, out uint tick))
            {
                uVKey.Tick = tick;
            }
            else
            {
                MessageBox.Show("Invalid tick value");
                return;
            }

            if (float.TryParse(KeyUTextBox.Text, out float u))
            {
                uVKey.KeyU = u;
            }
            else
            {
                MessageBox.Show("Invalid U value");
                return;
            }

            if (float.TryParse(KeyVTextBox.Text, out float v))
            {
                uVKey.KeyV = v;
            }
            else
            {
                MessageBox.Show("Invalid V value");
                return;
            }

            if (float.TryParse(KeyRTextBox.Text, out float r))
            {
                uVKey.KeyR = r;
            }
            else
            {
                MessageBox.Show("Invalid R value");
                return;
            }

            if (float.TryParse(KeySUTextBox.Text, out float su))
            {
                uVKey.KeySU = su;
            }
            else
            {
                MessageBox.Show("Invalid SU value");
                return;
            }

            if (float.TryParse(KeySVTextBox.Text, out float sv))
            {
                uVKey.KeySV = sv;
            }
            else
            {
                MessageBox.Show("Invalid SV value");
                return;
            }

            MessageBox.Show("UV Key saved");
            
        }

        private void UVKeysListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedUVKeyIndex = UVKeysListBox.SelectedIndex;
            int selectedTextureIndex = TexturesListBox.SelectedIndex;

            if (selectedUVKeyIndex < 0 || selectedTextureIndex < 0) return;

            TextureSet textureSet = textureEditor.MapTextureSet[textureEditor.TextureFileNames[SelectFileComboBox.SelectedIndex]].ElementAt(selectedTextureIndex).Value;

            UVKey key = textureSet.UVKeys[selectedUVKeyIndex];

            if (textureSet == null || selectedUVKeyIndex > textureSet.UVKeys.Count) return;

            KeyTickTextBox.Text = key.Tick.ToString();
            KeyUTextBox.Text = key.KeyU.ToString();
            KeyVTextBox.Text = key.KeyV.ToString();
            KeyRTextBox.Text = key.KeyR.ToString();
            KeySUTextBox.Text = key.KeySU.ToString();
            KeySVTextBox.Text = key.KeySV.ToString();
        }

        private void AddUVKeyButton_Click(object sender, EventArgs e)
        {
            int count = UVKeysListBox.Items.Count;
            UVKeysListBox.Items.Add(count);

            TextureSet textureSet = GetSelectedTextureData();
            textureSet.UVKeys.Add(new UVKey());
        }

        private void DeleteUVKeyButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = UVKeysListBox.SelectedIndex;

            try
            {
                GetSelectedTextureData().UVKeys.RemoveAt(selectedIndex);
                UVKeysListBox.Items.RemoveAt(selectedIndex);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete UV Key");
            }
        }

        private void UpdateTexturesInFileCountLabel()
        {
            TexturesInFileCountLabel.Visible = true;
            TexturesInFileCountLabel.Text = "Textures in file: " + TexturesListBox.Items.Count.ToString();
        }

        private int GetSelectedIndexInTextureIdsInTextureListBox()
        {
            return TextureIdsInTextureListBox.SelectedIndex;
        }

        private TextureSet GetSelectedTextureData()
        {
            return GetSelectedTextureKVP().Value;
        }

        private uint GetSelectedTextureKey()
        {
            return GetSelectedTextureKVP().Key;
        }

        private int GetSelectedTextureListBoxIndex()
        {
            return TexturesListBox.SelectedIndex;
        }

        private KeyValuePair<uint, TextureSet> GetSelectedTextureKVP()
        {
            string selectedFileTexture = GetSelectedFileTexture();

            return textureEditor.MapTextureSet[selectedFileTexture].ElementAt(GetSelectedTextureListBoxIndex());
        }

        private string GetSelectedFileTexture()
        {
            return textureEditor.TextureFileNames[SelectFileComboBox.SelectedIndex];
        }

        private int GetSelectedUVKeyIndex()
        {
            return UVKeysListBox.SelectedIndex;
        }

        private Dictionary<uint, TextureSet> GetTexturesInSelectedFile()
        {
            string fileName = GetSelectedFileTexture();

            return textureEditor.MapTextureSet[fileName];
        }

        private string GetCompressionText(byte bFormat)
        {
            switch (bFormat)
            {
                case (byte)TextureCompression.DXT1:
                    return "dxt1";
                case (byte)TextureCompression.DXT2:
                    return "dxt2";
                case (byte)TextureCompression.DXT3:
                    return "dxt3";
                case (byte)TextureCompression.DXT4:
                    return "dxt4";
                case (byte)TextureCompression.DXT5:
                    return "dxt5";
                case (byte)TextureCompression.NON_COMP:
                    return "21";
                default:
                    return "dxt3";
            }
        }
    }
}
