#ifndef FABRYKA_2
#define FABRYKA_2

#include <Windows.h>

class fabryka2 : IClassFactory
{
public:
	fabryka2();
	~fabryka2();

	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID InterfaceIdentifier, void **InterfacePointer);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE LockServer(BOOL blocked);
	virtual HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown *outerInterface,
		REFIID InterfaceIdentifier, void **InterfacePointer);

private:
	volatile ULONG m_ref;
};

#endif