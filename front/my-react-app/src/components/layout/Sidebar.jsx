// src/components/layout/Sidebar.jsx
import { NavLink } from 'react-router-dom';

const navItems = [
  { to: '/freelancer/profile', label: 'Profile', icon: '👤' },
  { to: '/freelancer/dashboard', label: 'Dashboards', icon: '📊' },
  { to: '/freelancer/projects', label: 'Projects', icon: '📁' },
  { to: '/freelancer/subscriptions', label: 'Subscriptions', icon: '💳' },
  
  
];

export default function Sidebar() {
  return (
    <aside className="w-64 bg-gray-50 border-r border-gray-200 min-h-screen">
      <nav className="p-4 space-y-1">
        {navItems.map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            className={({ isActive }) =>
              `flex items-center space-x-3 px-4 py-3 rounded-lg text-sm font-medium transition ${
                isActive
                  ? 'bg-indigo-100 text-indigo-700'
                  : 'text-gray-700 hover:bg-gray-100'
              }`
            }
          >
            <span className="text-xl">{item.icon}</span>
            <span>{item.label}</span>
          </NavLink>
        ))}
      </nav>
    </aside>
  );
}