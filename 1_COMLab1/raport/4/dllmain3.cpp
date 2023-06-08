#include<windows.h>
#include"klasa3.h"
#include"fabryka3.h"


volatile int usageCount = 0;


STDAPI DllGetClassObject(REFCLSID cls, REFIID iid, void **rv) {
	KlasaFactory *f;
	if(cls != CLSID_Klasa3) { return CLASS_E_CLASSNOTAVAILABLE; };
	if(iid == IID_IClassFactory) {
		f = new KlasaFactory();
		f->AddRef();
		*rv = f;
		return S_OK;
		};
	return CLASS_E_CLASSNOTAVAILABLE;
	};

/*
STDAPI DllRegisterServer() {
	};


STDAPI DllUnregisterServer() {
	};
*/


STDAPI DllCanUnloadNow() {
	return usageCount > 0 ? FALSE : TRUE;
	};


BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
	switch(ul_reason_for_call) {
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
		}
	return TRUE;
	};
