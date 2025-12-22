// src/routes/PublicRoute.jsx
import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';  // или '../components/context/AuthContext'

const PublicRoute = ({ children }) => {
  const { isAuthenticated } = useAuth();

  // Пока нет дашборда — просто показываем страницу (даже если залогинен)
  // Позже можно будет заменить на /dashboard
  if (isAuthenticated) {
    return children; // или <Navigate to="/dashboard" /> — когда будет дашборд
  }

  return children;
};

export default PublicRoute;