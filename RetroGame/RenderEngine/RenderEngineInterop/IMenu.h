#pragma once

#include "BaseInterop.h"
#include "IGraphNode.h"
#include "../RenderEngine/IMenu.h"


using namespace System::Numerics;
using namespace RenderEngine;


namespace RenderEngine {

	ref class IMenu : public IGraphNode
	{
	public:
		IMenu(Rendering::Interface::IMenu* resources) {
			this->nativeResources = resources;
		}

	};

}
