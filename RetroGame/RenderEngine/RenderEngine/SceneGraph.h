#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <vector>
#include <guiddef.h>
#include <string>

#include "IGraphNode.h"
#include "Window.h"


namespace rendering {

	typedef glm::vec<2, int, glm::defaultp> vec2int;

	/* 
		Main rendering class. 
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
		void Render() const;

		// Get the Window.
		rendering::Window &GetWindow();

	private:
		std::vector<IGraphNode*> m_nodes; 
		rendering::Window m_window;
	};


}

