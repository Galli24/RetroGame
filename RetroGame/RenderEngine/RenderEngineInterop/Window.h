#pragma once
#include "BaseInterop.h"
#include "../RenderEngine/Window.h"
#include "MenuManager.h"
#include <string>
#include <vcclr.h> //required for gcroot

namespace RenderEngine { class WindowWrapper; }

using namespace System;
using namespace System::Numerics;
namespace RenderEngine {


	public ref class Window : public BaseInterop<WindowWrapper>
	{


	internal:
		Window(String^ name, Vector2 size, MenuManager^ menuManager);
		void RaiseResizeEvent(int const x, int const y);
		void RaiseKeyActionEvent(int const key, int const mods, bool pressed);



	public:

		delegate void OnKeyPressedDelegate(int const key, int const mods);
		event OnKeyPressedDelegate^ OnKeyPressed;
		delegate void OnKeyReleaseDelegate(int const key, int const mods);
		event OnKeyReleaseDelegate^ OnKeyRelease;

		delegate void WindowResizeDelegate(int const x, int const y);
		event WindowResizeDelegate^ OnWindowResize;

		bool ShouldClose();

		property Vector2 Size {
			Vector2 get();
		}



	};

#pragma unmanaged
	class WindowWrapper : public Rendering::Window {

	private:
		gcroot<RenderEngine::Window^> _managed;

	public:

		WindowWrapper(gcroot<RenderEngine::Window^> managed, std::string const& name, glm::ivec2 const& size, Rendering::MenuManager* menuManager);


		void OnWindowResize(int const x, int const y) override;
		void OnKeyAction(int key, int scancode, int action, int mods) override;


	};

#pragma managed

}
