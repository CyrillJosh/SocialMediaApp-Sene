using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.Services
{
    public interface IErrorHandlingService
    {
        event Action<string, string> OnError;
        event Action OnClear;

        void ShowError(string title, string message);
        void Clear();
    }
}
