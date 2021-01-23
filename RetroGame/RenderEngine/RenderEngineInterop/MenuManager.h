#pragma once
#include "BaseInterop.h"
#include "IMenu.h"
#include "../RenderEngine/MenuManager.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;


namespace RenderEngine {

	public ref class MenuManager : BaseInterop<Rendering::MenuManager>
	{


				
	public:
		MenuManager();

		property List<IMenu^>^ Nodes {

			List<IMenu^>^ get() {
				auto a = gcnew List<IMenu^>();
				for (auto& e : this->nativeResources->menu_nodes)
					a->Add(gcnew IMenu(e));
				return a;
			}

			void set(List<IMenu^>^ value) {
				this->nativeResources->menu_nodes.clear();
				for each (auto e in value)
					this->nativeResources->menu_nodes.push_back(dynamic_cast<Rendering::Interface::IMenu*>(e->nativeResources));
			}

		}


	};

}
