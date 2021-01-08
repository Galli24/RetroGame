#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <vector>
#include <guiddef.h>
#include <string>

#include "IGraphNode.h"
#include "Window.h"


namespace Rendering {

	using vec2int = glm::vec<2, int, glm::defaultp>;

	/* 
		Main Rendering class. 
		Call Init to init the OpenGL context and create a Window. 
		Renders a scene based on a tree view where each node's model matrix depends depends on its parent's one.
	*/
	class SceneGraph
	{

	public:
		// Defines the OpenGL windows attributes (window size and name)
		SceneGraph(vec2int const& windowSize, std::string const& windowName);

		// Destroy the OpenGL context
		~SceneGraph();

		// Render the scene
		void Render(float const deltaTime) const;

		// Update the scene
		void Update(float const deltaTime) const;

		void Blit() const;
		// Get the Window.
		Rendering::Window &GetWindow();
		std::vector<Interface::IGraphNode*> nodes; 


	private:
		Rendering::Window m_window;
	};


}

