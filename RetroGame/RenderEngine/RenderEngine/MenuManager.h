#pragma once
#include <vector>

#include "glm/glm.hpp"
#include "IMenu.h"

namespace Rendering {


	class MenuManager
	{
	public:
		void Render(float const deltaTime, glm::vec2 const& winSize) const;
		void Update(float const deltaTime, glm::vec2 const& winSize);


		/*
			Callbacks
		*/

		void OnScroll(double const x, double const y);

		void OnMousePress(int const button, double const x, double const y);
		void OnMouseRelease(int const button, double const x, double const y);
		void OnMouseMove(double const x, double const y);

		void OnKeyPressed(int const key, int mods);
		void OnKeyRelease(int const key, int mods);

		std::vector<Interface::IMenu*> menu_nodes;

	private:
		Interface::IMenu* m_focusedItem;

	};

}
