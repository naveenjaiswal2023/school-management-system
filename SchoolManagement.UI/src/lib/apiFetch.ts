// src/lib/apiFetch.ts
export async function apiFetch<T = any>(
  endpoint: string,
  options: RequestInit = {}
): Promise<T> {
  const token = localStorage.getItem("sms_token");
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  if (!baseUrl) {
    throw new Error("❌ Missing VITE_API_BASE_URL in environment configuration");
  }

  const url = `${baseUrl}${endpoint.startsWith("/") ? endpoint : `/${endpoint}`}`;

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...(options.headers instanceof Headers
      ? Object.fromEntries(options.headers.entries())
      : (options.headers as Record<string, string>) ?? {}),
  };

  if (token) headers["Authorization"] = `Bearer ${token}`;

  // Timeout controller (10s)
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), 10000);

  try {
    let response = await fetch(url, {
      ...options,
      headers,
      signal: controller.signal,
    });

    clearTimeout(timeoutId);

    // Handle unauthorized
    if (response.status === 401) {
      const refreshed = await tryRefreshToken();
      if (refreshed) {
        const newToken = localStorage.getItem("sms_token");
        if (newToken) headers["Authorization"] = `Bearer ${newToken}`;

        // Retry request once
        response = await fetch(url, { ...options, headers });
      }

      if (response.status === 401) {
        console.warn("Session expired. Redirecting to login...");
        clearAuthStorage();
        window.location.href = "/login";
        throw new Error("Unauthorized - Redirecting to login");
      }
    }

    // Check API errors
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`API error: ${response.status} - ${errorText}`);
    }

    // ✅ Parse JSON automatically
    const data = await response.json().catch(() => null);
    return data as T;
  } catch (error: any) {
    clearTimeout(timeoutId);

    if (error.name === "AbortError") {
      throw new Error("⏱️ Request timed out. Please try again.");
    }

    console.error("API Fetch Error:", error);
    throw error;
  }
}

// 🔁 Token refresh logic
async function tryRefreshToken(): Promise<boolean> {
  const refreshToken = localStorage.getItem("sms_refresh_token");
  if (!refreshToken) return false;

  try {
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Auth/refresh`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ refreshToken }),
    });

    if (!response.ok) return false;

    const result = await response.json();

    if (result?.data?.accessToken) {
      localStorage.setItem("sms_token", result.data.accessToken);
      localStorage.setItem("sms_refresh_token", result.data.refreshToken);
      return true;
    }

    return false;
  } catch (error) {
    console.error("🔁 Token refresh failed:", error);
    return false;
  }
}

// 🧹 Clear all auth data
function clearAuthStorage() {
  localStorage.removeItem("sms_user");
  localStorage.removeItem("sms_token");
  localStorage.removeItem("sms_refresh_token");
}
