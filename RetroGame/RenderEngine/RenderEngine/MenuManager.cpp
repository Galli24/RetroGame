#include "MenuManager.h"

void Rendering::MenuManager::Render(float const deltaTime, glm::vec2 const& winSize) const
{
	for (auto& node : this->menu_nodes)
		node->Render(winSize);
}

void Rendering::MenuManager::Update(float const deltaTime, glm::vec2 const& winSize)
{
	for (auto& node : this->menu_nodes)
		node->UpdateGraphics(deltaTime, winSize);
}

void Rendering::MenuManager::OnWindowResize(int const x, int const y)
{

}

void Rendering::MenuManager::OnScroll(double const x, double const y)
{

}

void Rendering::MenuManager::OnMouseMove(double const x, double const y)
{

}

void Rendering::MenuManager::OnMousePress(int const button, int const action)
{

}
