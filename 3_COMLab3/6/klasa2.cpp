#include<windows.h>
#include<stdio.h>
#include <wchar.h>
#include"klasa2.h"

extern volatile ULONG UCKlasa = 0;

Klasa2::Klasa2() {
	InterlockedIncrement(&UCKlasa);
	reference_counter = 0;
};


Klasa2::~Klasa2() {
	InterlockedDecrement(&UCKlasa);
};


HRESULT STDMETHODCALLTYPE Klasa2::QueryInterface(REFIID iid, void **ptr) {
	if (ptr == NULL) return E_POINTER;

	*ptr = NULL;
	if (iid == IID_IUnknown)
		*ptr = this;
	else if (iid == IID_IKlasa2)
		*ptr = this;

	if (*ptr != NULL) {
		this->AddRef();
		return S_OK;
	}

	return E_NOINTERFACE;
};


ULONG STDMETHODCALLTYPE Klasa2::AddRef() {
	ULONG result = InterlockedIncrement(&reference_counter);
	return result;
};


ULONG STDMETHODCALLTYPE Klasa2::Release() {
	ULONG result = InterlockedDecrement(&reference_counter);
	if (result == 0) // kiedy uwolnilismy ostatni wskaznik na ten obiekt
		delete this;
	return result;
};


HRESULT STDMETHODCALLTYPE Klasa2::Test(const wchar_t *napis) {
	wprintf(napis);
	return S_OK;
};
