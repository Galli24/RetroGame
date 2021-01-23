#pragma once

#include "BaseInterop.h"
#include "IGraphNode.h"
#include "../RenderEngine/IMenu.h"


using namespace System::Numerics;
using namespace RenderEngine;


namespace RenderEngine {



	public ref class IMenu : public IGraphNode
	{
	public:

		enum class Anchor
		{
			TopLeft, Top, TopRight,
			Left, Center, Right,
			BottomLeft, Bot, BottomRight
		};

		IMenu() {}

		IMenu(Rendering::Interface::IGraphNode* resources) {
			this->nativeResources = resources;
		}


	};

}
