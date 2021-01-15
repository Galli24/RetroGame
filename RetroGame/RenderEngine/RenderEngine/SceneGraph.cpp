#include <iostream>

#include "SceneGraph.h"
#include <algorithm>

Rendering::SceneGraph::SceneGraph(glm::ivec2 const& windowSize, std::string const& windowName, Rendering::MenuManager const& manager)
	: m_window(windowName, windowSize), m_menuManager(manager)
{
	if (!gladLoadGL())
	{
		glfwTerminate();
		throw std::exception("Can't load GL");
	}
}

Rendering::SceneGraph::~SceneGraph()
{
	glfwTerminate();
}

void Rendering::SceneGraph::Render(float const deltaTime) const
{
	this->m_window.ProcessInput();
	this->m_window.ClearWindow();


	for (auto& node : this->nodes)
		node->Render(this->m_window.size);

	m_menuManager.Render(deltaTime, this->m_window.size);
}


void Rendering::SceneGraph::Update(float const deltaTime)
{

	for (auto& node : this->nodes)
		node->UpdateGraphics(deltaTime, this->m_window.size);
	
	m_menuManager.Update(deltaTime, this->m_window.size);
}

void Rendering::SceneGraph::Blit() const
{
	this->m_window.BlitWindow();
}

Rendering::Window& Rendering::SceneGraph::GetWindow()
{
	return this->m_window;
}

Rendering::MenuManager& Rendering::SceneGraph::GetMenuManager()
{
	return this->m_menuManager;
}

