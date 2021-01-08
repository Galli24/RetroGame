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


	for (auto& node : this->nodes)
		node->Render(this->m_window.size);

}


void Rendering::SceneGraph::Update(float const deltaTime) const
{

	for (auto& node : this->nodes)
		node->UpdatePosition(deltaTime, this->m_window.size);

	for (auto& node : this->nodes)
		node->UpdateGraphics(deltaTime, this->m_window.size);
}

void Rendering::SceneGraph::Blit() const
{
	this->m_window.BlitWindow();
}

Rendering::Window& Rendering::SceneGraph::GetWindow()
{
	return this->m_window;
}

