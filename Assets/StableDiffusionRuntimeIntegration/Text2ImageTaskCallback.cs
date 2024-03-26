using System.Threading.Tasks;
using UnityEngine;

namespace StableDiffusionRuntimeIntegration
{
    public abstract class Text2ImageTaskCallback : MonoBehaviour
    {
        public abstract void OnPerformTaskCallback(Task task);
        
        public abstract void OnTaskCompletedCallback(string imagePath);
    }
}
