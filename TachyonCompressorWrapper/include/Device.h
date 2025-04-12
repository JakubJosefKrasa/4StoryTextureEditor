#pragma once

#include "pch.h"

class Device
{
public:
	Device();
	~Device();
	LPDIRECT3DDEVICE9 GetDevice() const { return m_pD3Device; }

private:
	LPDIRECT3DDEVICE9 m_pD3Device;
	LPDIRECT3D9 m_pD3D;


private:
	void Init();
	HWND CreateHiddenWindow(HINSTANCE hInstance);
};