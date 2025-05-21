using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.Services
{
    public partial class ErrorHandlingService : IErrorHandlingService
    {
        public event Action<string, string>? OnError;
        public event Action? OnClear;

        public void ShowError(string title, string message)
        {
            OnError?.Invoke(title, message);
        }

        public void Clear()
        {
            OnClear?.Invoke();
        }
    }
}
