// src/components/layout/FreelancerHeader.jsx
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useState } from 'react';
import AssistantWidget from '../../pages/freelancer/AssistantWidget';

export default function FreelancerHeader() {
  const { user, signout } = useAuth();
  const navigate = useNavigate();
  const [searchType, setSearchType] = useState('projects');
  const [searchValue, setSearchValue] = useState('');
  const [isAssistantOpen, setIsAssistantOpen] = useState(false);

  const handleLogout = () => {
    signout();
    navigate('/signin');
  };

  const handleSearchSubmit = () => {
    const query = searchValue.trim();
    if (!query) return;

    if (searchType === 'projects') {
      navigate(`/freelancer/projects?search=${encodeURIComponent(query)}`);
    } else if (searchType === 'people') {
      navigate(`/freelancer/people?search=${encodeURIComponent(query)}`);
    }
  };

  // Переход на профиль фрилансера при клике на аватарку или имя
  const goToProfile = () => {
    navigate('/freelancer/profile');
  };

  return (
    <header className="bg-white shadow-sm border-b border-gray-200 sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">

          {/* ЛЕВО: Логотип + Assistant + Сообщения */}
          <div className="flex items-center space-x-6">
            <Link to="/home" className="flex items-center">
              <span className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
                Connect!
              </span>
            </Link>

            {/* Assistant */}
            <button
              onClick={() => setIsAssistantOpen(!isAssistantOpen)}
              className="relative flex items-center gap-1 p-2 text-gray-600 hover:text-indigo-600 transition rounded-lg hover:bg-gray-100"
            >
              <span className="text-xs font-medium">Assistant</span>
              <span className="absolute -top-1 -right-1 w-2 h-2 bg-red-500 rounded-full"></span>
            </button>

            {/* Сообщения */}
            <button className="relative p-2 text-gray-600 hover:text-indigo-600 transition">
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
              </svg>
              <span className="absolute -top-1 -right-1 w-2 h-2 bg-red-500 rounded-full"></span>
            </button>
          </div>

          {/* ПРАВО: Поиск + Профиль + Logout */}
          <div className="flex items-center space-x-4">
            {/* Поиск */}
            <div className="relative">
              <input
                type="text"
                placeholder={searchType === 'projects' ? 'Search projects...' : 'Search people...'}
                value={searchValue}
                onChange={(e) => setSearchValue(e.target.value)}
                onKeyDown={(e) => e.key === 'Enter' && handleSearchSubmit()}
                className="w-48 lg:w-64 pl-10 pr-12 py-2 border border-gray-300 rounded-lg focus:border-indigo-500 focus:ring-2 focus:ring-indigo-200 transition outline-none"
              />
              <button
                onClick={handleSearchSubmit}
                className="absolute left-3 top-2.5 text-gray-400 hover:text-indigo-600 transition"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </button>
              <select
                value={searchType}
                onChange={(e) => setSearchType(e.target.value)}
                className="absolute right-2 top-2 text-xs bg-gray-100 text-gray-600 rounded px-2 py-0.5 focus:outline-none cursor-pointer"
              >
                <option value="projects">Projects</option>
                <option value="people">People</option>
              </select>
            </div>

            {/* Профиль — теперь кликабельный целиком */}
            <button
              onClick={goToProfile}
              className="flex items-center space-x-3 hover:bg-gray-100 rounded-lg px-3 py-2 transition group"
            >
              <div className="w-10 h-10 bg-gradient-to-br from-indigo-400 to-purple-400 rounded-full flex-shrink-0"></div>
              <div className="text-left">
                <p className="text-sm font-medium text-gray-900 group-hover:text-indigo-600 transition">
                  {user?.firstName} {user?.lastName}
                </p>
                <p className="text-xs text-gray-500">Freelancer</p>
              </div>
            </button>

            {/* Logout */}
            <button
              onClick={handleLogout}
              className="px-4 py-2 text-sm font-medium text-gray-700 hover:text-red-600 hover:bg-red-50 rounded-lg transition"
            >
              Log out
            </button>
          </div>
        </div>
      </div>

      {/* Ассистент — модалка */}
      {isAssistantOpen && (
        <div className="fixed inset-0 z-50" onClick={() => setIsAssistantOpen(false)}>
          <div
            className="absolute top-20 right-4 w-96 bg-white rounded-2xl shadow-2xl p-6 space-y-6 animate-in slide-in-from-top-2 duration-300"
            onClick={(e) => e.stopPropagation()}
          >
            <AssistantWidget onClose={() => setIsAssistantOpen(false)} />
          </div>
        </div>
      )}
    </header>
  );
}