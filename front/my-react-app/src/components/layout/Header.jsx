// src/components/layout/Header.jsx
import { useAuth } from '../../hooks/useAuth';
import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';

export default function Header() {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [searchType, setSearchType] = useState('jobs');

  const scrollToCategories = () => {
    const element = document.getElementById('categories');
    element?.scrollIntoView({ behavior: 'smooth' });
  };

  return (
    <header className="bg-white shadow-sm border-b border-gray-100 sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link to="/" className="flex items-center space-x-2">
            <span className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
              Connect!
            </span>
          </Link>

          {/* Navigation */}
          <nav className="hidden md:flex items-center space-x-8">
            <button onClick={() => navigate('/')} className="text-gray-700 hover:text-indigo-600 font-medium transition">
              Home
            </button>
            <button onClick={scrollToCategories} className="text-gray-700 hover:text-indigo-600 font-medium transition">
              Categories
            </button>
            <Link
              to={isAuthenticated ? '/jobs' : '/signin'}
              className="text-gray-700 hover:text-indigo-600 font-medium transition"
            >
              Jobs
            </Link>
            <Link
              to={isAuthenticated ? '/freelancers' : '/signin'}
              className="text-gray-700 hover:text-indigo-600 font-medium transition"
            >
              Freelancers
            </Link>
          </nav>

          {/* Search + Login */}
          <div className="flex items-center space-x-4">
            <div className="relative">
              <input
                type="text"
                placeholder="Search..."
                className="w-48 lg:w-64 pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:border-indigo-500 focus:ring-2 focus:ring-indigo-200 transition"
              />
              <svg className="absolute left-3 top-2.5 w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
              <select
                value={searchType}
                onChange={(e) => setSearchType(e.target.value)}
                className="absolute right-2 top-2 text-xs bg-gray-100 text-gray-600 rounded px-2 py-0.5"
              >
                <option value="jobs">Jobs</option>
                <option value="people">People</option>
              </select>
            </div>

            <Link
              to="/signin"
              className="px-5 py-2 bg-gradient-to-r from-indigo-600 to-purple-600 text-white font-medium rounded-lg hover:shadow-lg transition transform hover:scale-105"
            >
              Login
            </Link>
          </div>
        </div>
      </div>
    </header>
  );
}