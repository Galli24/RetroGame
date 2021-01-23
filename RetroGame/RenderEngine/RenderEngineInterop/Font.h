#pragma once
#include "BaseInterop.h"
#include <cliext/adapter>
#include <cliext/algorithm>
#include <cliext/vector>
#include <cstring>
#include <msclr/marshal_cppstd.h>
#include "../RenderEngine/Font.h"

using namespace System;


namespace RenderEngine {


	public ref class Font : BaseInterop<Rendering::Font>
	{
	public:
		Font(String^ path, int size)
		{
			this->nativeResources = new Rendering::Font(msclr::interop::marshal_as<std::string>(path), size);
		}
	};

}
