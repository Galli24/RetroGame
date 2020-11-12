#pragma once



namespace Rendering {
	namespace Interface {

		class IMovable
		{
		public:
			// Update position (patterns etc).
			virtual void UpdatePosition(float deltaTime) = 0;
		};


	}
}

