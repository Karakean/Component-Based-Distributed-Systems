#include"fabryka2.h"
#include"klasa2.h"

extern volatile ULONG UCFabryka = 0;


Fabryka2::Fabryka2() {
	InterlockedIncrement(&UCFabryka);
	reference_counter = 0;
};


Fabryka2::~Fabryka2() {
	InterlockedDecrement(&UCFabryka);
};


HRESULT STDMETHODCALLTYPE Fabryka2::QueryInterface(REFIID id, void **ptr) {
	if (ptr == NULL) return E_POINTER;

	*ptr = NULL;
	if (id == IID_IUnknown)
		*ptr = this;
	else if (id == IID_IClassFactory)
		*ptr = this;

	if (*ptr != NULL) {
		this->AddRef();
		return S_OK;
	}

	return E_NOINTERFACE;
};


ULONG STDMETHODCALLTYPE Fabryka2::AddRef() {
	ULONG result = InterlockedIncrement(&reference_counter);
	return result;
};


ULONG STDMETHODCALLTYPE Fabryka2::Release() {
	ULONG result = InterlockedDecrement(&reference_counter);
	if (result == 0) // kiedy uwolnilismy ostatni wskaznik na ten obiekt
		delete this;
	return result;
};


HRESULT STDMETHODCALLTYPE Fabryka2::LockServer(BOOL v) {
	if (v)
		InterlockedIncrement(&UCFabryka);
	else
		InterlockedDecrement(&UCFabryka);
	return S_OK;
};


HRESULT STDMETHODCALLTYPE Fabryka2::CreateInstance(IUnknown *outer, REFIID iid, void **ptr) {

	if (ptr == NULL) return E_POINTER;
	*ptr = NULL;
	if (iid != IID_IUnknown && iid != IID_IKlasa2)
		return E_NOINTERFACE;

	Klasa2* objKlasy = new Klasa2();
	if (objKlasy == NULL) return E_OUTOFMEMORY;

	HRESULT result = objKlasy->QueryInterface(iid, ptr);
	if (FAILED(result)) {
		delete objKlasy;
		*ptr = NULL;
	}
	return result;
};
