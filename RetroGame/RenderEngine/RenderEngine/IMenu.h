#pragma once

#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>

#include "IInteractibleObject.h"
#include "IGraphNode.h"


namespace Rendering {
	namespace Interface {

		class IMenu : public IGraphNode, public IInteractibleObject
		{

		public:
			void	UpdateGraphics(float deltaTime, glm::vec2 const& winSize)	override = 0;
			void	Render(glm::vec2 const& winSize)							override = 0;
			void	UpdatePosition(float deltaTime, glm::vec2 const& winSize)	override = 0;


			void	OnScroll(double const x, double const y)					override = 0;

			void	OnMousePress(double const x, double const y)				override = 0;
			void	OnMouseRelease(double const x, double const y)				override = 0;
			void	OnMouseMove(double const x, double const y)					override = 0;

			void	OnKeyPressed(int const key)									override = 0;
			void	OnKeyRelease(int const key)									override = 0;
		};


	}
}
