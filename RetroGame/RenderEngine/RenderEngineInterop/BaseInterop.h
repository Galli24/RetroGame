#pragma once


namespace RenderEngine {


	template <typename T>
	public ref class BaseInterop
	{

	internal:
		T* nativeResources;

	protected:
		~BaseInterop()
		{
			this->!BaseInterop();
		};

		!BaseInterop()
		{
			delete this->nativeResources;
		}

	};

}
