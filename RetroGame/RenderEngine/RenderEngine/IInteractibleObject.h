#pragma once


namespace Rendering {
	namespace Interface {


		class IInteractibleObject
		{


		public:
			virtual void	OnScroll(double const x, double const y) = 0;

			virtual void	OnMousePress(double const x, double const y) = 0;
			virtual void	OnMouseRelease(double const x, double const y) = 0;
			virtual void	OnMouseMove(double const x, double const y) = 0;

			virtual void	OnKeyPressed(int const key) = 0;
			virtual void	OnKeyRelease(int const key) = 0;

		};


	}
}