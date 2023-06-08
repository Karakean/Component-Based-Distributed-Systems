
#include <iostream>
#include <Windows.h>
#include "fabryka2.h"
#include "IKlasa2.h"

volatile ULONG numberOfUsedInstances = 0;
int main(int argc, char** argv)
{
	CoInitializeEx(NULL, COINIT_MULTITHREADED);
	if (argc > 1) {
		for (int i = 0; i < argc; i++) {
			if (_stricmp(argv[i], "\Embedding") == 0 || _stricmp(argv[i], "-Embedding") == 0) {
				DWORD id;
				HRESULT rv;
				fabryka2 *f = new fabryka2();
				f->AddRef();
				rv = CoRegisterClassObject(CLSID_Klasa2, (IUnknown*)f, CLSCTX_LOCAL_SERVER, REGCLS_MULTIPLEUSE, &id);
				if (FAILED(rv)) { CoUninitialize(); return 0; };
				Sleep(5000);
				do {
					Sleep(1000);
					std::cout << "We still have " << numberOfUsedInstances << " dependencies" << std::endl;
				} while (numberOfUsedInstances > 1);
				CoRevokeClassObject(id);
				f->Release();
				break;
			}
		}
	}

	CoUninitialize();
}

