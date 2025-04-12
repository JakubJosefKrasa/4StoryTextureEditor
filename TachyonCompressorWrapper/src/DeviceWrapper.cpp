#include "pch.h"

#include "Device.h"
#include "DeviceWrapper.h"

extern "C"
{
	TACHYONZLIB_EXPORT_API void* DeviceCreate()
	{
		return reinterpret_cast<void*>(new Device());
	}

	TACHYONZLIB_EXPORT_API void DeviceDestroy(void* pDevice)
	{
		Device* p = reinterpret_cast<Device*>(pDevice);
		if (p) delete p;
	}
}