#pragma once

#include <glm/glm.hpp>

namespace Rendering {
	namespace Interface {


		class IRenderable {

		public:

			// Update graphical properties of the node
			virtual void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) = 0;
			// Rendering the node
			virtual void Render(glm::vec2 const& winSize) = 0;
		};


	}
}