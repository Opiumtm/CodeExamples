#include "pch.h"
#include "Class1.h"

using namespace NativeComponent;
using namespace Platform;

NativeCallable::NativeCallable()
{
}

int NativeCallable::Run()
{
	this->counter = this->counter + 1;
	return counter;
}

int NativeCallable::RunWithArgs(int arg1, Platform::String^ arg2)
{
	this->counter = this->counter + 1;
	return counter;
}

NativeCaller::NativeCaller()
{	
}

void NativeCaller::Invoke(INativeCallable^ callable, int count)
{
	size_t sz = 0;
	Platform::String^ empty = "";
	for (int i = 0; i < count; i++)
	{
		callable->Run();
		callable->RunWithArgs(i, empty);
	}
}