// src/components/layout/FreelancerLayout.jsx
import { Outlet } from 'react-router-dom';
import FreelancerHeader from './FreelancerHeader';
import Sidebar from './Sidebar';

export default function FreelancerLayout() {
  console.log('FreelancerLayout render', { path: window.location.pathname });
  return (
    <div className="min-h-screen bg-gray-50">
      <FreelancerHeader />
      <div className="flex">
        <Sidebar />
        <main className="flex-1 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
