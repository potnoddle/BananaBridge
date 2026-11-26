using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
    public interface IAiClient
    {
        Task<Bitmap> SendEditRequestAsync(
            string apiKey,
            string model,
            List<ChatTurn> history,
            Bitmap sourceImage,
            Bitmap maskImage,
            string newPrompt,
            string aspectRatio = null);

        Task<List<string>> ListModelsAsync(string apiKey);
    }
}

