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

		IMenu(Rendering::Interface::IMenu* resources) {
			this->graphNodeResources = resources;
			this->menuResources = resources;
		}


		!IMenu() {
			delete graphNodeResources;
			delete menuResources;
		}

		~IMenu() {
			this->!IMenu();
		}


		Rendering::Interface::IMenu* menuResources;

		void SetResources(Rendering::Interface::IMenu* res) {
			this->graphNodeResources = res;
			this->menuResources = res;

		}
	};

}
