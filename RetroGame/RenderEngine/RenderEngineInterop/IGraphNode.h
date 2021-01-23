#pragma once

#include "BaseInterop.h"
#include "../RenderEngine/IGraphNode.h"

using namespace System::Numerics;

namespace RenderEngine {


	public ref class IGraphNode
	{
	internal:
		IGraphNode() {}

		IGraphNode(Rendering::Interface::IGraphNode* resources) {
			this->graphNodeResources = resources;
		}

		!IGraphNode() {
			delete graphNodeResources;
		}

		~IGraphNode() {
			this->!IGraphNode();
		}


		Rendering::Interface::IGraphNode* graphNodeResources;

		virtual void SetResources(Rendering::Interface::IGraphNode* res)
		{
			graphNodeResources = res;
		}


	public:



		property System::Guid ^ID
		{
			System::Guid ^get() {
				auto &guid = this->graphNodeResources->GetID();
				return gcnew System::Guid(guid.Data1, guid.Data2, guid.Data3,
										  guid.Data4[0], guid.Data4[1],
										  guid.Data4[2], guid.Data4[3],
										  guid.Data4[4], guid.Data4[5],
										  guid.Data4[6], guid.Data4[7]);
			}
		}

		property Vector2 Position
		{
			Vector2 get()
			{
				return Vector2(this->graphNodeResources->position.x, this->graphNodeResources->position.y);
			}

			void set(Vector2 value)
			{
				this->graphNodeResources->position = glm::vec2{ value.X, value.Y };
			}
		}

	};

}

