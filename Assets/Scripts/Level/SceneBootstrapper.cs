using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tiggy.Level
{
    public class SceneBootstrapper : MonoBehaviour
    {
        [Tooltip("Level scene to load additively on top of the managers scene. Hardcoded for now until level selection exists.")]
        public string LevelSceneName = "Pipeline";

        protected virtual void Awake()
        {
            if (!SceneManager.GetSceneByName(LevelSceneName).isLoaded)
                SceneManager.LoadScene(LevelSceneName, LoadSceneMode.Additive);
        }
    }
}
