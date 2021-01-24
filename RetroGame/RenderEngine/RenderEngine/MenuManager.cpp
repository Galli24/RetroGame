#include "MenuManager.h"

bool isInside(glm::vec2 const& size, glm::vec2 position, double const x, double const y)
{
	return x > position.x && x < position.x + size.x && y > position.y && y < position.y + size.y;
}

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

void Rendering::MenuManager::OnScroll(double const x, double const y)
{
	if (m_focusedItem)
		m_focusedItem->OnScroll(x, y);
}

void Rendering::MenuManager::OnMousePress(int const button, double const x, double const y)
{
	if (m_focusedItem) 
	{
		m_focusedItem->OnLostFocus();
		m_focusedItem = nullptr;
	}

	for (auto& elt : menu_nodes)
	{
		auto pos = elt->GetActualPosition();
		auto size = elt->GetObjectSize();
		if (isInside(size, pos, x, y))
		{
			elt->OnFocus();
			m_focusedItem = elt;
			elt->OnMousePress(button, x - pos.x, y - pos.y);
			break;
		}
	}
}

void Rendering::MenuManager::OnMouseRelease(int const button, double const x, double const y)
{
	for (auto& elt : menu_nodes)
	{
		auto pos = elt->GetActualPosition();
		auto size = elt->GetObjectSize();
		if (isInside(size, pos, x, y))
		{
			elt->OnMouseRelease(button, x - pos.x, y - pos.y);
			break;
		}
	}
}

void Rendering::MenuManager::OnMouseMove(double const x, double const y)
{
	for (auto& elt : menu_nodes)
	{
		auto pos = elt->GetActualPosition();
		auto size = elt->GetObjectSize();
		if (isInside(size, pos, x, y))
		{
			elt->OnMouseMove(x - pos.x, y - pos.y);
			break;
		}
	}
}

void Rendering::MenuManager::OnKeyPressed(int const key, int mods)
{
	if (m_focusedItem)
		m_focusedItem->OnKeyPressed(key, mods);
}

void Rendering::MenuManager::OnKeyRelease(int const key, int mods)
{
	if (m_focusedItem)
		m_focusedItem->OnKeyRelease(key, mods);
}

void Rendering::MenuManager::OnCharReceived(char const c)
{
	if (m_focusedItem)
		m_focusedItem->OnCharReceived(c);
}

