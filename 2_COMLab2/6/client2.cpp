#include <Windows.h>
#include "IKlasa2.h"
#include <stdio.h>
#include <comdef.h>

int main()
{

	HRESULT rv;
	rv = CoInitializeEx(NULL, COINIT_MULTITHREADED);

	printf("\n-------------------\n");
	printf("CoInitializeEx(NULL, COINIT_APARTMENTTHREADED)\n");
	_com_error err(rv);
	wprintf(L" -> %s\n", err.ErrorMessage());

	printf("\n-------------------\n");
	wchar_t IID_IKlasa_GUID[64] = { 0 };
	StringFromGUID2(IID_IKlasa2, IID_IKlasa_GUID, 64);
	wprintf(L"IID_IKlasa2 = %s\n", IID_IKlasa_GUID);

	printf("\n-------------------\n");
	wchar_t CLSID_Klasa_GUID[64] = { 0 };
	StringFromGUID2(CLSID_Klasa2, CLSID_Klasa_GUID, 64);
	wprintf(L"CLSID_Klasa2 = %s\n", CLSID_Klasa_GUID);

	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

	IKlasa2 *klasaTestowa2;
	rv = CoCreateInstance(CLSID_Klasa2, NULL, CLSCTX_LOCAL_SERVER, IID_IKlasa2, (void**)&klasaTestowa2);
	printf("\n-------------------\n");
	printf("CoCreateInstance(CLSID_Klasa2, NULL, CLSCTX_LOCAL_SERVER, IID_IKlasa2, (void**)&klasaTestowa2)\n");
	_com_error err2(rv);
	wprintf(L" -> %s\n", err2.ErrorMessage());

	if (rv == S_OK) {
		klasaTestowa2->Test(42);
		printf("\n-------------------\n");
		printf("klasaTestowa2->Test(42)");
		_com_error err3(rv);
		wprintf(L" -> %s\n", err3.ErrorMessage());

		klasaTestowa2->Release();
		printf("\n-------------------\n");
		printf("klasaTestowa2->Release()");
		_com_error err4(rv);
		wprintf(L" -> %s\n", err3.ErrorMessage());
	}

	CoUninitialize();
	return 0;
}