#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>

#include <guiddef.h>
#include <combaseapi.h>
#include <vector>

#include "IRenderable.h"
#include "IMovable.h"
#include "IInteractibleObject.h"

namespace Rendering {
	namespace Interface {

		// Base class for the Rendering elements
		class IGraphNode : public IRenderable
		{
		public:
			IGraphNode(glm::vec2 const& pos) : position(pos), m_id() {
				auto _ = CoCreateGuid(&this->m_id);
			}

			// Gets the GUID of this node
			GUID GetID() const noexcept {
				return this->m_id;
			}

			// Update graphical properties of the node
			void UpdateGraphics(float deltaTime, glm::vec2 const& winSize) override = 0;
			// Rendering the node
			void Render(glm::vec2 const& winSize) override = 0;

			glm::vec2	position;

		protected:
			GUID		m_id;
		};

	};
}
