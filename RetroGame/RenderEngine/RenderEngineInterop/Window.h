#pragma once
#include "BaseInterop.h"
#include "../RenderEngine/Window.h"

using namespace System::Numerics;

namespace RenderEngine {


	public ref class Window : public BaseInterop<Rendering::Window>
	{


	internal:

		Window(Rendering::Window* win) {
			this->nativeResources = win;
		}

	public:
		bool ShouldClose() {
			return this->nativeResources->ShouldClose();
		}

		property Vector2^ Size {

			Vector2^ get() {
				auto size = this->nativeResources->size;
				return gcnew Vector2(size.x, size.y);
			}

		}
	};

}
