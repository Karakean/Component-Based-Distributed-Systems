#include<windows.h>
#include<stdio.h>
#include"IKlasa3.h"

int main() {
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

	IKlasa3* klasa3Ptr = NULL;
	HRESULT classGetInstanceResult = CoCreateInstance(CLSID_Klasa3, NULL, CLSCTX_INPROC_SERVER, IID_IKlasa3, (void**)&klasa3Ptr);

	if (!FAILED(classGetInstanceResult)) {
		klasa3Ptr->Test("klasa3 stowrzona poprawnie (instancja pobrana), indeks: 184865");
		klasa3Ptr->Release();
	}
	else {
		printf("klasa3 nie dziala (instancja nie pobrana)");
	}

	return 0;
};
