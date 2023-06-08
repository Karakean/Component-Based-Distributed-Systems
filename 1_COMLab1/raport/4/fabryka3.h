#include<windows.h>

class KlasaFactory: public IClassFactory {
public:	
	KlasaFactory();
	~KlasaFactory();
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **rv);
	virtual HRESULT STDMETHODCALLTYPE LockServer(BOOL v);
	virtual HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown *outer, REFIID id, void **rv);

private:	

	/*
	Tutaj zdeklaruj zmienne:
		- licznik blokad,
		- licznik referencji.
	 */
	volatile int factory_lock_count;
	volatile ULONG factory_ref_count;

	};
