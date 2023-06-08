#ifndef IKLASA_H_2
#define IKLASA_H_2

#include<windows.h>
// {442A7716-E106-4279-9BC0-9C1B4ED1779B}
DEFINE_GUID(IID_IKlasa2, 0x442a7716, 0xe106, 0x4279, 0x9b, 0xc0, 0x9c, 0x1b, 0x4e, 0xd1, 0x77, 0x9b);
// {3F7A6746-5F7C-4A1C-BA61-E2C5C79CC096}
DEFINE_GUID(CLSID_Klasa2, 0x67417a67, 0x96f, 0x45ee, 0x80, 0x63, 0x4d, 0x20, 0x6f, 0x59, 0x85, 0xb5);

// {8A558A37-E650-4DF6-9A26-1B13A7AA8D9E}
class IKlasa2 : public IUnknown {
public:
	virtual HRESULT STDMETHODCALLTYPE Test(const wchar_t *napis) = 0;
};

#endif
