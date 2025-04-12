#include "pch.h"

#include "Device.h"


Device::Device()
{
	Init();
}

Device::~Device()
{
	if (m_pD3Device)
	{
		m_pD3Device->Release();
		m_pD3Device = nullptr;
	}
	if (m_pD3D)
	{
		m_pD3D->Release();
		m_pD3D = nullptr;
	}
}

void Device::Init()
{
	HINSTANCE hInstance = GetModuleHandle(NULL);
	HWND hWnd = CreateHiddenWindow(hInstance);

	D3DPRESENT_PARAMETERS d3dParameters;
	ZeroMemory(&d3dParameters, sizeof(d3dParameters));

	d3dParameters.Windowed = TRUE;
	d3dParameters.SwapEffect = D3DSWAPEFFECT_DISCARD;

	if (NULL == (m_pD3D = Direct3DCreate9(D3D_SDK_VERSION)))
		return;

	if (FAILED(m_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd,
		D3DCREATE_SOFTWARE_VERTEXPROCESSING,
		&d3dParameters, &m_pD3Device)))
		return;
}

HWND Device::CreateHiddenWindow(HINSTANCE hInstance)  
{  
   WNDCLASSEXW wc = { sizeof(WNDCLASSEXW), CS_CLASSDC, DefWindowProcW, 0L, 0L,  
                       hInstance, NULL, NULL, NULL, NULL, L"DXWindowClass", NULL };  
   RegisterClassExW(&wc); 

   return CreateWindowW(wc.lpszClassName, L"Hidden Window",  
                        WS_OVERLAPPEDWINDOW, 0, 0, 100, 100,  
                        NULL, NULL, hInstance, NULL);  
}
