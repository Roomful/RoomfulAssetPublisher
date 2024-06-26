using UnityEngine;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
	internal class Environment : MonoBehaviour {

	    public GameObject Walls;
	    public GameObject SizeRef;

        public Material Skybox;
        public float AmbientIntensity;

        public Cubemap ReflectionCubemap;
        public float ReflectionIntensity;

	    public bool RenderEnvironment = true;
        
        void Awake() {
            ApplyEnvironment();
        }

        public void Update() {
            ApplyEnvironment();
        }

        private void ApplyEnvironment() {

            Walls.SetActive(RenderEnvironment);
            SizeRef.SetActive(RenderEnvironment);

            if (RenderSettings.skybox == null || RenderSettings.skybox.name == "Default-Skybox") {
                RenderSettings.skybox = Skybox;
                RenderSettings.ambientIntensity = AmbientIntensity;
            
                RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
                RenderSettings.customReflection = ReflectionCubemap;
                RenderSettings.reflectionIntensity = ReflectionIntensity;
            }
        }


	}
}