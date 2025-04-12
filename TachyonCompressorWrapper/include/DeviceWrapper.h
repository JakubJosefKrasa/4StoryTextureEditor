#pragma once

#define TACHYONZLIB_EXPORT_API __declspec(dllexport)

extern "C"
{
	TACHYONZLIB_EXPORT_API void* DeviceCreate();
	TACHYONZLIB_EXPORT_API void DeviceDestroy(void* pDevice);
}