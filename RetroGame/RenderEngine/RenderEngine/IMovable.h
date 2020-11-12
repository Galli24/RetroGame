#pragma once
#include <glm/glm.hpp>


namespace Rendering {
	namespace Interface {

		class IMovable
		{
		public:
			// Update position (patterns etc).
			virtual void UpdatePosition(float deltaTime, glm::vec2 const& winSize) = 0;
		};


	}
}

