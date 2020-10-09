#pragma once
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include <vector>
#include <guiddef.h>
#include <string>

#include "IGraphNode.h"
#include "Camera.h"


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

		bool Init();

		// Render the scene 
		void Render(rendering::Camera const& camera);

		// Append a IGraphNode to the render root. These will be drawn via SceneGraph::Render
		int AppendNode(IGraphNode* node);

		// Remove a node from the render root (by ID)
		void RemoveNode(GUID const& id);

		// Remove a node from the render root (by index)
		void RemoveNode(int const index);

		// Set the background color
		void SetClearColor(glm::vec4 const& color);

		// Mouse Move callback for the current Window
		void MouseCallback(double const xpos, double const ypos);

		// Window Resize callback for the current Window
		void WindowResizeCallback(int const width, int const height);

		// Mouse Scroll callback for the current Window
		void ScrollCallback(double const xoff, double const yoff);

		// Defines the camera for this window
		void SetCamera(Camera* camera) { this->m_camera = camera; }

		// Get the GLFW Window.
		GLFWwindow* GetWindow() const;


	private:
		void Clear();
		void Blit();

	private:
		std::vector<IGraphNode*> m_nodes;
		glm::vec4 m_clearColor;
		GLFWwindow* m_window;
		vec2int m_windowSize;
		std::string m_windowName;
		Camera *m_camera;
	};


}

