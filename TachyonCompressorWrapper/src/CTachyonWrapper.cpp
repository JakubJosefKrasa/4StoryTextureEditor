// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

#include "CTachyon.h"
#include "CTachyonWrapper.h"
#include "Device.h"

extern "C"
{
    TACHYONZLIB_EXPORT_API bool DoGenerateTextureDDS(void* pDevice, void* pData, DWORD dwSize, BYTE bFormat, void** ppGeneratedDataOut, void* pGeneratedSizeOut, void* pOriginalSizeOut)
    {
        Device* p = reinterpret_cast<Device*>(pDevice);

        if (!p || !pData || !ppGeneratedDataOut || !pGeneratedSizeOut || !pOriginalSizeOut) return false;

        GenerateTextureDDS(p, (BYTE*)pData, dwSize, bFormat, (BYTE**)ppGeneratedDataOut, (DWORD*)pGeneratedSizeOut, (DWORD*)pOriginalSizeOut);
    }

    TACHYONZLIB_EXPORT_API void* DoCreateNewFile(const char* fileName)
    {
        return CreateNewFile(fileName);
    }

    TACHYONZLIB_EXPORT_API void DoCloseFile(void* file)
    {
        CloseFile(reinterpret_cast<CFile*>(file));
    }

    TACHYONZLIB_EXPORT_API unsigned int DoGetFilePosition(void* pFile)
    {
        return GetFilePosition(pFile);
    }

    TACHYONZLIB_EXPORT_API void DoDestroyFile(void* pFile)
    {
        DestroyFile(pFile);
    }

    TACHYONZLIB_EXPORT_API void* DoUncompressTextureData(void* pData, DWORD dwCompressedSize, DWORD dwOriginalSize)
    {
		return UncompressTextureData((BYTE*)pData, dwCompressedSize, dwOriginalSize);
    }

    TACHYONZLIB_EXPORT_API void DoFreeUncompressedTextureData(void* pData)
    {
		FreeUncompressedTextureData((BYTE*)pData);
    }

    TACHYONZLIB_EXPORT_API void* CompressorCreate()
    {
		return reinterpret_cast<void*>(new CTachyonCompressor());
    }

    TACHYONZLIB_EXPORT_API void CompressorDestroy(void* pCompressor)
    {
		CTachyonCompressor* p = reinterpret_cast<CTachyonCompressor*>(pCompressor);
        if (p) delete p;
    }

    TACHYONZLIB_EXPORT_API void CompressorCompress(void* pCompressor)
    {
		CTachyonCompressor* p = reinterpret_cast<CTachyonCompressor*>(pCompressor);
		if (p) p->Compress();
    }

    TACHYONZLIB_EXPORT_API void CompressorWrite(void* pCompressor, const void* lpBuf, unsigned int nCount)
    {
		CTachyonCompressor* p = reinterpret_cast<CTachyonCompressor*>(pCompressor);
		if (p) p->Write(lpBuf, nCount);
    }

    TACHYONZLIB_EXPORT_API const void* CompressorGetData(void* pCompressor)
    {
		CTachyonCompressor* p = reinterpret_cast<CTachyonCompressor*>(pCompressor);
		if (p) return p->GetData();
    }

    TACHYONZLIB_EXPORT_API unsigned int CompressorGetCompressedLength(void* pCompressor)
    {
		CTachyonCompressor* p = reinterpret_cast<CTachyonCompressor*>(pCompressor);
		if (p) return p->GetCompressedLength();
    }

    TACHYONZLIB_EXPORT_API void CompressorToFile(void* pCompressor, void* pFile)
    {
		CTachyonCompressor* p = reinterpret_cast<CTachyonCompressor*>(pCompressor);
		if (!p || !pFile) return;

		p->ToFile((CFile*) pFile);
    }


    TACHYONZLIB_EXPORT_API void* UncompressorCreate()
    {
		return reinterpret_cast<void*>(new CTachyonUncompressor());
    }

    TACHYONZLIB_EXPORT_API void UncompressorDestroy(void* pUncompressor)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
        if (p)
        {
            p->Release();
            delete p;
        }
    }

    TACHYONZLIB_EXPORT_API void UncompressorUncompress(void* pUncompressor, const void* pCompressedData, unsigned int dwCompressedSize, unsigned int dwOriginalSize)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (p && pCompressedData) p->Uncompress(pCompressedData, dwCompressedSize, dwOriginalSize);
    }

    TACHYONZLIB_EXPORT_API unsigned int UncompressorRead(void* pUncompressor, void* lpBuf, unsigned int nCount)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (p && lpBuf) return p->Read(lpBuf, nCount);
    }

    TACHYONZLIB_EXPORT_API unsigned int UncompressorSeek(void* pUncompressor, unsigned int lOff, unsigned int nFrom)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (p) return p->Seek(lOff, nFrom);
    }

    TACHYONZLIB_EXPORT_API unsigned int UncompressorFromFile(void* pUncompressor, const char* fileName)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (!p || !fileName)
			return 0;
		return p->FromFile(fileName);
    }

    TACHYONZLIB_EXPORT_API void UncompressorGetData(void* pUncompressor, const void* buffer)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (p) buffer = p->GetCurData();
    }

    TACHYONZLIB_EXPORT_API unsigned int UncompressorGetLength(void* pUncompressor)
    {
        CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (p) return p->GetLength();
    }

    TACHYONZLIB_EXPORT_API unsigned int UncompressorGetFilePosition(void* pUncompressor)
    {
        CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
        if (p) return p->GetFilePosition();
    }

    TACHYONZLIB_EXPORT_API unsigned int UncompressorGetFileLength(void* pUncompressor)
    {
		CTachyonUncompressor* p = reinterpret_cast<CTachyonUncompressor*>(pUncompressor);
		if (p) return p->GetFileLength();
    }
}

