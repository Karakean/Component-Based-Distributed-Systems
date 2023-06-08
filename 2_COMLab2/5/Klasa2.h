#pragma once
#include <Windows.h>
#include "IKlasa2.h"

class Klasa2 : public IKlasa2
{
public:
	Klasa2();
	~Klasa2();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID interfaceIdentifier, void **interfacePointer);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE Test(int i);


private:
	volatile ULONG m_ref;
};

