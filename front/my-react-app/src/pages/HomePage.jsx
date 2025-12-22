// src/pages/HomePage.jsx
import { useAuth } from '../hooks/useAuth';
import LandingPage from './LandingPage';
import AuthenticatedLandingPage from './AuthenticatedLandingPage';

export default function HomePage() {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <AuthenticatedLandingPage /> : <LandingPage />;
}