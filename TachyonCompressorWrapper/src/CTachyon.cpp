#pragma once

#include "pch.h"

#include "CTachyon.h"
#include "Device.h"
#include "zlib.h"


bool GenerateTextureDDS(Device* pDevice, BYTE* pData, DWORD dwSize, BYTE bFormat, BYTE** ppGeneratedDataOut, DWORD* pGeneratedSizeOut, DWORD* pOriginalSizeOut)
{
	HRESULT hResult;
	LPDIRECT3DTEXTURE9 pTex;

	UINT nWidth = 0;
	UINT nHeight = 0;
	DWORD dwFilter = D3DX_FILTER_NONE;
	D3DFORMAT d3dFormat = D3DFMT_DXT3;

	D3DXIMAGE_INFO imageInfo;

	hResult = D3DXGetImageInfoFromFileInMemory(pData, dwSize, &imageInfo);

	if (hResult != D3D_OK) return false;

	nWidth = imageInfo.Width;
	nHeight = imageInfo.Height;

	switch (bFormat)
	{
	case NON_COMP: d3dFormat = D3DFMT_A8R8G8B8; break;
	case DXT1:     d3dFormat = D3DFMT_DXT1; break;
	case DXT2:     d3dFormat = D3DFMT_DXT2; break;
	case DXT3:     d3dFormat = D3DFMT_DXT3; break;
	case DXT4:     d3dFormat = D3DFMT_DXT4; break;
	case DXT5:     d3dFormat = D3DFMT_DXT5; break;
	}

	hResult = D3DXCreateTextureFromFileInMemoryEx(
		pDevice->GetDevice(),
		pData,
		dwSize,
		nWidth,
		nHeight,
		D3DX_DEFAULT,
		0,
		d3dFormat,
		D3DPOOL_MANAGED,
		dwFilter,
		D3DX_DEFAULT,
		NULL,
		NULL,
		NULL,
		&pTex
	);

	if (hResult != D3D_OK) return false;

	LPD3DXBUFFER pBuffer = NULL;
	hResult = D3DXSaveTextureToFileInMemory(&pBuffer, D3DXIFF_DDS, pTex, NULL);

	if (hResult != D3D_OK) return false;

	*pOriginalSizeOut = pBuffer->GetBufferSize();
	*pGeneratedSizeOut = *pOriginalSizeOut + *pOriginalSizeOut / 10 + 12;

	*ppGeneratedDataOut = new BYTE[*pGeneratedSizeOut];
	compress2(*ppGeneratedDataOut, pGeneratedSizeOut, (LPBYTE)pBuffer->GetBufferPointer(), *pOriginalSizeOut, 9);
	pBuffer->Release();

	pTex->Release();

	return true;
}

void* CreateNewFile(const char* fileName) { return new CFile(fileName, CFile::modeCreate | CFile::modeWrite | CFile::typeBinary); }

void CloseFile(CFile* file)
{
	if (file) file->Close();
}

DWORD GetFilePosition(void* pFile)
{
	CFile* p = reinterpret_cast<CFile*>(pFile);
	if (p) return (DWORD)p->GetPosition();
}

void DestroyFile(void* pFile)
{
	CFile* p = reinterpret_cast<CFile*>(pFile);
	if (p)
	{
		p->Close();
		delete p;
	}
}

BYTE* UncompressTextureData(BYTE* pData, DWORD dwCompressedSize, DWORD dwOriginalSize)
{
	BYTE* pUncompressedData = new BYTE[dwOriginalSize];

	if (uncompress(pUncompressedData, &dwOriginalSize, pData, dwCompressedSize) != Z_OK)
	{
		delete[] pUncompressedData;
		pUncompressedData = NULL;

		return NULL;
	}

	return pUncompressedData;
}

void FreeUncompressedTextureData(BYTE* pData)
{
	if (pData)
	{
		delete[] pData;
		pData = NULL;
	}
}

CTachyonCompressor::CTachyonCompressor()
	: m_dwCompressedSize(0), m_pCompressedData(NULL)
{
}
// --------------------------------------------------------------
CTachyonCompressor::~CTachyonCompressor()
{
	Release();
}
// ==============================================================
void CTachyonCompressor::Compress()
{
	if (m_pCompressedData)
	{
		delete[] m_pCompressedData;
		m_pCompressedData = NULL;
	}

	m_dwCompressedSize = 0;

	UINT nSize = (UINT)m_vData.size();
	m_dwCompressedSize = nSize + nSize / 10 + 12;
	m_pCompressedData = new BYTE[m_dwCompressedSize];

	compress2(m_pCompressedData, &m_dwCompressedSize, &m_vData[0], nSize, 9);
}
// --------------------------------------------------------------
void CTachyonCompressor::Release()
{
	m_vData.clear();

	if (m_pCompressedData)
	{
		delete[] m_pCompressedData;
		m_pCompressedData = NULL;
	}

	m_dwCompressedSize = 0;
}
// ==============================================================
void CTachyonCompressor::Write(LPCVOID lpBuf, UINT nCount)
{
	size_t nPos = m_vData.size();
	m_vData.resize(nPos + nCount);
	memcpy(&m_vData[nPos], lpBuf, nCount);
}
// ==============================================================
void CTachyonCompressor::ToFile(CFile* pFile)
{
	if (!pFile) return;

	Compress();

	DWORD dwOriginSize = GetLength();

	pFile->Write(&dwOriginSize, sizeof(dwOriginSize));
	pFile->Write(&m_dwCompressedSize, sizeof(m_dwCompressedSize));
	pFile->Write(m_pCompressedData, m_dwCompressedSize);
}
// ==============================================================


// CTachyonUncompressor
// ==============================================================
CTachyonUncompressor::CTachyonUncompressor()
	: m_dwSize(0), m_pData(NULL)
{
}
// --------------------------------------------------------------
CTachyonUncompressor::~CTachyonUncompressor()
{
	Release();
}
// ==============================================================
void CTachyonUncompressor::Uncompress(LPCVOID pCompressedData, DWORD dwCompressedSize, DWORD dwOriginalSize)
{
	Release();

	m_dwSize = dwOriginalSize;
	m_pData = new BYTE[dwOriginalSize];
	uncompress(m_pData, &m_dwSize, (const Bytef*)pCompressedData, dwCompressedSize);
}
// --------------------------------------------------------------
void CTachyonUncompressor::Release()
{
	if (m_pData)
	{
		delete[] m_pData;
		m_pData = NULL;
	}

	m_dwSize = 0;
	m_dwPos = 0;
}
// ==============================================================
UINT CTachyonUncompressor::Read(LPVOID lpBuf, UINT nCount)
{
	if (m_dwPos > m_dwSize)
		return 0;

	if (m_dwPos + nCount >= m_dwSize)
		nCount = m_dwSize - m_dwPos;

	if (nCount)
	{
		memcpy(lpBuf, m_pData + m_dwPos, nCount);
		m_dwPos += nCount;
	}

	return m_dwPos;
}
// --------------------------------------------------------------
DWORD CTachyonUncompressor::Seek(DWORD lOff, UINT nFrom)
{
	m_dwPos = nFrom + lOff;
	return m_dwPos;
}
// ==============================================================
DWORD CTachyonUncompressor::FromFile(const char* fileName)
{
	CFile file(fileName, CFile::modeRead | CFile::typeBinary);

	DWORD dwLength = DWORD(file.GetLength());
	DWORD dwPOS = DWORD(file.GetPosition());

	m_dwFileLength = dwLength;

	return dwLength;
}
