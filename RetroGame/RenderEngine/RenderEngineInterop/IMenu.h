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

		delegate void OnScrollDelegate(double const x, double const y);
		delegate void OnMousePressDelegate(int const key, double const x, double const y);
		delegate void OnMouseReleaseDelegate(int const key, double const x, double const y);
		delegate void OnMouseMoveDelegate(double const x, double const y);
		delegate void OnKeyPressDelegate(int const key, int const mods);
		delegate void OnKeyReleaseDelegate(int const key, int const mods);
		delegate void OnCharReceivedDelegate(char const c);

		event OnScrollDelegate^ OnScroll;
		event OnMousePressDelegate^ OnMousePress;
		event OnMouseReleaseDelegate^ OnMouseRelease;
		event OnMouseMoveDelegate^ OnMouseMove;
		event OnKeyPressDelegate^ OnKeyPress;
		event OnKeyReleaseDelegate^ OnKeyRelease;
		event OnCharReceivedDelegate^ OnCharReceived;

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

	internal:
		void SetResources(Rendering::Interface::IMenu* res) {
			this->graphNodeResources = res;
			this->menuResources = res;

		}

		void RaiseScroll(double const x, double const y) { OnScroll(x, y); }
		void RaiseMousePress(int const key, double const x, double const y) { OnMousePress(key, x, y); }
		void RaiseMouseRelease(int const key, double const x, double const y) { OnMouseRelease(key, x, y); }
		void RaiseMouseMove(double const x, double const y) { OnMouseMove(x, y); }
		void RaiseKeyPressed(int const key, int const mods) { OnKeyPress(key, mods); }
		void RaiseKeyRelease(int const key, int const mods) { OnKeyRelease(key, mods); }
		void RaiseCharReceived(char const c) { OnCharReceived(c); }

	};

}
