#include<windows.h>
#include<stdio.h>
#include"IKlasa.h"

int main() {
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

	IKlasa* klasaPtr = NULL;
	HRESULT classGetInstanceResult = CoCreateInstance(CLSID_Klasa, NULL, CLSCTX_INPROC_SERVER, IID_IKlasa, (void**)&klasaPtr);

	if (!FAILED(classGetInstanceResult)) {
		klasaPtr->Test("klasa stowrzona poprawnie (instancja pobrana), indeks: 184865");
		klasaPtr->Release();
	}
	else {
		printf("klasa nie dziala (instancja nie pobrana)");
	}

	return 0;
};
