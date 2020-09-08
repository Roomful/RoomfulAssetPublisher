using UnityEngine;

namespace net.roomful.assets
{

	[ExecuteInEditMode]
	public class Environment : MonoBehaviour {


	    public GameObject Light;
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



            RenderSettings.skybox = Skybox;
            RenderSettings.ambientIntensity = AmbientIntensity;


            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
            RenderSettings.customReflection = ReflectionCubemap;
            RenderSettings.reflectionIntensity = ReflectionIntensity;
        }


	}
}