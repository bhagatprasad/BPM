using BPM.Web.Operations.UI.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace BPM.Web.Operations.UI.Helper
{
    public class SessionManager
    {
        private readonly string _sessionFilePath;
        private AuthResponse _cachedAuthResponse;
        private readonly object _lock = new object();

        public SessionManager()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "BPM",
                "Operations"
            );
            Directory.CreateDirectory(appDataPath);
            _sessionFilePath = Path.Combine(appDataPath, "session.json");
            LoadSession();
        }

        public AuthResponse GetAuthResponse()
        {
            lock (_lock)
            {
                return _cachedAuthResponse;
            }
        }

        public void SetAuthResponse(AuthResponse authResponse)
        {
            lock (_lock)
            {
                _cachedAuthResponse = authResponse;
                SaveSession();
            }
        }

        public void SetToken(string jwtToken, string refreshToken)
        {
            lock (_lock)
            {
                if (_cachedAuthResponse != null)
                {
                    _cachedAuthResponse.JwtToken = jwtToken;
                    _cachedAuthResponse.RefreshToken = refreshToken;
                    SaveSession();
                }
            }
        }

        public void ClearSession()
        {
            lock (_lock)
            {
                _cachedAuthResponse = null;
                if (File.Exists(_sessionFilePath))
                {
                    File.Delete(_sessionFilePath);
                }
            }
        }

        public bool IsAuthenticated()
        {
            lock (_lock)
            {
                return _cachedAuthResponse != null &&
                       !string.IsNullOrWhiteSpace(_cachedAuthResponse.JwtToken);
            }
        }

        private void LoadSession()
        {
            try
            {
                if (File.Exists(_sessionFilePath))
                {
                    var json = File.ReadAllText(_sessionFilePath);
                    _cachedAuthResponse = JsonConvert.DeserializeObject<AuthResponse>(json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading session: {ex.Message}");
                _cachedAuthResponse = null;
            }
        }

        private void SaveSession()
        {
            try
            {
                if (_cachedAuthResponse != null)
                {
                    var json = JsonConvert.SerializeObject(_cachedAuthResponse);
                    File.WriteAllText(_sessionFilePath, json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving session: {ex.Message}");
            }
        }
    }
}