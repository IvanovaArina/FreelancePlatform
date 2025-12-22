import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import SignUpPage from '../pages/auth/SignUp';
import SignInPage from '../pages/auth/SignIn';
//import LandingPage from '../pages/LandingPage';
import HomePage from '../pages/HomePage';
import ForgotPasswordPage from '../pages/auth/ForgotPassword';
import DashboardFreelancer from '../pages/freelancer/Dashboard';
import FreelancerLayout from '../components/layout/FreelancerLayout';
import Profile from '../pages/freelancer/Profile';
import Projects from '../pages/freelancer/Projects';
import Subscriptions from '../pages/freelancer/Subscriptions';
import Verification from '../pages/freelancer/Verification';
import People from '../pages/freelancer/People';

import PrivateRoute from './PrivateRoute';
import PublicRoute from './PublicRoute';

const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public routes */}
        <Route path="/" element={<Navigate to="/home" replace />} />
        <Route path="/home" element={<HomePage />} />

        {/* <Route 
          path="/home" 
          element={
            <PublicRoute>
              <LandingPage />
            </PublicRoute>
          } 
        /> */}
        
        <Route 
          path="/signup" 
          element={
            <PublicRoute>
              <SignUpPage />
            </PublicRoute>
          } 
        />
        
        <Route 
          path="/signin" 
          element={
            <PublicRoute>
              <SignInPage />
            </PublicRoute>
          } 
        />
        
        <Route 
          path="/forgot-password" 
          element={
            <PublicRoute>
              <ForgotPasswordPage />
            </PublicRoute>
          } 
        />

        {/* Private routes - будем добавлять по мере создания страниц */}
        {/* 
        <Route 
          path="/dashboard" 
          element={
            <PrivateRoute>
              <Dashboard />
            </PrivateRoute>
          } 
        />
        */}
        {/* === ФРИЛАНСЕР === */}
        <Route element={<PrivateRoute />}>
          <Route element={<FreelancerLayout />}>
            <Route path="/freelancer/dashboard" element={<DashboardFreelancer />} />
            <Route path="/freelancer/projects" element={<Projects/>} />
            <Route path="/freelancer/profile" element={<Profile/>} />
            <Route path="/freelancer/subscriptions" element={<Subscriptions/>} />
            <Route path="/freelancer/verification" element={<Verification />} />
            <Route path="/freelancer/people" element={<People />} />
          </Route>
        </Route>

        {/* 404 */}
        <Route path="*" element={<div>404 - Page Not Found</div>} />
      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;