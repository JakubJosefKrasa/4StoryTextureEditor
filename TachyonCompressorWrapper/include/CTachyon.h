#pragma once

#include "pch.h"
#include "Device.h"

enum
{
	DXT1,
	DXT2,
	DXT3,
	DXT4,
	DXT5,
	NON_COMP
};

bool GenerateTextureDDS(Device* pDevice, BYTE* pData, DWORD dwSize, BYTE bFormat, BYTE** ppGeneratedDataOut, DWORD* pGeneratedSizeOut, DWORD* pOriginalSizeOut);

void* CreateNewFile(const char* fileName);

void CloseFile(CFile* file);

DWORD GetFilePosition(void* pFile);

void DestroyFile(void* pFile);

BYTE* UncompressTextureData(BYTE* pData, DWORD dwCompressedSize, DWORD dwOriginalSize);

void FreeUncompressedTextureData(BYTE* pData);

class CTachyonCompressor
{
protected:
	std::vector<BYTE>	m_vData;

	DWORD				m_dwCompressedSize;
	LPBYTE				m_pCompressedData;

public:
	void Compress();
	void Release();

	void Write(LPCVOID lpBuf, UINT nCount);

	void ToFile(CFile* pFile);

	LPCVOID GetData() const { return m_vData.empty() ? NULL : (&m_vData[0]); }
	LPCVOID GetCompressedData() const { return m_pCompressedData; }

	DWORD GetLength() const { return (DWORD)m_vData.size(); }
	DWORD GetCompressedLength() const { return m_dwCompressedSize; }



public:
	CTachyonCompressor();
	~CTachyonCompressor();
};

class CTachyonUncompressor
{
protected:
	DWORD	m_dwSize;
	LPBYTE	m_pData;
	DWORD	m_dwPos;

	DWORD   m_dwFileLength;

public:
	void Uncompress(LPCVOID pCompressedData, DWORD dwCompressedSize, DWORD dwOriginalSize);
	void Release();

	UINT Read(LPVOID lpBuf, UINT nCount);
	DWORD Seek(DWORD lOff, UINT nFrom);

	DWORD FromFile(const char* fileName);

	LPCVOID GetCurData() const { return m_pData + m_dwPos; }
	LPCVOID GetData() const { return m_pData; }
	DWORD GetLength() const { return m_dwSize; }
	DWORD GetFilePosition() const { return m_dwPos; }
	DWORD GetFileLength() const { return m_dwFileLength; }

public:
	CTachyonUncompressor();
	~CTachyonUncompressor();
};