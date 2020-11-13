#include "pch.h"
#include "SceneGraph.h"

RenderEngine::SceneGraph::SceneGraph(Vector2 size, String^ name)
{
	this->nativeResources = new Rendering::SceneGraph{
		Rendering::vec2int{ size.X, size.Y },
		msclr::interop::marshal_as<std::string>(name)
	};
}

void RenderEngine::SceneGraph::Render(float deltaTime) 
{
	this->nativeResources->Render(deltaTime);
}

void RenderEngine::SceneGraph::Update(float deltaTime)
{
	this->nativeResources->Update(deltaTime);
}
