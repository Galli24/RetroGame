#include "pch.h"
#include "Window.h"
#include <cliext/adapter>
#include <cliext/algorithm>
#include <cliext/vector>
#include <cstring>
#include <msclr/marshal_cppstd.h>

RenderEngine::Window::Window(String^ name, Vector2 size, MenuManager^ menuManager)
{
	this->nativeResources = new WindowWrapper(
		this,
		msclr::interop::marshal_as<std::string>(name),
		{ size.X, size.Y },
		menuManager->nativeResources
	);

}

void RenderEngine::Window::RaiseResizeEvent(int const x, int const y)
{
	OnWindowResize(x, y);
}

void RenderEngine::Window::RaiseKeyActionEvent(int const key, int const mods)
{
	OnKeyAction(key, mods);
}

bool RenderEngine::Window::ShouldClose()
{
	return this->nativeResources->ShouldClose();
}

System::Numerics::Vector2 RenderEngine::Window::Size::get()
{
	auto size = this->nativeResources->size;
	return Vector2(size.x, size.y);
}

RenderEngine::WindowWrapper::WindowWrapper(gcroot<RenderEngine::Window^> managed, std::string const& name, glm::ivec2 const& size, Rendering::MenuManager* menuManager) : Rendering::Window(name, size, menuManager),
_managed(managed)
{

}

void RenderEngine::WindowWrapper::OnWindowResize(int const x, int const y)
{
	Rendering::Window::OnWindowResize(x, y);
	_managed->RaiseResizeEvent(x, y);
}

void RenderEngine::WindowWrapper::OnKeyAction(int key, int scancode, int action, int mods)
{
	Rendering::Window::OnKeyAction(key, scancode, action, mods);
	_managed->RaiseKeyActionEvent(key, mods);
}
