//
// Created by marwac-9 on 9/16/15.
//
#include "app.h"
#include <imgui.h>
#include "imgui_impl_glfw_gl3.h"
#include "gl_window.h"
#include "MyMathLib.h"
#include "Attenuation.h"

class BoundingBox;
class Object;
class Camera;
class FrameBuffer;
class Texture;

namespace Picking
{
    class PickingApp : public Core::App
    {
    public:
        /// constructor
        PickingApp();
        /// destructor
        ~PickingApp();

        /// open app
        bool Open();
        /// run app
        void Run();

	private:
		void Clear();
		void DrawPass2();
        void PickingTest();

		void Draw();
		void DrawPicking();
		void DrawDebug();

		void GenerateGUI();

        void InitGL();
        void ClearBuffers();
		void SetUpBuffers(int windowWidth, int windowHeight);
        void KeyCallback(int key, int scancode, int action, int mods);
		void MouseCallback(double mouseX, double mouseY);
		void Monitor(Display::Window* window);
		void SetUpCamera();
		void LoadScene1();
		void LoadScene2();
		void LoadScene3();
		void LoadScene4();
		void LoadScene5();
		void Vortex();
		bool scene4loaded = false;
		bool wireframe = false;
        GLuint MatrixHandle;
        GLuint ViewMatrixHandle;
        GLuint ModelMatrixHandle;
        GLuint MaterialAmbientValueHandle;
        GLuint MaterialSpecularValueHandle;
        GLuint MaterialDiffuseIntensityValueHandle;
        GLuint MaterialColorValueHandle;

        bool altButtonToggle = true;
        //camera type 1 == fps, 2 == tps 3 == monitoring camera

        bool running = false;
		bool debug = true;
        Display::Window* window;
        bool isLeftMouseButtonPressed = false;
        double leftMouseX;
        double leftMouseY;
        int windowWidth;
        int windowHeight;
		float windowMidX;
		float windowMidY;
		float near = 0.1f;
		float far = 2000.f;
		float fov = 45.f;

		float xAngles = 108.f;
		float yAngles = 168.f;
		float posX = 0;
		float posY = 10;
		float posZ = 0;

		float xAngled = 108.f;
		float yAngled = 168.f;

		mwm::Vector3F lightInvDir = mwm::Vector3F(-1.f, 1.f, 1.f);

		mwm::Vector3F pointLightPos = mwm::Vector3F(5.f, 0.f, 0.f);
		float pointLightRadius = 10.0f;
		Attenuation pointLightAttenuation;
		
		mwm::Vector3F spotLightPos = mwm::Vector3F(0.f, 10.f, 0.f);
		mwm::Vector3F spotLightInvDir = mwm::Vector3F(-1.f, 1.f, 1.f);
		float spotLightCutOff = 12.5f;
		float spotLightOuterCutOff = 17.5f;
		float spotLightRadius = 30.f;
		Attenuation spotLightAttenuation;

		Camera* currentCamera;
        Object* lastPickedObject = nullptr;
		int objectsRendered = 0;
		unsigned int pickedID = 0;
		Object* spotLightObject;
		Object* directionalLightObject;
		FrameBuffer* pickingBuffer;
		Texture* pickingTexture;
		Texture* worldPosTexture;
    };
} // namespace Example