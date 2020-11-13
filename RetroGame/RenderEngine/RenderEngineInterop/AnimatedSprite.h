#pragma once
#include "BaseInterop.h"
#include "IGraphNode.h"
#include "../RenderEngine/AnimatedSprite.h"

using namespace System;
using namespace System::Numerics;
using namespace System::Collections::Generic;

namespace RenderEngine {

	public ref class AnimatedSprite : RenderEngine::IGraphNode
	{

	public:
		AnimatedSprite(
			IEnumerable<String^>^ sprites,
			float frameDuration,
			Vector2 position,
			Vector2 size,
			Vector2 scale
		);

		AnimatedSprite(
			IEnumerable<String^>^ sprites,
			float frameDuration,
			Vector2 position,
			Vector2 size
		);

	};

}
