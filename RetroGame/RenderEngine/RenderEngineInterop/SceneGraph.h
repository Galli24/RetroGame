#pragma once

#include <msclr/marshal_cppstd.h>

#include "../RenderEngine/SceneGraph.h"
#include "BaseInterop.h"
#include "IGraphNode.h"


using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;
using namespace RenderEngine;

namespace RenderEngine {


	public ref class SceneGraph : BaseInterop<Rendering::SceneGraph>
	{
	public:
		SceneGraph(Vector2 size, String^ name);

		void Render(float deltaTime);
		void Update(float deltaTime);

		property List<IGraphNode^> ^Nodes {

			List<IGraphNode^> ^get() {
				auto a = gcnew List<IGraphNode^>();
				for (auto& e : this->nativeResources->nodes)
					a->Add(gcnew IGraphNode(e));
				return a;
			}

			void set(List<IGraphNode^> ^value) {
				this->nativeResources->nodes.clear();
				for each (auto e in value)
					this->nativeResources->nodes.push_back(e->nativeResources);
			}

		}


	};

}
