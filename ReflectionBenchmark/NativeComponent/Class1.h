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

	public interface class INativeCaller
	{
	public:
		void Invoke(INativeCallable^ callable, int count);
	};

	public interface class IWinmdNativeCaller
	{
	public:
		void Invoke(DotNetComponent::IDotnetCallableInterface^ callable, int count);
	};

	public interface class IWinmdNativeCaller2
	{
	public:
		void Invoke(DotNetComponent::DotnetCallable^ callable, int count);
	};

	public ref class NativeCaller sealed : public INativeCaller
	{
	private:
		int counter = 0;
	public:
		NativeCaller();
		virtual void Invoke(INativeCallable^ callable, int count);
	};

	public ref class WinmdNativeCaller sealed : public IWinmdNativeCaller, IWinmdNativeCaller2
	{
	private:
		int counter = 0;
	public:
		WinmdNativeCaller();
		virtual void Invoke(DotNetComponent::IDotnetCallableInterface^ callable, int count);
		virtual void Invoke(DotNetComponent::DotnetCallable^ callable, int count);
	};

	public interface class IBenchmarkRunner
	{
	public:
		void Run(int count);
	};

	public ref class BubbleSortRunner sealed : public IBenchmarkRunner
	{
	private:
		void DoSort();
		void Swap(int *a, int *b);
		void BubbleSort(std::vector<int> array);
		std::vector<int> data;
	public:
		BubbleSortRunner();
		virtual void Run(int count);
	};
}
