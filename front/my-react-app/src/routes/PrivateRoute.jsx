// import React from 'react';
// import { Navigate } from 'react-router-dom';
// import { useAuth } from '../hooks/useAuth';

// const PrivateRoute = ({ children }) => {
//   const { isAuthenticated, loading } = useAuth();
  
//   if (loading) {
//     return <div>Loading...</div>; // Или компонент лоадера
//   }
  
//   if (!isAuthenticated) {
//     return <Navigate to="/signin" replace />;
//   }
  
//   return children;
// };

// export default PrivateRoute;
// src/routes/PrivateRoute.jsx
import { useAuth } from '../hooks/useAuth';
import { Navigate, Outlet } from 'react-router-dom';

export default function PrivateRoute() {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/signin" replace />;
  }

  if (user?.role !== 'freelancer') {
    return <Navigate to="/" replace />;
  }

   return <Outlet />;
}