using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextureEditor
{
    class CTachyonWrapperFunctions
    {
        private const string DllName = "TachyonCompressorWrapper.dll";

        public const uint CFILE_CURRENT = 1;

        // Device functions
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr DeviceCreate();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void DeviceDestroy(IntPtr device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr DoCreateNewFile([MarshalAs(UnmanagedType.LPStr)] string fileName);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint DoGetFilePosition(IntPtr file);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void DoDestroyFile(IntPtr file);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr DoUncompressTextureData(byte[] data, uint compressedSize, uint originalSize);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void DoFreeUncompressedTextureData(IntPtr data);


        // Compressor functions

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool DoGenerateTextureDDS(IntPtr device, byte[] data, uint size, byte format, out IntPtr generatedData, out uint generatedSize, out uint originalSize);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr CompressorCreate();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void CompressorDestroy(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void CompressorCompress(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void CompressorWrite(IntPtr compressor, byte[] buffer, uint count);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr CompressorGetData(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint CompressorGetCompressedLength(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint CompressorToFile(IntPtr handle, IntPtr file);


        // Uncompressor functions
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr UncompressorCreate();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void UncompressorDestroy(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void UncompressorUncompress(IntPtr compressor, byte[] compressedData, uint compressedSize, uint originalSize);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint UncompressorRead(IntPtr compressor, [Out] byte[] buffer, uint count);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint UncompressorSeek(IntPtr compressor, uint off, uint from);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint UncompressorFromFile(IntPtr compressor, [MarshalAs(UnmanagedType.LPStr)] string fileName);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr UncompressorGetData(IntPtr compressor, [Out] byte[] buffer);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint UncompressorGetLength(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint UncompressorGetFilePosition(IntPtr compressor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint UncompressorGetFileLength(IntPtr compressor);
    }
}
