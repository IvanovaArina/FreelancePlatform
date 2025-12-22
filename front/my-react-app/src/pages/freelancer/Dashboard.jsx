// src/pages/freelancer/Dashboard.jsx
import { useAuth } from '../../hooks/useAuth';
import { ActionButton } from '../../components/ui';

export default function FreelancerDashboard() {
  const { user } = useAuth();
  const firstName = user?.firstName || 'John';

  return (
    <div className="max-w-6xl mx-auto space-y-8 pb-12">

      {/* === Заголовок + кнопка People === */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Welcome back, {firstName}!</h1>
          <p className="text-sm text-gray-600 mt-1">Here’s what’s happening with your freelance business</p>
        </div>
      </div>

      {/* === Основные метрики === */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        
        {/* Total Earnings */}
        <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-6 flex items-center space-x-4">
          <div className="text-3xl">💵</div>
          <div>
            <p className="text-2xl font-bold text-gray-900">
              $24,580 <span className="text-sm font-normal text-green-600">/$120</span>
            </p>
            <p className="text-sm text-gray-600">Total Earnings /1mo</p>
          </div>
        </div>

        {/* Completed Projects */}
        <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-6 flex items-center space-x-4">
          <div className="text-3xl">✅</div>
          <div>
            <p className="text-2xl font-bold text-gray-900">47</p>
            <p className="text-sm text-gray-600">Completed Projects</p>
          </div>
        </div>

        {/* Success Rate */}
        <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-6 flex items-center space-x-4">
          <div className="text-3xl">📈</div>
          <div>
            <p className="text-2xl font-bold text-gray-900">98%</p>
            <p className="text-sm text-gray-600">Success Rate</p>
          </div>
        </div>

        {/* Active Projects */}
        <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-6 flex items-center space-x-4">
          <div className="text-3xl">🧳</div>
          <div>
            <p className="text-2xl font-bold text-gray-900">5</p>
            <p className="text-sm text-gray-600">Active Projects</p>
          </div>
        </div>
      </div>

      {/* === Subscription Analytics === */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <h2 className="text-xl font-bold text-gray-900 mb-6">Subscription Analytics</h2>

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-6">
          
          {/* Total Subscribers */}
          <div className="bg-blue-50 rounded-xl p-6 text-center">
            <p className="text-3xl font-bold text-blue-700">10</p>
            <p className="text-sm text-blue-600 mt-1">Total Subscribers</p>
          </div>

          {/* Monthly Recurring */}
          <div className="bg-green-50 rounded-xl p-6 text-center">
            <p className="text-3xl font-bold text-green-700">$1,597</p>
            <p className="text-sm text-green-600 mt-1">Monthly Recurring</p>
          </div>

          {/* Annual Revenue */}
          <div className="bg-orange-50 rounded-xl p-6 text-center">
            <p className="text-3xl font-bold text-orange-700">$19,164</p>
            <p className="text-sm text-orange-600 mt-1">Annual Revenue</p>
          </div>
        </div>
      </div>
    </div>
  );
}