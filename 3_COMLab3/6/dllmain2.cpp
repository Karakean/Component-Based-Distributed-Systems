#include<windows.h>
#include"fabryka2.h"
#include"IKlasa2.h"

volatile ULONG dllUsageCount = 0;

HRESULT __stdcall DllGetClassObject(REFCLSID cls, REFIID iid, void **ptr) {
	Fabryka2* fabryka2;

	if (ptr == NULL) // brak zaczepu na fabryke
		return E_INVALIDARG;
	if (cls != CLSID_Klasa2) return CLASS_E_CLASSNOTAVAILABLE; // nie ta klasa
	if (iid != IID_IUnknown && iid != IID_IClassFactory) return E_NOINTERFACE; // iid nie jest comem i fabryka

	fabryka2 = new Fabryka2();
	if (fabryka2 == NULL)
		return E_OUTOFMEMORY; //nie udalo sie zaalokowac fabryki

	HRESULT getInterfaceResult = fabryka2->QueryInterface(iid, ptr);
	if (FAILED(getInterfaceResult)) { //jesli wystapil blad przy pobieraniu interfejsu do fabryki
		delete fabryka2;	//usuwamy obj fabryka
		*ptr = NULL;
	}
	return getInterfaceResult;
};


HRESULT __stdcall DllCanUnloadNow() {
	if (dllUsageCount > 0) //spr czy mozna zwolnic dll
		return S_FALSE;
	else
		return S_OK;
};


//HRESULT __stdcall DllRegisterServer() {
//	return E_NOTIMPL;
//	};
//
//
//HRESULT __stdcall DllUnregisterServer() {
//	return E_NOTIMPL;
//	};


BOOL WINAPI DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
	switch (ul_reason_for_call) {
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	};
	return TRUE;
};
