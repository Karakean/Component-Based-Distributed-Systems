#ifndef KLASA_H_2
#define KLASA_H_2

#include<windows.h>
#include"IKlasa2.h"

// {8DBF9031-3A92-4A2D-9A07-E68705C6527B}
class Klasa2 : public IKlasa2 {
public:
	Klasa2();
	~Klasa2();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **rv);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE Test(const wchar_t *napis);
private:
	volatile ULONG reference_counter;
};

#endif
