import React, { createContext, useContext, useState, useEffect } from 'react';

// Define User interface matching backend response
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[]; // Roles as string array
}

interface AuthContextType {
  user: User | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Safe load user from localStorage with error handling
  useEffect(() => {
    try {
      const storedUser = localStorage.getItem('sms_user');
      if (storedUser && storedUser !== "undefined") {
        setUser(JSON.parse(storedUser));
      }
    } catch (error) {
      console.error("Error parsing stored user", error);
      localStorage.removeItem('sms_user');
    }
    setIsLoading(false);
  }, []);

  const login = async (email: string, password: string) => {
    setIsLoading(true);
    try {
      const response = await fetch('https://localhost:7045/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      });

      if (!response.ok) {
        throw new Error("Invalid credentials");
      }

      const result = await response.json();

      if (!result.succeeded) {
        throw new Error(result.errors?.join(", ") || "Login failed");
      }

      const userFromApi = result.data.user;

      const mappedUser: User = {
        id: userFromApi.id,
        email: userFromApi.email,
        firstName: userFromApi.firstName,
        lastName: userFromApi.lastName,
        roles: Array.isArray(userFromApi.roles) ? userFromApi.roles : []
      };

      setUser(mappedUser);
      localStorage.setItem("sms_user", JSON.stringify(mappedUser));
      localStorage.setItem("sms_token", result.data.accessToken);
      localStorage.setItem("sms_refresh_token", result.data.refreshToken);

    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('sms_user');
    localStorage.removeItem('sms_token');
    localStorage.removeItem('sms_refresh_token');
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
