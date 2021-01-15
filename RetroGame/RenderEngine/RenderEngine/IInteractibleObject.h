#pragma once


namespace Rendering {
	namespace Interface {


		class IInteractibleObject
		{


		public:
			virtual void	OnScroll(double const x, double const y) = 0;

			virtual void	OnMousePress(int const key, double const x, double const y) = 0;
			virtual void	OnMouseRelease(int const key, double const x, double const y) = 0;
			virtual void	OnMouseMove(double const x, double const y) = 0;

			virtual void	OnKeyPressed(int const key, int const mods) = 0;
			virtual void	OnKeyRelease(int const key, int const mods) = 0;

		};


	}
}