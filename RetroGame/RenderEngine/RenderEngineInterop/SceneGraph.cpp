#include "pch.h"
#include "SceneGraph.h"
#include <glm/glm.hpp>

RenderEngine::SceneGraph::SceneGraph(Vector2 size, String^ name, MenuManager^ menuManager)
{
	_win = gcnew Window(name, size, menuManager);
	this->nativeResources = new Rendering::SceneGraph(_win->nativeResources, menuManager->nativeResources);
		
		
		/*new Rendering::SceneGraph(
		{ size.X, size.Y },
		msclr::interop::marshal_as<std::string>(name),
		menuManager->nativeResources
	);*/
}


void RenderEngine::SceneGraph::Render(float deltaTime) 
{
	this->nativeResources->Render(deltaTime);
	this->nativeResources->Blit();
}

void RenderEngine::SceneGraph::Update(float deltaTime)
{
	this->nativeResources->Update(deltaTime);
}
