#ifndef FABRYKA_H
#define FABRYKA_H
#include<windows.h>

class Fabryka2 : public IClassFactory {
public:
	Fabryka2();
	~Fabryka2();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **rv);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE LockServer(BOOL v);
	virtual HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown *outer, REFIID id, void **rv);
private:
	volatile ULONG reference_counter;
};

#endif
