#include <iostream>

#include "SceneGraph.h"


rendering::SceneGraph::SceneGraph(vec2int const& windowSize, std::string const& windowName)
	: m_nodes(), m_window(windowName, windowSize)
{
	if (!gladLoadGL())
	{
		glfwTerminate();
		throw std::exception("Can't load GL");
	}
}

rendering::SceneGraph::~SceneGraph()
{
	glfwTerminate();
}

void rendering::SceneGraph::Render() const
{
	this->m_window.ProcessInput();
	this->m_window.ClearWindow();

	// Render

	this->m_window.BlitWindow();
}


rendering::Window &rendering::SceneGraph::GetWindow()
{
	return this->m_window;
}
