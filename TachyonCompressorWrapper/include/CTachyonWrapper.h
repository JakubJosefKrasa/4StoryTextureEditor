#pragma once

#define TACHYONZLIB_EXPORT_API __declspec(dllexport)

extern "C"
{
	TACHYONZLIB_EXPORT_API bool DoGenerateTextureDDS(void* pDevice, void* pData, DWORD dwSize, BYTE bFormat, void** ppGeneratedDataOut, void* pGeneratedSizeOut, void* pOriginalSizeOut);

	TACHYONZLIB_EXPORT_API void* DoCreateNewFile(const char* fileName);

	TACHYONZLIB_EXPORT_API unsigned int DoGetFilePosition(void* pFile);

	TACHYONZLIB_EXPORT_API void DoDestroyFile(void* pFile);

	TACHYONZLIB_EXPORT_API void* DoUncompressTextureData(void* pData, DWORD dwCompressedSize, DWORD dwOriginalSize);

	TACHYONZLIB_EXPORT_API void DoFreeUncompressedTextureData(void* pData);

	TACHYONZLIB_EXPORT_API void* CompressorCreate();
	TACHYONZLIB_EXPORT_API void CompressorDestroy(void* pCompressor);
	TACHYONZLIB_EXPORT_API void CompressorCompress(void* pCompressor);
	TACHYONZLIB_EXPORT_API void CompressorWrite(void* pCompressor, const void* lpBuf, unsigned int nCount);
	TACHYONZLIB_EXPORT_API const void* CompressorGetData(void* pCompressor);
	TACHYONZLIB_EXPORT_API unsigned int CompressorGetCompressedLength(void* pCompressor);
	TACHYONZLIB_EXPORT_API void CompressorToFile(void* pCompressor, void* pFile);

	TACHYONZLIB_EXPORT_API void* UncompressorCreate();
	TACHYONZLIB_EXPORT_API void UncompressorDestroy(void* pUncompressor);
	TACHYONZLIB_EXPORT_API void UncompressorUncompress(void* pUncompressor, const void* pCompressedData, unsigned int dwCompressedSize, unsigned int dwOriginalSize);
	TACHYONZLIB_EXPORT_API unsigned int UncompressorRead(void* pUncompressor, void* lpBuf, unsigned int nCount);
	TACHYONZLIB_EXPORT_API unsigned int UncompressorSeek(void* pUncompressor, unsigned int lOff, unsigned int nFrom);
	TACHYONZLIB_EXPORT_API unsigned int UncompressorFromFile(void* pUncompressor, const char* fileName);
	TACHYONZLIB_EXPORT_API void UncompressorGetData(void* pUncompressor, const void* buffer);
	TACHYONZLIB_EXPORT_API unsigned int UncompressorGetLength(void* pUncompressor);
	TACHYONZLIB_EXPORT_API unsigned int UncompressorGetFilePosition(void* pUncompressor);
	TACHYONZLIB_EXPORT_API unsigned int UncompressorGetFileLength(void* pUncompressor);
}