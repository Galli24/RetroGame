#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <vector>
#include <guiddef.h>
#include <string>

#include "IGraphNode.h"
#include "Window.h"
#include "IMenu.h"
#include "MenuManager.h"


namespace Rendering {


	/* 
		Main Rendering class. 
		Call Init to init the OpenGL context and create a Window. 
		Renders a scene based on a tree view where each node's model matrix depends depends on its parent's one.
	*/
	class SceneGraph
	{

	public:
		// Defines the OpenGL windows attributes (window size and name)
		SceneGraph(glm::ivec2 const& windowSize, std::string const& windowName, Rendering::MenuManager const& manager);

		// Destroy the OpenGL context
		~SceneGraph();

		// Render the scene
		void Render(float const deltaTime) const;

		// Update the scene
		void Update(float const deltaTime);

		void Blit() const;
		// Get the Window.
		Rendering::Window &GetWindow();
		Rendering::MenuManager& GetMenuManager();


		std::vector<Interface::IGraphNode*> nodes; 

	private:
		Rendering::Window m_window;
		Rendering::MenuManager m_menuManager;
	};


}

