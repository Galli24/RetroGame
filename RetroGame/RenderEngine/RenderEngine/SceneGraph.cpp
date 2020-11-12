#include <iostream>

#include "SceneGraph.h"
#include <algorithm>

Rendering::SceneGraph::SceneGraph(vec2int const& windowSize, std::string const& windowName)
	: m_window(windowName, windowSize)
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

	for (auto& node : this->m_nodes)
		node->UpdateGraphics(deltaTime, this->m_window.size);

	for (auto& node : this->m_nodes)
		node->Render(this->m_window.size);

	this->m_window.BlitWindow();
}


Rendering::Window& Rendering::SceneGraph::GetWindow()
{
	return this->m_window;
}

size_t Rendering::SceneGraph::AddNode(Interface::IGraphNode* node)
{
	this->m_nodes.push_back(node);
	return m_nodes.size();
}

size_t Rendering::SceneGraph::RemoveNode(Interface::IGraphNode* node)
{
	auto elt = std::find(this->m_nodes.begin(), this->m_nodes.end(), node);
	if (elt != this->m_nodes.end())
		this->m_nodes.erase(elt);
	return this->m_nodes.size();
}

