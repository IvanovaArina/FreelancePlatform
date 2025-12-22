//src/context/AuthContext.jsx

import React, { createContext, useState } from "react";
import { authApi } from "../api/authApi";

export const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(
    JSON.parse(localStorage.getItem("user")) || null
  );
  const [token, setToken] = useState(localStorage.getItem("token") || null);

  const saveAuth = (auth, remember) => {
    setUser(auth.user);
    setToken(auth.token);

    if (remember) {
      localStorage.setItem("user", JSON.stringify(auth.user));
      localStorage.setItem("token", auth.token);
      console.log("%c[AUTH] Saved to localStorage", "color: #4ade80");
    }

    console.log("%c[AUTH] Logged in as:", "color: #22d3ee", auth.user);
  };

  // 🔹 LOGIN
  const signin = async (formData) => {
    console.log("[AUTH] Signin request:", formData);

    const { data } = await authApi.signin({
      email: formData.email,
      password: formData.password,
      rememberMe: formData.remember
    });

    saveAuth(
      { user: data.user, token: data.token },
      formData.remember
    );

    return data;
  };

  // 🔹 SIGNUP (автоматически логинит)
  const signup = async (formData) => {
    console.log("[AUTH] Signup request:", formData);

    const { data } = await authApi.signup({
      name: formData.name,
      email: formData.email,
      password: formData.password,
      confirmPassword: formData.confirmPassword,
      role: formData.role
    });

    console.log("%c[AUTH] Registration successful", "color: #a78bfa");

    saveAuth(
      { user: data.user, token: data.token },
      true // после регистрации запоминаем
    );

    return data;
  };

  const signout = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    setUser(null);
    setToken(null);

    console.log("%c[AUTH] Logged out", "color: #f87171");
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isAuthenticated: !!user,
        signin,
        signup,
        signout
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
