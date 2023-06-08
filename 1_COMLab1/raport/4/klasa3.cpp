#include<windows.h>
#include<stdio.h>
#include"klasa3.h"


extern volatile int usageCount;


Klasa3::Klasa3() {
	usageCount++;
	class3_ref_count = 0;
	};


Klasa3::~Klasa3() {
	usageCount--;
	};


ULONG STDMETHODCALLTYPE Klasa3::AddRef() {
	/*
	Tutaj zaimplementuj dodawanie referencji na obiekt.
	 */
	InterlockedIncrement(&class3_ref_count);
	return class3_ref_count;
	};


ULONG STDMETHODCALLTYPE Klasa3::Release() {
	/*
	Tutaj zaimplementuj usuwania referencji na obiekt.
	Je�eli nie istnieje �adna referencja obiekt powinien zosta� usuni�ty.
	 */
	ULONG dec_references = InterlockedDecrement(&class3_ref_count);
	if(dec_references == 0) 
		delete this;
	return dec_references;
	};


HRESULT STDMETHODCALLTYPE Klasa3::QueryInterface(REFIID iid, void **ptr) {
	if(ptr == NULL) return E_POINTER;
	if(IsBadWritePtr(ptr, sizeof(void *))) return E_POINTER;
	*ptr = NULL;
	if(iid == IID_IUnknown) *ptr = this;
	if(iid == IID_IKlasa3) *ptr = this;
	if(*ptr != NULL) { AddRef(); return S_OK; };
	return E_NOINTERFACE;
	};

HRESULT STDMETHODCALLTYPE Klasa3::Test(const char *napis){
	/*
	Tutaj zaimplementuj funkcj� drukuj�c� napis.
	 */
	printf(napis);
	return S_OK;
	};

