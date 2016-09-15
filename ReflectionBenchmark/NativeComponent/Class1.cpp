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

BubbleSortRunner::BubbleSortRunner()
{	
}

void BubbleSortRunner::Run(int count)
{
	for (int i = 0; i < count; i++)
	{
		this->DoSort();
	}
}

void BubbleSortRunner::Swap(int *a, int *b)
{
	int temp = *a;
	*a = *b;
	*b = temp;
}

void BubbleSortRunner::BubbleSort(std::vector<int> array)
{	
	//comparisons will be done n times
	for (int i = 0; i < array.size(); i++)
	{
		//compare elemet to the next element, and swap if condition is true
		for (int j = 0; j < array.size() - 1; j++)
		{
			if (array[j] > array[j + 1])
				this->Swap(&array[j], &array[j + 1]);
		}
	}
}

void BubbleSortRunner::DoSort()
{
	std::vector<int> array;
	array.resize(4096);
	for (int i = 0; i < 4096; i++)
	{
		array[i] = i % 100;
	}
	this->BubbleSort(array);
	this->data = array;
}