#pragma once

namespace NativeComponent
{
	public interface class INativeCallable
	{
	public:
		int Run();
		int RunWithArgs(int arg1, Platform::String^ arg2);
	};

	public ref class NativeCallable sealed : public INativeCallable
    {
	private:
		int counter = 0;
    public:
		NativeCallable();
		virtual int Run();
		virtual int RunWithArgs(int arg1, Platform::String^ arg2);
	};
}
