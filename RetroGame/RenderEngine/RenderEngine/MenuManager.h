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

		void	OnWindowResize(int const x, int const y);
		void	OnScroll(double const x, double const y);
		void	OnMouseMove(double const x, double const y);
		void	OnMousePress(int const button, int const action);

		std::vector<Interface::IMenu*> menu_nodes;

	};

}
